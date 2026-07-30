"""Build the first quality-first game asset set from approved imagegen boards.

The script does not redraw the assets with geometric placeholders.  It removes
residual matte pixels, separates board modules, rectifies the isometric floor
surfaces into seamless source textures, and places the approved artwork on
shared production canvases.
"""

from __future__ import annotations

import hashlib
import json
from pathlib import Path

import cv2
import numpy as np
from PIL import Image, ImageDraw, ImageFont, ImageOps


ROOT = Path(__file__).resolve().parents[1]
CLEAN = ROOT / "source" / "cleaned-boards"
OUTPUT = ROOT / "artifacts" / "game-assets"
PREVIEWS = ROOT / "artifacts" / "previews"
QUALITY_ROOT = (
    ROOT.parent
    / "2026-07-30-rat-host-2d-quality-first-vertical-slice"
    / "artifacts"
    / "quality-masters"
)

ENV_OUT = OUTPUT / "environment"
RAT_OUT = OUTPUT / "rat"
HUD_OUT = OUTPUT / "hud"

GROUNDLINE_TOP = 152
RAT_PIVOT_BOTTOM_LEFT = (128, 40)
RAT_FRAMES = ("neutral", "contact", "passing")


def ensure_dirs() -> None:
    for path in (ENV_OUT, RAT_OUT, HUD_OUT, PREVIEWS):
        path.mkdir(parents=True, exist_ok=True)


def matte_cleanup(image: Image.Image) -> Image.Image:
    rgba = image.convert("RGBA")
    array = np.array(rgba)
    alpha = array[:, :, 3]
    alpha[alpha < 24] = 0
    alpha[alpha > 232] = 255
    array[:, :, 3] = alpha
    array[alpha == 0, :3] = 0
    return Image.fromarray(array, "RGBA")


