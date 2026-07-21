#!/usr/bin/env python3
"""Deterministic, dependency-free renderer for the rat 8-direction trial asset.

The same in-code low-poly mesh is exported to OBJ and rendered from eight yaw
rotations with one fixed orthographic camera, one fixed light, one canvas and
one root pivot. The renderer intentionally has no anti-aliasing so the 64 px
cells remain suitable for point-filtered inspection.
"""

from __future__ import annotations

import csv
import hashlib
import json
import math
import struct
import zlib
from dataclasses import dataclass
from pathlib import Path


CELL = 64
PIVOT = (32, 48)
SCALE = 9.0
DIRECTIONS = (
    ("S", (0.0, -1.0)),
    ("SW", (-1.0, -1.0)),
    ("W", (-1.0, 0.0)),
    ("NW", (-1.0, 1.0)),
    ("N", (0.0, 1.0)),
    ("NE", (1.0, 1.0)),
    ("E", (1.0, 0.0)),
    ("SE", (1.0, -1.0)),
)

CAMERA_TO_OBJECT = (6.0, -8.0, 6.0)
KEY_LIGHT = (-0.45, -0.55, 1.0)

PALETTE = {
    "body_dark": (73, 64, 59, 255),
    "body_mid": (112, 96, 86, 255),
    "body_light": (151, 129, 113, 255),
    "pink_dark": (140, 87, 88, 255),
    "pink_light": (190, 128, 124, 255),
    "eye": (27, 29, 31, 255),
    "eye_glint": (222, 207, 180, 255),
}


def add(a, b):
    return tuple(a[i] + b[i] for i in range(3))


def sub(a, b):
    return tuple(a[i] - b[i] for i in range(3))


def mul(a, s):
    return tuple(v * s for v in a)


def dot(a, b):
    return sum(a[i] * b[i] for i in range(3))


def cross(a, b):
    return (
        a[1] * b[2] - a[2] * b[1],
        a[2] * b[0] - a[0] * b[2],
        a[0] * b[1] - a[1] * b[0],
    )


def norm(a):
    length = math.sqrt(max(dot(a, a), 1e-12))
    return tuple(v / length for v in a)


def rotate_z(p, yaw):
    c, s = math.cos(yaw), math.sin(yaw)
    return (p[0] * c - p[1] * s, p[0] * s + p[1] * c, p[2])


@dataclass
class Face:
    indices: tuple[int, int, int]
    material: str


class Mesh:
    def __init__(self):
        self.vertices: list[tuple[float, float, float]] = []
        self.faces: list[Face] = []

    def vertex(self, p):
        self.vertices.append(tuple(float(v) for v in p))
        return len(self.vertices) - 1

    def triangle(self, a, b, c, material):
        self.faces.append(Face((a, b, c), material))


def add_ellipsoid(mesh, center, radii, material, segments=8, rings=5):
    bottom = mesh.vertex((center[0], center[1], center[2] - radii[2]))
    ring_ids = []
    for ring in range(1, rings):
        lat = -math.pi / 2.0 + math.pi * ring / rings
        z = center[2] + radii[2] * math.sin(lat)
        radial = math.cos(lat)
        ids = []
        for seg in range(segments):
            angle = 2.0 * math.pi * seg / segments
            ids.append(mesh.vertex((
                center[0] + radii[0] * radial * math.cos(angle),
                center[1] + radii[1] * radial * math.sin(angle),
                z,
            )))
        ring_ids.append(ids)
    top = mesh.vertex((center[0], center[1], center[2] + radii[2]))

    for seg in range(segments):
        nxt = (seg + 1) % segments
        mesh.triangle(bottom, ring_ids[0][nxt], ring_ids[0][seg], material)
    for ring in range(len(ring_ids) - 1):
        low, high = ring_ids[ring], ring_ids[ring + 1]
        for seg in range(segments):
            nxt = (seg + 1) % segments
            mesh.triangle(low[seg], low[nxt], high[nxt], material)
            mesh.triangle(low[seg], high[nxt], high[seg], material)
    for seg in range(segments):
        nxt = (seg + 1) % segments
        mesh.triangle(ring_ids[-1][seg], ring_ids[-1][nxt], top, material)


