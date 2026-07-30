"""Apply and prove the isolated HUD portrait fragment correction."""

from __future__ import annotations

import hashlib
import json
import subprocess
import sys
from pathlib import Path

import cv2
import numpy as np
from PIL import Image, ImageDraw, ImageFont


TASK = Path(__file__).resolve().parents[1]
PROJECT = TASK.parents[2]
PRODUCTION = (
    PROJECT
    / "_workspace"
    / "active"
    / "2026-07-30-rat-host-2d-production-assets-v1"
)
GAME = PRODUCTION / "artifacts" / "game-assets"
PORTRAIT = GAME / "hud" / "hud_rat_portrait_184.png"
BUILD = PRODUCTION / "source" / "build_production_assets.py"
ARTIFACTS = TASK / "artifacts"


def digest(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def file_hashes() -> dict[str, str]:
    return {
        path.relative_to(GAME).as_posix(): digest(path)
        for path in sorted(GAME.rglob("*"))
        if path.is_file()
    }


def checker(size: tuple[int, int], cell: int = 16) -> Image.Image:
    image = Image.new("RGBA", size, (30, 31, 34, 255))
    draw = ImageDraw.Draw(image)
    for y in range(0, size[1], cell):
        for x in range(0, size[0], cell):
            if (x // cell + y // cell) % 2:
                draw.rectangle((x, y, x + cell - 1, y + cell - 1), fill=(49, 50, 54, 255))
    return image


def component_stats(image: Image.Image) -> list[dict[str, int]]:
    array = np.array(image.convert("RGBA"))
    mask = (array[:, :, 3] > 24).astype(np.uint8)
    count, _labels, stats, _centroids = cv2.connectedComponentsWithStats(mask, 8)
    rows = []
    for index in range(1, count):
        rows.append(
            {
                "x": int(stats[index, cv2.CC_STAT_LEFT]),
                "y": int(stats[index, cv2.CC_STAT_TOP]),
                "width": int(stats[index, cv2.CC_STAT_WIDTH]),
                "height": int(stats[index, cv2.CC_STAT_HEIGHT]),
                "area": int(stats[index, cv2.CC_STAT_AREA]),
            }
        )
    return sorted(rows, key=lambda row: row["area"], reverse=True)


def make_preview(before: Image.Image, after: Image.Image, changed: np.ndarray) -> None:
    scale = 2
    panel_size = (368, 368)
    preview = Image.new("RGBA", (1200, 430), (12, 13, 15, 255))
    draw = ImageDraw.Draw(preview)
    font = ImageFont.load_default()
    labels = ("BEFORE", "AFTER", "REMOVED PIXELS")
    for index, label in enumerate(labels):
        x = 20 + index * 395
        draw.text((x, 10), label, fill=(226, 207, 160, 255), font=font)
        preview.alpha_composite(checker(panel_size), (x, 38))

    before_2x = before.resize(panel_size, Image.Resampling.NEAREST)
    after_2x = after.resize(panel_size, Image.Resampling.NEAREST)
    preview.alpha_composite(before_2x, (20, 38))
    preview.alpha_composite(after_2x, (415, 38))

    diff = np.zeros((184, 184, 4), dtype=np.uint8)
    diff[changed] = (255, 70, 210, 255)
    diff_image = Image.fromarray(diff, "RGBA").resize(panel_size, Image.Resampling.NEAREST)
    preview.alpha_composite(diff_image, (810, 38))
    draw.text(
        (20, 412),
        "Only the detached top brass component is removed; rat pixels remain byte-identical.",
        fill=(216, 219, 210, 255),
        font=font,
    )
    preview.save(ARTIFACTS / "hud-before-after.png", optimize=True)


def main() -> int:
    ARTIFACTS.mkdir(parents=True, exist_ok=True)
    before_hashes = file_hashes()
    before = Image.open(PORTRAIT).convert("RGBA")
    before_array = np.array(before)
    before_components = component_stats(before)

    run = subprocess.run(
        [sys.executable, str(BUILD)],
        cwd=PROJECT,
        capture_output=True,
        text=True,
        check=False,
    )
    if run.returncode != 0:
        print(run.stdout)
        print(run.stderr, file=sys.stderr)
        return run.returncode

    after_hashes = file_hashes()
    after = Image.open(PORTRAIT).convert("RGBA")
    after_array = np.array(after)
    after_components = component_stats(after)
    changed = np.any(before_array != after_array, axis=2)

    target = "hud/hud_rat_portrait_184.png"
    other_keys = sorted(set(before_hashes) - {target})
    unchanged_other = [
        key for key in other_keys if before_hashes[key] == after_hashes.get(key)
    ]
    changed_rows = np.where(changed)[0]
    max_changed_y = int(changed_rows.max()) if len(changed_rows) else -1
    rat_region_identical = bool(np.array_equal(before_array[27:], after_array[27:]))
    after_top_clear = bool(np.all(after_array[:27] == 0))
    only_target_changed = (
        len(unchanged_other) == len(other_keys)
        and before_hashes[target] != after_hashes[target]
    )

    make_preview(before, after, changed)
    after.save(ARTIFACTS / "hud_rat_portrait_184-corrected.png", optimize=True)

    report = {
        "build_exit": run.returncode,
        "build_stdout": run.stdout.strip(),
        "target": target,
        "before_sha256": before_hashes[target],
        "after_sha256": after_hashes[target],
        "game_asset_file_count": len(after_hashes),
        "other_files_unchanged": f"{len(unchanged_other)}/{len(other_keys)}",
        "only_target_changed": only_target_changed,
        "changed_pixel_count": int(changed.sum()),
        "max_changed_top_y": max_changed_y,
        "rows_y27_and_below_identical": rat_region_identical,
        "top_rows_0_to_26_transparent": after_top_clear,
        "before_components": before_components,
        "after_components": after_components,
        "checks": {
            "source_build_reproducible": run.returncode == 0,
            "other_19_files_sha_unchanged": len(unchanged_other) == 19,
            "portrait_sha_changed": before_hashes[target] != after_hashes[target],
            "only_top_artifact_pixels_changed": max_changed_y <= 26,
            "rat_pixels_byte_identical": rat_region_identical,
            "after_has_single_visible_component": len(after_components) == 1,
        },
    }
    report["passed"] = all(report["checks"].values())
    (ARTIFACTS / "hud-correction-verification.json").write_text(
        json.dumps(report, ensure_ascii=False, indent=2) + "\n",
        encoding="utf-8",
    )
    print(json.dumps({"passed": report["passed"], **report["checks"]}, ensure_ascii=False))
    return 0 if report["passed"] else 1


if __name__ == "__main__":
    raise SystemExit(main())
