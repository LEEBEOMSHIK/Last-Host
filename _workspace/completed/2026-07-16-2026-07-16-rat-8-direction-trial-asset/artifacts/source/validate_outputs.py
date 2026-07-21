#!/usr/bin/env python3
"""Validate the trial asset without third-party packages."""

from __future__ import annotations

import hashlib
import json
import struct
import zlib
from pathlib import Path


def decode_png(path: Path):
    data = path.read_bytes()
    assert data[:8] == b"\x89PNG\r\n\x1a\n", f"PNG signature: {path}"
    offset = 8
    idat = bytearray()
    width = height = None
    while offset < len(data):
        length = struct.unpack(">I", data[offset:offset + 4])[0]
        kind = data[offset + 4:offset + 8]
        payload = data[offset + 8:offset + 8 + length]
        crc = struct.unpack(">I", data[offset + 8 + length:offset + 12 + length])[0]
        assert (zlib.crc32(kind + payload) & 0xFFFFFFFF) == crc, f"CRC: {path}"
        offset += 12 + length
        if kind == b"IHDR":
            width, height, depth, color, compression, filtering, interlace = struct.unpack(">IIBBBBB", payload)
            assert (depth, color, compression, filtering, interlace) == (8, 6, 0, 0, 0), f"RGBA8: {path}"
        elif kind == b"IDAT":
            idat.extend(payload)
        elif kind == b"IEND":
            break
    raw = zlib.decompress(bytes(idat))
    stride = width * 4
    pixels = []
    previous = bytearray(stride)
    cursor = 0
    for _ in range(height):
        filter_type = raw[cursor]
        cursor += 1
        row = bytearray(raw[cursor:cursor + stride])
        cursor += stride
        assert filter_type == 0, f"Only deterministic filter 0 expected: {path}"
        pixels.extend(tuple(row[i:i + 4]) for i in range(0, stride, 4))
        previous = row
    return width, height, pixels


def digest(path: Path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def main():
    source_dir = Path(__file__).resolve().parent
    artifact_dir = source_dir.parent
    settings = json.loads((artifact_dir / "render-settings.json").read_text(encoding="utf-8"))
    expected_order = ["S", "SW", "W", "NW", "N", "NE", "E", "SE"]
    assert settings["render"]["direction_order"] == expected_order
    allowed = {tuple(value) for value in settings["render"]["palette_rgba"].values()}
    cells = []
    bounds = {}
    for record in settings["files"]:
        path = artifact_dir / "renders" / record["file"]
        assert digest(path) == record["sha256"]
        width, height, pixels = decode_png(path)
        assert (width, height) == (64, 64)
        assert all(pixels[y * width + x][3] == 0 for x, y in ((0, 0), (63, 0), (0, 63), (63, 63)))
        opaque = [(i % width, i // width, pixel) for i, pixel in enumerate(pixels) if pixel[3]]
        assert opaque, f"Empty sprite: {path}"
        assert {pixel for _, _, pixel in opaque}.issubset(allowed), f"Palette drift: {path}"
        bounds[record["direction"]] = [
            min(x for x, _, _ in opaque), min(y for _, y, _ in opaque),
            max(x for x, _, _ in opaque), max(y for _, y, _ in opaque),
        ]
        cells.append(pixels)

    sheet_path = artifact_dir / "renders" / settings["sheet"]["file"]
    assert digest(sheet_path) == settings["sheet"]["sha256"]
    width, height, sheet = decode_png(sheet_path)
    assert (width, height) == (512, 64)
    expected_sheet = []
    for y in range(64):
        for cell in cells:
            expected_sheet.extend(cell[y * 64:(y + 1) * 64])
    assert sheet == expected_sheet, "Sheet is not an exact S..SE concatenation"

    obj_lines = (source_dir / "rat-trial-v1.obj").read_text(encoding="utf-8").splitlines()
    vertices = sum(line.startswith("v ") for line in obj_lines)
    triangles = sum(line.startswith("f ") for line in obj_lines)
    assert vertices == settings["source"]["vertex_count"] == 528
    assert triangles == settings["source"]["triangle_count"] == 924
    print(json.dumps({
        "status": "pass",
        "directions": expected_order,
        "cell": [64, 64],
        "sheet": [512, 64],
        "opaque_palette_colors_max": len(allowed),
        "bounds": bounds,
        "sheet_sha256": digest(sheet_path),
        "source": {"vertices": vertices, "triangles": triangles},
    }, ensure_ascii=False))


if __name__ == "__main__":
    main()