def add_tube(mesh, p0, p1, r0, r1, material, segments=6):
    axis = norm(sub(p1, p0))
    helper = (0.0, 0.0, 1.0) if abs(axis[2]) < 0.9 else (0.0, 1.0, 0.0)
    side = norm(cross(axis, helper))
    up = norm(cross(side, axis))
    ring0, ring1 = [], []
    for seg in range(segments):
        angle = 2.0 * math.pi * seg / segments
        radial = add(mul(side, math.cos(angle)), mul(up, math.sin(angle)))
        ring0.append(mesh.vertex(add(p0, mul(radial, r0))))
        ring1.append(mesh.vertex(add(p1, mul(radial, r1))))
    for seg in range(segments):
        nxt = (seg + 1) % segments
        mesh.triangle(ring0[seg], ring0[nxt], ring1[nxt], material)
        mesh.triangle(ring0[seg], ring1[nxt], ring1[seg], material)


def build_rat():
    mesh = Mesh()
    # Source forward is +X. Root/pivot is world origin on the ground plane.
    add_ellipsoid(mesh, (-0.25, 0.00, 0.88), (1.35, 0.72, 0.78), "body")
    add_ellipsoid(mesh, (0.98, 0.00, 0.91), (0.82, 0.54, 0.56), "body")
    add_ellipsoid(mesh, (1.62, 0.00, 0.76), (0.63, 0.34, 0.33), "body")

    # Ears are flattened ellipsoids; feet are deliberately chunky at 64 px.
    add_ellipsoid(mesh, (0.72, 0.43, 1.43), (0.36, 0.17, 0.48), "pink", 8, 4)
    add_ellipsoid(mesh, (0.72, -0.43, 1.43), (0.36, 0.17, 0.48), "pink", 8, 4)
    for x in (-0.70, 0.66):
        for y in (-0.47, 0.47):
            add_ellipsoid(mesh, (x, y, 0.25), (0.38, 0.23, 0.23), "body", 8, 4)
            add_ellipsoid(mesh, (x + 0.18, y, 0.11), (0.30, 0.17, 0.12), "pink", 8, 4)

    add_ellipsoid(mesh, (2.18, 0.00, 0.72), (0.17, 0.16, 0.15), "pink", 8, 4)
    add_ellipsoid(mesh, (1.30, 0.47, 1.04), (0.13, 0.10, 0.13), "eye", 8, 4)
    add_ellipsoid(mesh, (1.30, -0.47, 1.04), (0.13, 0.10, 0.13), "eye", 8, 4)
    add_ellipsoid(mesh, (1.36, 0.54, 1.10), (0.035, 0.025, 0.035), "eye_glint", 6, 3)
    add_ellipsoid(mesh, (1.36, -0.54, 1.10), (0.035, 0.025, 0.035), "eye_glint", 6, 3)

    tail = [
        (-1.43, 0.00, 0.58),
        (-1.88, 0.08, 0.44),
        (-2.28, 0.26, 0.31),
        (-2.58, 0.55, 0.21),
        (-2.72, 0.88, 0.14),
        (-2.68, 1.18, 0.11),
    ]
    for i in range(len(tail) - 1):
        add_tube(mesh, tail[i], tail[i + 1], 0.13 - i * 0.017, 0.115 - i * 0.017, "pink")
    return mesh


CAM_FORWARD = norm(CAMERA_TO_OBJECT)
WORLD_UP = (0.0, 0.0, 1.0)
CAM_RIGHT = norm(cross(CAM_FORWARD, WORLD_UP))
CAM_UP = norm(cross(CAM_RIGHT, CAM_FORWARD))
LIGHT = norm(KEY_LIGHT)


def project(p):
    return (dot(p, CAM_RIGHT), dot(p, CAM_UP), dot(p, CAM_FORWARD))


def yaw_for_screen_direction(target):
    # Invert the projection of a ground-plane vector so its projected heading
    # matches the requested screen direction (x right, y up).
    ax, ay = CAM_RIGHT[0], CAM_RIGHT[1]
    bx, by = CAM_UP[0], CAM_UP[1]
    tx, ty = target
    determinant = ax * by - ay * bx
    wx = (tx * by - ay * ty) / determinant
    wy = (ax * ty - tx * bx) / determinant
    return math.atan2(wy, wx)


