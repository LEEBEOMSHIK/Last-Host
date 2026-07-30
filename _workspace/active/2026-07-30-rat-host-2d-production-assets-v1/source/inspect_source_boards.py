"""Inspect imagegen chroma source boards before production extraction.

This script is diagnostic only.  It never modifies source masters.
"""

from __future__ import annotations

import json
import sys
from collections import Counter
from pathlib import Path

from PIL import Image


def border_pixels(image: Image.Image) -> list[tuple[int, int, int]]:
    rgb = image.convert("RGB")
    width, height = rgb.size
    values: list[tuple[int, int, int]] = []
    for x in range(width):
        values.append(rgb.getpixel((x, 0)))
        values.append(rgb.getpixel((x, height - 1)))
    for y in range(1, height - 1):
        values.append(rgb.getpixel((0, y)))
        values.append(rgb.getpixel((width - 1, y)))
    return values


def describe(path: Path) -> dict[str, object]:
    with Image.open(path) as image:
        colors = Counter(border_pixels(image))
        common = colors.most_common(5)
        total = sum(colors.values())
        return {
            "file": path.name,
            "size": list(image.size),
            "mode": image.mode,
            "border_top_colors": [
                {
                    "rgb": list(color),
                    "count": count,
                    "fraction": round(count / total, 6),
                }
                for color, count in common
            ],
        }


def main() -> int:
    if len(sys.argv) != 2:
        print("usage: inspect_source_boards.py <source-masters-dir>")
        return 2
    root = Path(sys.argv[1])
    files = sorted(root.glob("*.png"))
    payload = {
        "root": str(root),
        "file_count": len(files),
        "files": [describe(path) for path in files],
    }
    print(json.dumps(payload, ensure_ascii=False, indent=2))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