def neutralize_green_halo(image: Image.Image) -> Image.Image:
    """Convert residual green-key edge pixels to warm whisker/fur neutrals."""
    array = np.array(image.convert("RGBA"))
    rgb = array[:, :, :3].astype(np.int16)
    alpha = array[:, :, 3]
    red, green, blue = rgb[:, :, 0], rgb[:, :, 1], rgb[:, :, 2]
    mask = (
        (alpha > 16)
        & (green > 65)
        & (green * 100 > red * 118)
        & (green * 100 > blue * 150)
    )
    value = np.maximum(red, (green * 72) // 100)
    array[mask, 0] = np.clip(value[mask], 0, 255)
    array[mask, 1] = np.clip((value[mask] * 84) // 100, 0, 255)
    array[mask, 2] = np.clip((value[mask] * 66) // 100, 0, 255)
    return Image.fromarray(array, "RGBA")


def remove_portrait_top_artifact(image: Image.Image) -> Image.Image:
    """Remove a detached frame fragment above the portrait without resampling.

    The imagegen HUD board contains two independent left-side components: the
    portrait frame and the rat portrait.  The original broad crop retained the
    frame's bottom ornament.  At production size that ornament is a detached
    alpha component above the much larger rat component.  Clear only rows
    occupied by detached components that end before the rat begins; every rat
    pixel remains byte-identical.
    """
    array = np.array(image.convert("RGBA"))
    mask = (array[:, :, 3] > 24).astype(np.uint8)
    count, _labels, stats, _centroids = cv2.connectedComponentsWithStats(mask, 8)
    if count <= 2:
        return image
    components = list(range(1, count))
    largest = max(components, key=lambda index: int(stats[index, cv2.CC_STAT_AREA]))
    rat_top = int(stats[largest, cv2.CC_STAT_TOP])
    detached_above = [
        index
        for index in components
        if index != largest
        and int(stats[index, cv2.CC_STAT_TOP] + stats[index, cv2.CC_STAT_HEIGHT])
        < rat_top
    ]
    if not detached_above:
        return image
    clear_through = max(
        int(stats[index, cv2.CC_STAT_TOP] + stats[index, cv2.CC_STAT_HEIGHT])
        for index in detached_above
    )
    array[:clear_through, :, :] = 0
    return Image.fromarray(array, "RGBA")


def alpha_bbox(image: Image.Image, threshold: int = 24) -> tuple[int, int, int, int]:
    alpha = np.array(image.getchannel("A"))
    ys, xs = np.where(alpha > threshold)
    if not len(xs):
        raise ValueError("no visible pixels")
    return int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1


def trim(image: Image.Image, padding: int = 0) -> Image.Image:
    left, top, right, bottom = alpha_bbox(image)
    return image.crop(
        (
            max(0, left - padding),
            max(0, top - padding),
            min(image.width, right + padding),
            min(image.height, bottom + padding),
        )
    )


def fit_nearest(
    image: Image.Image,
    canvas_size: tuple[int, int],
    max_size: tuple[int, int],
    *,
    bottom: int | None = None,
    center_x: int | None = None,
) -> Image.Image:
    source = trim(matte_cleanup(image), 1)
    ratio = min(max_size[0] / source.width, max_size[1] / source.height)
    size = max(1, round(source.width * ratio)), max(1, round(source.height * ratio))
    source = source.resize(size, Image.Resampling.NEAREST)
    canvas = Image.new("RGBA", canvas_size, (0, 0, 0, 0))
    x = (canvas_size[0] - size[0]) // 2 if center_x is None else center_x - size[0] // 2
    y = (canvas_size[1] - size[1]) // 2 if bottom is None else bottom - size[1]
    canvas.alpha_composite(source, (x, y))
    return matte_cleanup(canvas)


def rectify_quad(
    source: Image.Image,
    points: tuple[float, float, float, float, float, float, float, float],
    size: int = 256,
) -> Image.Image:
    # Pillow QUAD order: upper-left, lower-left, lower-right, upper-right.
    result = source.transform(
        (size, size),
        Image.Transform.QUAD,
        points,
        resample=Image.Resampling.BICUBIC,
    )
    result.putalpha(Image.new("L", (size, size), 255))
    return result


def seamless_edges(texture: Image.Image, band: int = 14) -> Image.Image:
    """Make opposite texture edges pixel-identical with a narrow crossfade."""
    data = np.array(texture.convert("RGBA"), dtype=np.float32)
    height, width, _ = data.shape
    for index in range(band):
        weight = index / max(1, band - 1)
        left = data[:, index].copy()
        right = data[:, width - 1 - index].copy()
        blend = left * (0.5 + 0.5 * weight) + right * (0.5 - 0.5 * weight)
        reverse = right * (0.5 + 0.5 * weight) + left * (0.5 - 0.5 * weight)
        data[:, index] = blend
        data[:, width - 1 - index] = reverse
    edge = ((data[:, 0] + data[:, -1]) / 2).astype(np.uint8)
    data[:, 0] = edge
    data[:, -1] = edge
    for index in range(band):
        weight = index / max(1, band - 1)
        top = data[index].copy()
        bottom = data[height - 1 - index].copy()
        blend = top * (0.5 + 0.5 * weight) + bottom * (0.5 - 0.5 * weight)
        reverse = bottom * (0.5 + 0.5 * weight) + top * (0.5 - 0.5 * weight)
        data[index] = blend
        data[height - 1 - index] = reverse
    edge = ((data[0] + data[-1]) / 2).astype(np.uint8)
    data[0] = edge
    data[-1] = edge
    return Image.fromarray(np.clip(data, 0, 255).astype(np.uint8), "RGBA")


def square_to_iso(texture: Image.Image, width: int = 128, height: int = 64) -> Image.Image:
    texture = texture.convert("RGBA")
    source = np.array(texture)
    out = np.zeros((height, width, 4), dtype=np.uint8)
    cx = (width - 1) / 2
    cy = (height - 1) / 2
    for y in range(height):
        for x in range(width):
            dx = (x - cx) / width
            dy = (y - cy) / height
            u = 0.5 + dx + dy
            v = 0.5 - dx + dy
            if 0 <= u <= 1 and 0 <= v <= 1:
                sx = min(texture.width - 1, max(0, round(u * (texture.width - 1))))
                sy = min(texture.height - 1, max(0, round(v * (texture.height - 1))))
                out[y, x] = source[sy, sx]
                out[y, x, 3] = 255
    return Image.fromarray(out, "RGBA")


def save_png(image: Image.Image, path: Path) -> None:
    matte_cleanup(image).save(path, optimize=True)


def split_major_components(image: Image.Image, count: int) -> list[Image.Image]:
    cleaned = matte_cleanup(image)
    array = np.array(cleaned)
    mask = (array[:, :, 3] > 32).astype(np.uint8)
    labels_count, labels, stats, centroids = cv2.connectedComponentsWithStats(mask, 8)
    components = [
        (index, int(stats[index, cv2.CC_STAT_AREA]))
        for index in range(1, labels_count)
        if stats[index, cv2.CC_STAT_AREA] >= 3
    ]
    anchors = sorted(components, key=lambda item: item[1], reverse=True)[:count]
    if len(anchors) != count:
        raise ValueError(f"expected {count} components, found {len(anchors)}")
    anchors = sorted(anchors, key=lambda item: centroids[item[0]][0])
    groups = [np.zeros(mask.shape, dtype=bool) for _ in anchors]
    anchor_points = [centroids[index] for index, _ in anchors]
    for component, _area in components:
        point = centroids[component]
        nearest = min(
            range(count),
            key=lambda i: (point[0] - anchor_points[i][0]) ** 2
            + (point[1] - anchor_points[i][1]) ** 2,
        )
        groups[nearest] |= labels == component
    results: list[Image.Image] = []
    for group in groups:
        output = array.copy()
        output[~group] = 0
        results.append(trim(Image.fromarray(output, "RGBA"), 2))
    return results


def build_environment() -> dict[str, Image.Image]:
    board = matte_cleanup(Image.open(CLEAN / "environment-tile-source-board-alpha.png"))
    # Source quadrilaterals are the top surfaces only.  The generated slab side
    # shadow is intentionally excluded so floor repeats do not bake drop shadows.
    # Sample several pixels inside the generated slab outline.  Capturing the
    # outer black rim would bake a dark diamond around every repeated cell.
    clean_texture = rectify_quad(board, (282, 151, 66, 277, 287, 397, 502, 277))
    worn_texture = rectify_quad(board, (779, 149, 569, 273, 790, 398, 1005, 274))
    water_texture = rectify_quad(board, (777, 626, 575, 745, 779, 869, 981, 745))
    clean_texture = seamless_edges(clean_texture)
    worn_texture = seamless_edges(worn_texture)
    water_texture = seamless_edges(water_texture)

    assets: dict[str, Image.Image] = {
        "floor_clean_128x64.png": square_to_iso(clean_texture),
        "floor_worn_128x64.png": square_to_iso(worn_texture),
        "water_center_128x64.png": square_to_iso(water_texture),
    }

    straight = board.crop((1024, 0, 1536, 512))
    corner = board.crop((0, 512, 512, 1024))
    water_edge = board.crop((1024, 512, 1536, 1024))
    assets["wall_straight_160x160.png"] = fit_nearest(
        straight, (160, 160), (148, 150), bottom=156
    )
    assets["wall_corner_192x160.png"] = fit_nearest(
        corner, (192, 160), (184, 148), bottom=156
    )
    assets["water_edge_128x96.png"] = fit_nearest(
        water_edge, (128, 96), (124, 90), bottom=94
    )

    props = split_major_components(
        Image.open(CLEAN / "props-source-board-alpha.png"), 3
    )
    assets["prop_barrel_96x112.png"] = fit_nearest(
        props[0], (96, 112), (90, 106), bottom=110
    )
    assets["prop_crate_112x112.png"] = fit_nearest(
        props[1], (112, 112), (106, 106), bottom=110
    )
    assets["prop_drain_128x80.png"] = fit_nearest(
        props[2], (128, 80), (122, 74), bottom=77
    )
    for name, image in assets.items():
        save_png(image, ENV_OUT / name)
    return assets


def build_rat() -> dict[str, Image.Image]:
    board = Image.open(CLEAN / "rat-side-walk-source-board-alpha.png")
    frames = split_major_components(board, 3)
    assets: dict[str, Image.Image] = {}
    canvases: list[Image.Image] = []
    for name, frame in zip(RAT_FRAMES, frames):
        canvas = fit_nearest(
            frame,
            (256, 192),
            (238, 106),
            bottom=GROUNDLINE_TOP,
            center_x=128,
        )
        canvas = neutralize_green_halo(canvas)
        filename = f"rat_side_{name}_256x192.png"
        save_png(canvas, RAT_OUT / filename)
        assets[filename] = canvas
        canvases.append(canvas)
    sheet = Image.new("RGBA", (768, 192), (0, 0, 0, 0))
    for index, canvas in enumerate(canvases):
        sheet.alpha_composite(canvas, (index * 256, 0))
    save_png(sheet, RAT_OUT / "rat_side_walk_3f_sheet.png")
    assets["rat_side_walk_3f_sheet.png"] = sheet

    frame_map = {
        "schema": 1,
        "direction": "side_right",
        "canvas": {"width": 256, "height": 192},
        "groundline_top_y": GROUNDLINE_TOP,
        "pivot": {
            "coordinate_system": "bottom-left",
            "pixels": list(RAT_PIVOT_BOTTOM_LEFT),
            "normalized": [0.5, round(40 / 192, 6)],
        },
        "frames": [
            {
                "name": name,
                "index": index,
                "rect": {"x": index * 256, "y": 0, "width": 256, "height": 192},
                "source": f"rat_side_{name}_256x192.png",
            }
            for index, name in enumerate(RAT_FRAMES)
        ],
        "status": "production-v1 candidate; not final PPU",
    }
    (RAT_OUT / "rat_side_walk_3f_frame-map.json").write_text(
        json.dumps(frame_map, ensure_ascii=False, indent=2) + "\n",
        encoding="utf-8",
    )
    return assets


def build_hud() -> dict[str, Image.Image]:
    board = matte_cleanup(Image.open(CLEAN / "hud-module-source-board-alpha.png"))
    regions = {
        "hud_portrait_frame_256.png": board.crop((0, 0, 560, 560)),
        "hud_rat_portrait_184.png": board.crop((0, 500, 560, 940)),
        "hud_bar_frame_512x80.png": board.crop((540, 0, 1673, 390)),
        "hud_health_fill_400x52.png": board.crop((540, 360, 1673, 625)),
        "hud_immune_fill_400x52.png": board.crop((540, 590, 1673, 940)),
    }
    specs = {
        "hud_portrait_frame_256.png": ((256, 256), (250, 250)),
        "hud_rat_portrait_184.png": ((184, 184), (178, 174)),
        "hud_bar_frame_512x80.png": ((512, 80), (506, 76)),
        "hud_health_fill_400x52.png": ((400, 52), (396, 48)),
        "hud_immune_fill_400x52.png": ((400, 52), (396, 48)),
    }
    assets: dict[str, Image.Image] = {}
    for name, region in regions.items():
        canvas, maximum = specs[name]
        image = fit_nearest(region, canvas, maximum)
        if name == "hud_rat_portrait_184.png":
            image = remove_portrait_top_artifact(image)
        save_png(image, HUD_OUT / name)
        assets[name] = image
    layout = {
        "portrait": {
            "frame": "hud_portrait_frame_256.png",
            "subject": "hud_rat_portrait_184.png",
            "subject_offset_top_left": [36, 40],
        },
        "bar": {
            "frame": "hud_bar_frame_512x80.png",
            "fill_offset_top_left": [56, 14],
            "fill_size": [400, 52],
            "health_fill": "hud_health_fill_400x52.png",
            "immune_fill": "hud_immune_fill_400x52.png",
        },
        "display_scale_candidate": 0.5,
    }
    (HUD_OUT / "hud_module-layout.json").write_text(
        json.dumps(layout, ensure_ascii=False, indent=2) + "\n",
        encoding="utf-8",
    )
    return assets


def checker(size: tuple[int, int], cell: int = 12) -> Image.Image:
    image = Image.new("RGBA", size, (24, 24, 26, 255))
    draw = ImageDraw.Draw(image)
    for y in range(0, size[1], cell):
        for x in range(0, size[0], cell):
            if (x // cell + y // cell) % 2:
                draw.rectangle((x, y, x + cell - 1, y + cell - 1), fill=(42, 43, 46, 255))
    return image


def label(draw: ImageDraw.ImageDraw, position: tuple[int, int], text: str) -> None:
    draw.text(position, text, fill=(224, 207, 170, 255), font=ImageFont.load_default())


def iso_grid_preview(
    tiles: list[Image.Image], columns: int, rows: int, panel_size: tuple[int, int]
) -> Image.Image:
    panel = Image.new("RGBA", panel_size, (12, 14, 15, 255))
    origin_x = panel_size[0] // 2
    origin_y = 28
    index = 0
    for total in range(columns + rows - 1):
        for gy in range(rows):
            gx = total - gy
            if 0 <= gx < columns:
                tile = tiles[index % len(tiles)]
                x = origin_x + (gx - gy) * 64 - 64
                y = origin_y + (gx + gy) * 32
                panel.alpha_composite(tile, (x, y))
                index += 1
    return panel


def make_previews(
    environment: dict[str, Image.Image],
    rat: dict[str, Image.Image],
    hud: dict[str, Image.Image],
) -> None:
    repeat = Image.new("RGBA", (960, 420), (10, 12, 13, 255))
    panels = [
        ("CLEAN 4x4", [environment["floor_clean_128x64.png"]]),
        (
            "CLEAN/WORN 4x4",
            [
                environment["floor_clean_128x64.png"],
                environment["floor_worn_128x64.png"],
            ],
        ),
        ("WATER 4x4", [environment["water_center_128x64.png"]]),
    ]
    draw = ImageDraw.Draw(repeat)
    for index, (title, tiles) in enumerate(panels):
        panel = iso_grid_preview(tiles, 4, 4, (320, 390))
        repeat.alpha_composite(panel, (index * 320, 30))
        label(draw, (index * 320 + 10, 9), title)
    repeat.save(PREVIEWS / "environment_repeat_checker.png", optimize=True)

    room = Image.new("RGBA", (960, 540), (8, 10, 11, 255))
    floor = iso_grid_preview(
        [
            environment["floor_clean_128x64.png"],
            environment["floor_worn_128x64.png"],
            environment["floor_clean_128x64.png"],
        ],
        7,
        6,
        (720, 480),
    )
    room.alpha_composite(floor, (120, 40))
    room.alpha_composite(environment["wall_straight_160x160.png"], (580, 72))
    room.alpha_composite(environment["wall_corner_192x160.png"], (238, 84))
    room.alpha_composite(environment["water_edge_128x96.png"], (690, 340))
    room.alpha_composite(environment["prop_barrel_96x112.png"], (280, 275))
    room.alpha_composite(environment["prop_crate_112x112.png"], (610, 286))
    room.alpha_composite(environment["prop_drain_128x80.png"], (430, 320))
    room.save(PREVIEWS / "environment_room_preview.png", optimize=True)

    frames = [rat[f"rat_side_{name}_256x192.png"] for name in RAT_FRAMES]
    actual = checker((768, 224), 16)
    draw = ImageDraw.Draw(actual)
    for index, (name, frame) in enumerate(zip(RAT_FRAMES, frames)):
        actual.alpha_composite(frame, (index * 256, 24))
        label(draw, (index * 256 + 8, 6), name)
        draw.line(
            (index * 256, 24 + GROUNDLINE_TOP, index * 256 + 255, 24 + GROUNDLINE_TOP),
            fill=(80, 180, 170, 150),
        )
    actual.save(PREVIEWS / "rat_actual_size.png", optimize=True)
    actual.resize((384, 112), Image.Resampling.NEAREST).save(
        PREVIEWS / "rat_50_percent.png", optimize=True
    )
    actual.resize((1536, 448), Image.Resampling.NEAREST).save(
        PREVIEWS / "rat_2x.png", optimize=True
    )

    hud_preview = Image.new("RGBA", (960, 540), (12, 14, 15, 255))
    # subtle dark stone field
    field = checker((960, 540), 24)
    field.putalpha(70)
    hud_preview.alpha_composite(field)
    portrait = hud["hud_rat_portrait_184.png"]
    portrait_frame = hud["hud_portrait_frame_256.png"]
    hud_preview.alpha_composite(portrait, (56, 72))
    hud_preview.alpha_composite(portrait_frame, (20, 32))
    bar_frame = hud["hud_bar_frame_512x80.png"]
    health = hud["hud_health_fill_400x52.png"]
    immune = hud["hud_immune_fill_400x52.png"]
    # Full health, half immune, and an empty state demonstrate that the
    # independent modules assemble without losing the frame's brass bevel.
    hud_preview.alpha_composite(bar_frame, (300, 64))
    hud_preview.alpha_composite(health, (356, 78))
    hud_preview.alpha_composite(bar_frame, (300, 180))
    hud_preview.alpha_composite(immune.crop((0, 0, 200, 52)), (356, 194))
    hud_preview.alpha_composite(bar_frame, (300, 296))
    # 50% practical display comparison
    small_group = hud_preview.crop((0, 0, 900, 390)).resize(
        (450, 195), Image.Resampling.NEAREST
    )
    hud_preview.alpha_composite(small_group, (480, 340))
    draw = ImageDraw.Draw(hud_preview)
    label(draw, (20, 10), "100% source modules")
    label(draw, (480, 320), "nearest 50% display")
    hud_preview.save(PREVIEWS / "hud_states.png", optimize=True)

    comparison = Image.new("RGBA", (1440, 1250), (9, 10, 11, 255))
    env_master = Image.open(QUALITY_ROOT / "environment-quality-master.png").convert("RGBA")
    env_thumb = ImageOps.contain(env_master, (680, 390), Image.Resampling.LANCZOS)
    room_thumb = ImageOps.contain(room, (680, 390), Image.Resampling.NEAREST)
    comparison.alpha_composite(env_thumb, (20, 35))
    comparison.alpha_composite(room_thumb, (740, 35))
    rat_master = Image.open(QUALITY_ROOT / "rat-side-walk-quality-master.png").convert("RGBA")
    rat_thumb = ImageOps.contain(rat_master, (680, 300), Image.Resampling.LANCZOS)
    rat_asset = ImageOps.contain(actual, (680, 300), Image.Resampling.NEAREST)
    comparison.alpha_composite(rat_thumb, (20, 475))
    comparison.alpha_composite(rat_asset, (740, 475))
    draw = ImageDraw.Draw(comparison)
    label(draw, (20, 15), "APPROVED QUALITY MASTER")
    label(draw, (740, 15), "PRODUCTION V1 ASSET ASSEMBLY")
    label(draw, (20, 450), "APPROVED RAT MASTER")
    label(draw, (740, 450), "COMMON-CANVAS RAT ASSETS")
    hud_master = Image.open(QUALITY_ROOT / "hud-quality-master.png").convert("RGBA")
    hud_master_thumb = ImageOps.contain(hud_master, (680, 330), Image.Resampling.LANCZOS)
    hud_asset_thumb = ImageOps.contain(hud_preview, (680, 330), Image.Resampling.NEAREST)
    comparison.alpha_composite(hud_master_thumb, (20, 855))
    comparison.alpha_composite(hud_asset_thumb, (740, 855))
    label(draw, (20, 830), "APPROVED HUD MASTER")
    label(draw, (740, 830), "SEPARATED HUD MODULE STATES")
    comparison.save(PREVIEWS / "master_asset_comparison.png", optimize=True)


def write_hashes() -> None:
    files = sorted(path for path in OUTPUT.rglob("*") if path.is_file())
    payload = {
        path.relative_to(ROOT).as_posix(): hashlib.sha256(path.read_bytes()).hexdigest()
        for path in files
    }
    (ROOT / "artifacts" / "asset-hashes.json").write_text(
        json.dumps(payload, ensure_ascii=False, indent=2) + "\n",
        encoding="utf-8",
    )


def main() -> None:
    ensure_dirs()
    environment = build_environment()
    rat = build_rat()
    hud = build_hud()
    make_previews(environment, rat, hud)
    write_hashes()
    print(
        f"built environment={len(environment)} rat={len(rat)} hud={len(hud)} "
        f"into {OUTPUT}"
    )


if __name__ == "__main__":
    main()