def face_color(material, normal):
    if dot(normal, CAM_FORWARD) < 0.0:
        normal = mul(normal, -1.0)
    brightness = 0.55 + 0.45 * max(0.0, dot(normal, LIGHT))
    if material == "body":
        return PALETTE["body_dark"] if brightness < 0.68 else (PALETTE["body_mid"] if brightness < 0.84 else PALETTE["body_light"])
    if material == "pink":
        return PALETTE["pink_dark"] if brightness < 0.78 else PALETTE["pink_light"]
    return PALETTE[material]


def render(mesh, yaw):
    rotated = [rotate_z(v, yaw) for v in mesh.vertices]
    projected = [project(v) for v in rotated]
    screen = [(PIVOT[0] + SCALE * p[0], PIVOT[1] - SCALE * p[1], p[2]) for p in projected]
    pixels = [(0, 0, 0, 0)] * (CELL * CELL)
    depth = [-1e30] * (CELL * CELL)

    for face in mesh.faces:
        ia, ib, ic = face.indices
        a, b, c = rotated[ia], rotated[ib], rotated[ic]
        normal = norm(cross(sub(b, a), sub(c, a)))
        color = face_color(face.material, normal)
        pa, pb, pc = screen[ia], screen[ib], screen[ic]
        min_x = max(0, int(math.floor(min(pa[0], pb[0], pc[0]))))
        max_x = min(CELL - 1, int(math.ceil(max(pa[0], pb[0], pc[0]))))
        min_y = max(0, int(math.floor(min(pa[1], pb[1], pc[1]))))
        max_y = min(CELL - 1, int(math.ceil(max(pa[1], pb[1], pc[1]))))
        denom = (pb[1] - pc[1]) * (pa[0] - pc[0]) + (pc[0] - pb[0]) * (pa[1] - pc[1])
        if abs(denom) < 1e-9:
            continue
        for py in range(min_y, max_y + 1):
            sy = py + 0.5
            for px in range(min_x, max_x + 1):
                sx = px + 0.5
                wa = ((pb[1] - pc[1]) * (sx - pc[0]) + (pc[0] - pb[0]) * (sy - pc[1])) / denom
                wb = ((pc[1] - pa[1]) * (sx - pc[0]) + (pa[0] - pc[0]) * (sy - pc[1])) / denom
                wc = 1.0 - wa - wb
                if wa < -1e-7 or wb < -1e-7 or wc < -1e-7:
                    continue
                z = wa * pa[2] + wb * pb[2] + wc * pc[2]
                index = py * CELL + px
                if z > depth[index]:
                    depth[index] = z
                    pixels[index] = color
    return pixels


def png_bytes(width, height, pixels):
    raw = bytearray()
    for y in range(height):
        raw.append(0)
        for rgba in pixels[y * width:(y + 1) * width]:
            raw.extend(rgba)

    def chunk(kind, data):
        return struct.pack(">I", len(data)) + kind + data + struct.pack(">I", zlib.crc32(kind + data) & 0xFFFFFFFF)

    return (
        b"\x89PNG\r\n\x1a\n"
        + chunk(b"IHDR", struct.pack(">IIBBBBB", width, height, 8, 6, 0, 0, 0))
        + chunk(b"IDAT", zlib.compress(bytes(raw), 9))
        + chunk(b"IEND", b"")
    )


def save_png(path, width, height, pixels):
    path.write_bytes(png_bytes(width, height, pixels))


def save_obj(mesh, path):
    lines = ["# The Last Host rat trial v1", "mtllib rat-trial-v1.mtl", "o rat_trial_v1"]
    lines.extend(f"v {x:.6f} {y:.6f} {z:.6f}" for x, y, z in mesh.vertices)
    active = None
    for face in mesh.faces:
        if face.material != active:
            active = face.material
            lines.append(f"usemtl {active}")
        a, b, c = (index + 1 for index in face.indices)
        lines.append(f"f {a} {b} {c}")
    path.write_text("\n".join(lines) + "\n", encoding="utf-8")


