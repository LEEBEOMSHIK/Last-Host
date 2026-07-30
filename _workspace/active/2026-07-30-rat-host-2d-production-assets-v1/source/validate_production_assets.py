"""Validate production-v1 2D assets without importing them into Unity."""

from __future__ import annotations

import hashlib
import json
import subprocess
import sys
from pathlib import Path

import cv2
import numpy as np
from PIL import Image


ROOT = Path(__file__).resolve().parents[1]
GAME = ROOT / "artifacts" / "game-assets"
PREVIEWS = ROOT / "artifacts" / "previews"
REPORT = ROOT / "artifacts" / "validation-report.json"
BUILD = ROOT / "source" / "build_production_assets.py"


class Checks:
    def __init__(self) -> None:
        self.rows: list[dict[str, object]] = []

    def check(self, name: str, passed: bool, detail: str) -> None:
        self.rows.append({"name": name, "passed": bool(passed), "detail": detail})


def sha(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def png_info(path: Path) -> tuple[Image.Image, np.ndarray]:
    image = Image.open(path).convert("RGBA")
    return image, np.array(image)


def chroma_residual(array: np.ndarray) -> tuple[int, int]:
    red = array[:, :, 0]
    green = array[:, :, 1]
    blue = array[:, :, 2]
    alpha = array[:, :, 3]
    visible = alpha > 16
    magenta = visible & (red > 180) & (blue > 180) & (green < 80)
    green_key = visible & (green > 180) & (red < 80) & (blue < 100)
    return int(magenta.sum()), int(green_key.sum())


def alpha_bbox(array: np.ndarray) -> tuple[int, int, int, int]:
    ys, xs = np.where(array[:, :, 3] > 24)
    if not len(xs):
        return 0, 0, 0, 0
    return int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1


def verify_repeat(tile: Image.Image) -> tuple[bool, str]:
    width, height = tile.size
    canvas = Image.new("RGBA", (width * 5, height * 5), (0, 0, 0, 0))
    origin_x = canvas.width // 2
    origin_y = 8
    for total in range(7):
        for gy in range(4):
            gx = total - gy
            if 0 <= gx < 4:
                x = origin_x + (gx - gy) * (width // 2) - width // 2
                y = origin_y + (gx + gy) * (height // 2)
                canvas.alpha_composite(tile, (x, y))
    mask = (np.array(canvas.getchannel("A")) > 24).astype(np.uint8) * 255
    contours, hierarchy = cv2.findContours(mask, cv2.RETR_CCOMP, cv2.CHAIN_APPROX_SIMPLE)
    holes = 0
    if hierarchy is not None:
        holes = sum(1 for row in hierarchy[0] if row[3] >= 0)
    components, _labels = cv2.connectedComponents((mask > 0).astype(np.uint8), 8)
    passed = holes == 0 and components == 2
    return passed, f"visible_components={components - 1}, holes={holes}"


def all_output_files() -> list[Path]:
    return sorted(path for path in GAME.rglob("*") if path.is_file())


def main() -> int:
    checks = Checks()
    expected_sizes = {
        "environment/floor_clean_128x64.png": (128, 64),
        "environment/floor_worn_128x64.png": (128, 64),
        "environment/water_center_128x64.png": (128, 64),
        "environment/wall_straight_160x160.png": (160, 160),
        "environment/wall_corner_192x160.png": (192, 160),
        "environment/water_edge_128x96.png": (128, 96),
        "environment/prop_barrel_96x112.png": (96, 112),
        "environment/prop_crate_112x112.png": (112, 112),
        "environment/prop_drain_128x80.png": (128, 80),
        "rat/rat_side_neutral_256x192.png": (256, 192),
        "rat/rat_side_contact_256x192.png": (256, 192),
        "rat/rat_side_passing_256x192.png": (256, 192),
        "rat/rat_side_walk_3f_sheet.png": (768, 192),
        "hud/hud_portrait_frame_256.png": (256, 256),
        "hud/hud_rat_portrait_184.png": (184, 184),
        "hud/hud_bar_frame_512x80.png": (512, 80),
        "hud/hud_health_fill_400x52.png": (400, 52),
        "hud/hud_immune_fill_400x52.png": (400, 52),
    }
    for relative, size in expected_sizes.items():
        path = GAME / relative
        checks.check(f"exists:{relative}", path.exists(), str(path))
        if not path.exists():
            continue
        image, array = png_info(path)
        checks.check(f"rgba:{relative}", image.mode == "RGBA", f"mode={image.mode}")
        checks.check(f"size:{relative}", image.size == size, f"size={image.size}")
        corners = (
            array[0, 0, 3],
            array[0, -1, 3],
            array[-1, 0, 3],
            array[-1, -1, 3],
        )
        checks.check(
            f"transparent-corners:{relative}",
            all(int(value) == 0 for value in corners),
            f"alpha={tuple(int(value) for value in corners)}",
        )
        visible = int((array[:, :, 3] > 24).sum())
        checks.check(f"visible:{relative}", visible > 20, f"visible_pixels={visible}")
        magenta, green = chroma_residual(array)
        checks.check(
            f"no-key-color:{relative}",
            magenta == 0 and green == 0,
            f"magenta={magenta}, green={green}",
        )

    for name in (
        "floor_clean_128x64.png",
        "floor_worn_128x64.png",
        "water_center_128x64.png",
    ):
        image = Image.open(GAME / "environment" / name).convert("RGBA")
        passed, detail = verify_repeat(image)
        checks.check(f"repeat:{name}", passed, detail)

    rat_boxes: list[tuple[int, int, int, int]] = []
    for name in ("neutral", "contact", "passing"):
        _image, array = png_info(GAME / "rat" / f"rat_side_{name}_256x192.png")
        box = alpha_bbox(array)
        rat_boxes.append(box)
        checks.check(
            f"rat-groundline:{name}",
            box[3] == 152,
            f"alpha_bbox={box}, expected_bottom_top_y=152",
        )
    widths = [box[2] - box[0] for box in rat_boxes]
    heights = [box[3] - box[1] for box in rat_boxes]
    checks.check(
        "rat-width-consistency",
        max(widths) / min(widths) <= 1.05,
        f"widths={widths}",
    )
    checks.check(
        "rat-height-consistency",
        max(heights) / min(heights) <= 1.05,
        f"heights={heights}",
    )

    frame_map_path = GAME / "rat" / "rat_side_walk_3f_frame-map.json"
    frame_map = json.loads(frame_map_path.read_text(encoding="utf-8"))
    checks.check(
        "rat-pivot",
        frame_map["pivot"]["pixels"] == [128, 40]
        and frame_map["groundline_top_y"] == 152,
        json.dumps(frame_map["pivot"], ensure_ascii=False),
    )
    checks.check(
        "rat-frame-order",
        [item["name"] for item in frame_map["frames"]]
        == ["neutral", "contact", "passing"],
        str([item["name"] for item in frame_map["frames"]]),
    )

    layout = json.loads(
        (GAME / "hud" / "hud_module-layout.json").read_text(encoding="utf-8")
    )
    checks.check(
        "hud-fill-layout",
        layout["bar"]["fill_offset_top_left"] == [56, 14]
        and layout["bar"]["fill_size"] == [400, 52],
        json.dumps(layout["bar"], ensure_ascii=False),
    )

    expected_previews = (
        "environment_repeat_checker.png",
        "environment_room_preview.png",
        "rat_actual_size.png",
        "rat_50_percent.png",
        "rat_2x.png",
        "hud_states.png",
        "master_asset_comparison.png",
    )
    for name in expected_previews:
        path = PREVIEWS / name
        checks.check(f"preview:{name}", path.exists() and path.stat().st_size > 1000, str(path))

    before = {path.relative_to(ROOT).as_posix(): sha(path) for path in all_output_files()}
    run = subprocess.run(
        [sys.executable, str(BUILD)],
        cwd=ROOT.parent.parent.parent,
        capture_output=True,
        text=True,
        check=False,
    )
    after = {path.relative_to(ROOT).as_posix(): sha(path) for path in all_output_files()}
    checks.check("rebuild-exit", run.returncode == 0, run.stdout.strip() or run.stderr.strip())
    checks.check(
        "rebuild-reproducible",
        before == after,
        f"files={len(before)}, matching={sum(before.get(key) == value for key, value in after.items())}",
    )

    passed = sum(1 for row in checks.rows if row["passed"])
    failed = len(checks.rows) - passed
    payload = {
        "summary": {"passed": passed, "failed": failed, "total": len(checks.rows)},
        "unity_import": "not run; explicitly out of scope",
        "checks": checks.rows,
    }
    REPORT.write_text(
        json.dumps(payload, ensure_ascii=False, indent=2) + "\n", encoding="utf-8"
    )
    print(json.dumps(payload["summary"], ensure_ascii=False))
    if failed:
        for row in checks.rows:
            if not row["passed"]:
                print(f"FAIL {row['name']}: {row['detail']}")
    return 1 if failed else 0


if __name__ == "__main__":
    raise SystemExit(main())