def save_mtl(path):
    def rgb(color):
        return " ".join(f"{v / 255.0:.6f}" for v in color[:3])
    lines = ["# Flat preview materials; renderer uses the documented quantized palette."]
    for name, palette_key in (("body", "body_mid"), ("pink", "pink_light"), ("eye", "eye"), ("eye_glint", "eye_glint")):
        lines.extend((f"newmtl {name}", f"Kd {rgb(PALETTE[palette_key])}", "Ka 0 0 0", "Ks 0 0 0", "d 1.0", ""))
    path.write_text("\n".join(lines), encoding="utf-8")


def sha256(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def main():
    source_dir = Path(__file__).resolve().parent
    artifact_dir = source_dir.parent
    render_dir = artifact_dir / "renders"
    render_dir.mkdir(parents=True, exist_ok=True)
    mesh = build_rat()
    save_obj(mesh, source_dir / "rat-trial-v1.obj")
    save_mtl(source_dir / "rat-trial-v1.mtl")

    records = []
    cells = []
    for index, (label, target) in enumerate(DIRECTIONS):
        yaw = yaw_for_screen_direction(target)
        pixels = render(mesh, yaw)
        filename = f"rat-{index:02d}-{label.lower()}.png"
        path = render_dir / filename
        save_png(path, CELL, CELL, pixels)
        records.append({
            "index": index,
            "direction": label,
            "screen_heading": [target[0], target[1]],
            "source_yaw_degrees": round(math.degrees(yaw), 6),
            "file": filename,
            "sha256": sha256(path),
        })
        cells.append(pixels)

    sheet = []
    for y in range(CELL):
        for cell in cells:
            sheet.extend(cell[y * CELL:(y + 1) * CELL])
    sheet_path = render_dir / "rat-8dir-sheet.png"
    save_png(sheet_path, CELL * len(cells), CELL, sheet)

    with (artifact_dir / "direction-map.csv").open("w", newline="", encoding="utf-8-sig") as handle:
        writer = csv.DictWriter(handle, fieldnames=("index", "direction", "screen_dx", "screen_dy", "source_yaw_degrees", "file", "sha256"))
        writer.writeheader()
        for record in records:
            writer.writerow({
                "index": record["index"],
                "direction": record["direction"],
                "screen_dx": record["screen_heading"][0],
                "screen_dy": record["screen_heading"][1],
                "source_yaw_degrees": record["source_yaw_degrees"],
                "file": record["file"],
                "sha256": record["sha256"],
            })

    settings = {
        "asset_id": "rat-host-directional-trial-v1",
        "status": "trial-placeholder-not-final-art",
        "source": {
            "definition": "source/render_rat_8dir.py::build_rat",
            "export": "source/rat-trial-v1.obj",
            "vertex_count": len(mesh.vertices),
            "triangle_count": len(mesh.faces),
            "source_forward": "+X",
            "root_pivot": [0, 0, 0],
        },
        "render": {
            "algorithm": "dependency-free deterministic orthographic triangle z-buffer",
            "cell_px": [CELL, CELL],
            "sheet_px": [CELL * len(cells), CELL],
            "transparent_background": True,
            "anti_aliasing": False,
            "filter_preview": "nearest-neighbor / Point",
            "camera_to_object": CAMERA_TO_OBJECT,
            "projection": "orthographic",
            "scale_px_per_unit": SCALE,
            "pivot_px_top_left": PIVOT,
            "key_light": KEY_LIGHT,
            "palette_rgba": PALETTE,
            "direction_order": [record["direction"] for record in records],
        },
        "files": records,
        "sheet": {"file": "rat-8dir-sheet.png", "sha256": sha256(sheet_path)},
        "reproduce": "python source/render_rat_8dir.py",
    }
    (artifact_dir / "render-settings.json").write_text(json.dumps(settings, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps({"vertices": len(mesh.vertices), "triangles": len(mesh.faces), "sheet_sha256": settings["sheet"]["sha256"]}, ensure_ascii=False))


if __name__ == "__main__":
    main()
