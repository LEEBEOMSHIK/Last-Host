"""Build and render the near-final brown sewer-rat appearance sample.

This script is intended to run inside Blender.  It creates a new low-poly
source file and writes only beneath this task's artifacts directory.  The
existing v1-v5b sources and Unity project are read-only references and are not
opened or modified.
"""
import bpy
import csv
import json
import math
import os
import struct
import zlib
from mathutils import Vector

ROOT = r"C:\project\Last-Host"
TASK = os.path.join(ROOT, "_workspace", "active", "2026-07-24-rat-final-appearance-sample")
ARTIFACTS = os.path.join(TASK, "artifacts")
SOURCE_DIR = os.path.join(ARTIFACTS, "source")
IDLE_DIR = os.path.join(ARTIFACTS, "renders", "idle")
WALK_DIR = os.path.join(ARTIFACTS, "renders", "walk-key")
RAW_DIR = os.path.join(ARTIFACTS, "raw-renders")
BLEND_PATH = os.path.join(SOURCE_DIR, "rat-final-appearance-sample-v1.blend")
FRAME_MAP = os.path.join(ARTIFACTS, "frame-map.csv")
SETTINGS_PATH = os.path.join(ARTIFACTS, "render-settings.json")
PALETTE_STATS = os.path.join(ARTIFACTS, "palette-statistics.json")
CONTACT_SHEET = os.path.join(ARTIFACTS, "rat-final-appearance-contact-sheet-2048.png")
TURNAROUND = os.path.join(ARTIFACTS, "rat-final-appearance-turnaround-preview-2048.png")

for directory in (SOURCE_DIR, IDLE_DIR, WALK_DIR, RAW_DIR):
    os.makedirs(directory, exist_ok=True)

CELL = 128
PIVOT = (64, 96)
WALK_KEY = 4
TARGET = Vector((-0.143, 0.208, 0.70))
DIRECTIONS = (
    (0, "S", -53.130102),
    (1, "SW", -25.904474),
    (2, "W", 36.869898),
    (3, "NW", 99.644270),
    (4, "N", 126.869898),
    (5, "NE", 154.095526),
    (6, "E", -143.130102),
    (7, "SE", -80.355730),
)

# Shared v5b-compatible palette: transparent plus 27 opaque entries.
PALETTE = (
    (0, 0, 0, 0),
    (24, 21, 20, 255), (37, 29, 25, 255), (50, 37, 31, 255),
    (63, 46, 37, 255), (76, 55, 43, 255), (90, 66, 50, 255),
    (105, 78, 59, 255), (121, 91, 69, 255), (139, 108, 84, 255),
    (158, 127, 101, 255), (176, 148, 121, 255), (196, 171, 143, 255),
    (74, 62, 52, 255), (94, 79, 65, 255), (118, 101, 82, 255),
    (145, 126, 103, 255), (171, 153, 128, 255), (199, 183, 156, 255),
    (86, 53, 51, 255), (111, 69, 65, 255), (137, 88, 82, 255),
    (164, 110, 103, 255), (188, 137, 127, 255), (210, 167, 153, 255),
    (220, 204, 177, 255), (235, 223, 197, 255), (247, 237, 213, 255),
)

MATERIAL_COLORS = {
    "coat_dark": (73, 52, 41),
    "coat_warm": (112, 82, 61),
    "coat_light": (154, 121, 92),
    "belly": (169, 151, 124),
    "skin_dark": (112, 69, 65),
    "skin": (164, 110, 103),
    "skin_light": (196, 145, 132),
    "eye": (37, 29, 25),
    "glint": (235, 223, 197),
    "nose": (50, 37, 31),
}


def srgb_to_linear(value):
    value /= 255.0
    return value / 12.92 if value <= 0.04045 else ((value + 0.055) / 1.055) ** 2.4


def linear_to_srgb(value):
    value = max(0.0, min(1.0, value))
    return 12.92 * value if value <= 0.0031308 else 1.055 * (value ** (1.0 / 2.4)) - 0.055


def make_material(name, rgb):
    material = bpy.data.materials.new("RatSample_" + name)
    color = tuple(srgb_to_linear(channel) for channel in rgb) + (1.0,)
    material.diffuse_color = color
    material.use_nodes = True
    bsdf = material.node_tree.nodes.get("Principled BSDF")
    bsdf.inputs["Base Color"].default_value = color
    bsdf.inputs["Roughness"].default_value = 0.92
    if "Specular IOR Level" in bsdf.inputs:
        bsdf.inputs["Specular IOR Level"].default_value = 0.18
    return material


def parent_to_root(obj, root):
    obj.parent = root
    obj.matrix_parent_inverse.identity()
    return obj


def add_ico(name, location, scale, material, root, subdivisions=1):
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=subdivisions, radius=1.0, location=location)
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    obj.data.materials.append(material)
    for polygon in obj.data.polygons:
        polygon.use_smooth = False
    return parent_to_root(obj, root)


def add_uv(name, location, scale, material, root, segments=12, rings=6):
    bpy.ops.mesh.primitive_uv_sphere_add(
        segments=segments, ring_count=rings, radius=1.0, location=location
    )
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    obj.data.materials.append(material)
    for polygon in obj.data.polygons:
        polygon.use_smooth = False
    return parent_to_root(obj, root)


def add_snout(name, location, material, root):
    bpy.ops.mesh.primitive_cone_add(
        vertices=8,
        radius1=0.39,
        radius2=0.16,
        depth=0.82,
        location=location,
        rotation=(0.0, math.radians(90.0), 0.0),
    )
    obj = bpy.context.object
    obj.name = name
    obj.scale = (1.0, 0.82, 0.90)
    obj.data.materials.append(material)
    for polygon in obj.data.polygons:
        polygon.use_smooth = False
    return parent_to_root(obj, root)


def add_tail(root, material):
    curve = bpy.data.curves.new("RatSample_TailCurve", type="CURVE")
    curve.dimensions = "3D"
    curve.resolution_u = 1
    curve.bevel_depth = 0.075
    curve.bevel_resolution = 0
    curve.resolution_u = 1
    spline = curve.splines.new("POLY")
    points = (
        (0.00, 0.00, 0.00),
        (-0.42, 0.00, -0.08),
        (-0.87, 0.03, -0.13),
        (-1.33, 0.10, -0.17),
        (-1.76, 0.20, -0.15),
    )
    spline.points.add(len(points) - 1)
    for point, co in zip(spline.points, points):
        point.co = (*co, 1.0)
    obj = bpy.data.objects.new("Tail", curve)
    bpy.context.scene.collection.objects.link(obj)
    obj.location = (-1.43, 0.0, 0.55)
    curve.materials.append(material)
    return parent_to_root(obj, root)


def build_model():
    scene = bpy.context.scene
    for obj in list(scene.objects):
        bpy.data.objects.remove(obj, do_unlink=True)
    for datablock in list(bpy.data.materials):
        bpy.data.materials.remove(datablock)

    mats = {name: make_material(name, rgb) for name, rgb in MATERIAL_COLORS.items()}
    root = bpy.data.objects.new("RatFinalSampleRoot", None)
    scene.collection.objects.link(root)

    body = add_ico("Body", (-0.30, 0.0, 0.78), (1.38, 0.68, 0.62), mats["coat_warm"], root, 2)
    add_ico("BackCoat", (-0.50, 0.0, 0.93), (1.15, 0.61, 0.50), mats["coat_dark"], root, 1)
    belly = add_ico("Belly", (-0.17, 0.0, 0.51), (1.02, 0.58, 0.31), mats["belly"], root, 2)
    neck = add_ico("Neck", (0.63, 0.0, 0.82), (0.58, 0.56, 0.53), mats["coat_light"], root, 1)
    head = add_ico("Head", (0.93, 0.0, 0.89), (0.70, 0.56, 0.55), mats["coat_light"], root, 2)
    snout = add_snout("Snout", (1.38, 0.0, 0.78), mats["belly"], root)
    nose = add_ico("Nose", (1.80, 0.0, 0.78), (0.17, 0.15, 0.14), mats["nose"], root, 1)

    for side, y in (("L", 0.43), ("R", -0.43)):
        add_uv("EarOuter_" + side, (0.72, y, 1.27), (0.32, 0.12, 0.40), mats["skin_dark"], root, 10, 5)
        add_uv("EarInner_" + side, (0.74, y * 1.055, 1.28), (0.23, 0.055, 0.29), mats["skin_light"], root, 10, 5)
        eye = add_ico("Eye_" + side, (1.17, y * 0.83, 1.00), (0.095, 0.070, 0.095), mats["eye"], root, 1)
        glint = add_ico("EyeGlint_" + side, (1.22, y * 0.87, 1.055), (0.028, 0.020, 0.028), mats["glint"], root, 1)

    paw_specs = {
        "Paw_FL": ((0.64, 0.48, 0.13), (0.35, 0.20, 0.13)),
        "Paw_FR": ((0.64, -0.48, 0.13), (0.35, 0.20, 0.13)),
        "Paw_RL": ((-0.77, 0.50, 0.14), (0.42, 0.23, 0.15)),
        "Paw_RR": ((-0.77, -0.50, 0.14), (0.42, 0.23, 0.15)),
    }
    paws = {}
    for name, (location, scale) in paw_specs.items():
        paws[name] = add_uv(name, location, scale, mats["skin"], root, 8, 4)

    tail = add_tail(root, mats["skin"])

    # Store authored design intent directly in the Blender source.
    root["sample_status"] = "near-final-candidate-not-user-approved"
    root["appearance_direction"] = "brown sewer rat; cute 60 / uncanny 40"
    root["source_forward"] = "+X"
    root["unity_ppu"] = 64
    root["unity_custom_pivot"] = "(0.5, 0.25)"
    root["forbidden"] = "gore; rabbit-hop gait; anthropomorphic hands; high-saturation pink"
    return root, body, belly, neck, head, snout, nose, paws, tail


def animate_model(body, belly, neck, head, snout, nose, paws, tail):
    bases = {obj.name: obj.location.copy() for obj in (body, belly, neck, head, snout, nose, tail, *paws.values())}
    pair_sign = {"Paw_FL": 1.0, "Paw_RR": 1.0, "Paw_FR": -1.0, "Paw_RL": -1.0}
    for frame in range(1, 9):
        cycle = (frame - 1) * math.pi / 4.0
        for name, paw in paws.items():
            gait = math.cos(cycle) * pair_sign[name]
            paw.location = bases[name]
            paw.location.x += 0.24 * gait
            # Only the advancing diagonal pair lifts; contact feet keep the ground.
            paw.location.z += 0.13 * max(0.0, -gait)
            paw.keyframe_insert(data_path="location", frame=frame)
        low_pulse = 0.018 * (math.sin(cycle) ** 2)
        for obj, scale in ((body, 1.0), (belly, 0.9), (neck, 0.75)):
            obj.location = bases[obj.name]
            obj.location.z += low_pulse * scale
            obj.keyframe_insert(data_path="location", frame=frame)
        for obj in (head, snout, nose):
            obj.location = bases[obj.name]
            obj.location.x += 0.025 * math.sin(cycle)
            obj.location.z += low_pulse * 0.55
            obj.keyframe_insert(data_path="location", frame=frame)
        tail.location = bases[tail.name]
        tail.rotation_euler = (0.0, 0.0, math.radians(-4.5 * math.sin(cycle)))
        tail.keyframe_insert(data_path="location", frame=frame)
        tail.keyframe_insert(data_path="rotation_euler", frame=frame)

    # Blender 4.4+ stores animation curves behind Action Slots.  The exact
    # authored keys are the source of truth for this two-pose sample, so no
    # legacy Action.fcurves traversal is required here.


def configure_scene():
    scene = bpy.context.scene
    scene.frame_start, scene.frame_end = 1, 8
    scene.render.fps = 8
    scene.render.engine = "BLENDER_EEVEE"
    scene.render.resolution_x = CELL
    scene.render.resolution_y = CELL
    scene.render.resolution_percentage = 100
    scene.render.image_settings.file_format = "PNG"
    scene.render.image_settings.color_mode = "RGBA"
    scene.render.film_transparent = True
    scene.render.filter_size = 0.01
    scene.render.image_settings.color_depth = "8"
    scene.render.image_settings.compression = 100
    scene.render.filepath = os.path.join(RAW_DIR, "_preview.png")
    scene.render.use_file_extension = True

    scene.view_settings.look = "AgX - Medium High Contrast"
    scene.world.color = (0.020, 0.016, 0.014)

    camera_data = bpy.data.cameras.new("RatSampleCamera")
    camera = bpy.data.objects.new("RatSampleCamera", camera_data)
    scene.collection.objects.link(camera)
    camera.location = TARGET + Vector((6.0, -8.0, 6.0))
    camera.rotation_euler = (TARGET - camera.location).to_track_quat("-Z", "Y").to_euler()
    camera_data.type = "ORTHO"
    camera_data.ortho_scale = 6.80
    camera.data.lens = 50
    scene.camera = camera

    sun_data = bpy.data.lights.new("RatSampleKeyLight", type="SUN")
    sun_data.energy = 2.6
    sun_data.angle = math.radians(6.0)
    sun = bpy.data.objects.new("RatSampleKeyLight", sun_data)
    scene.collection.objects.link(sun)
    sun.location = (-4.5, -5.5, 9.0)
    sun.rotation_euler = (Vector((0.0, 0.0, 0.4)) - sun.location).to_track_quat("-Z", "Y").to_euler()

    fill_data = bpy.data.lights.new("RatSampleFillLight", type="AREA")
    fill_data.energy = 180.0
    fill_data.shape = "DISK"
    fill_data.size = 5.0
    fill = bpy.data.objects.new("RatSampleFillLight", fill_data)
    scene.collection.objects.link(fill)
    fill.location = (1.5, 3.0, 4.5)
    fill.rotation_euler = (TARGET - fill.location).to_track_quat("-Z", "Y").to_euler()
    return scene, camera, sun, fill


def nearest_palette(r, g, b):
    best = PALETTE[1]
    best_distance = 1 << 30
    for color in PALETTE[1:]:
        distance = (
            3 * (r - color[0]) ** 2
            + 4 * (g - color[1]) ** 2
            + 2 * (b - color[2]) ** 2
        )
        if distance < best_distance:
            best_distance = distance
            best = color
    return best


def png_bytes(width, height, pixels):
    raw = bytearray()
    for y in range(height):
        raw.append(0)
        for pixel in pixels[y * width:(y + 1) * width]:
            raw.extend(pixel)

    def chunk(kind, data):
        return (
            struct.pack(">I", len(data))
            + kind
            + data
            + struct.pack(">I", zlib.crc32(kind + data) & 0xFFFFFFFF)
        )

    return (
        b"\x89PNG\r\n\x1a\n"
        + chunk(b"IHDR", struct.pack(">IIBBBBB", width, height, 8, 6, 0, 0, 0))
        + chunk(b"IDAT", zlib.compress(bytes(raw), 9))
        + chunk(b"IEND", b"")
    )


def save_png(path, width, height, pixels):
    with open(path, "wb") as handle:
        handle.write(png_bytes(width, height, pixels))


def render_quantized(scene, path):
    raw_path = os.path.join(RAW_DIR, os.path.basename(path))
    scene.render.filepath = raw_path
    bpy.ops.render.render(write_still=True)
    result = bpy.data.images.load(raw_path, check_existing=False)
    width, height = result.size
    floats = list(result.pixels)
    output = []
    # Blender image pixels are bottom-up; PNG rows are written top-down.
    for y in range(height - 1, -1, -1):
        for x in range(width):
            offset = (y * width + x) * 4
            alpha = floats[offset + 3]
            if alpha < 0.50:
                output.append(PALETTE[0])
                continue
            rgb = tuple(
                int(round(linear_to_srgb(floats[offset + channel]) * 255.0))
                for channel in range(3)
            )
            output.append(nearest_palette(*rgb))
    bpy.data.images.remove(result)
    os.remove(raw_path)
    save_png(path, width, height, output)
    return output


def compose_nearest(frames, cols, rows, scale, background=(0, 0, 0, 0), pad_y=0):
    width = cols * CELL * scale
    height = rows * CELL * scale + pad_y * 2
    pixels = [background] * (width * height)
    for slot, source in enumerate(frames):
        col = slot % cols
        row = slot // cols
        ox = col * CELL * scale
        oy = pad_y + row * CELL * scale
        for sy in range(CELL):
            for sx in range(CELL):
                color = source[sy * CELL + sx]
                for dy in range(scale):
                    start = (oy + sy * scale + dy) * width + ox + sx * scale
                    pixels[start:start + scale] = [color] * scale
    return width, height, pixels


root, body, belly, neck, head, snout, nose, paws, tail = build_model()
animate_model(body, belly, neck, head, snout, nose, paws, tail)
scene, camera, sun, fill = configure_scene()

records = []
all_frames = {}
unique_by_file = {}
alpha_values = set()
bounds = {}
for pose, frame, output_dir in (("idle", 1, IDLE_DIR), ("walk-key", WALK_KEY, WALK_DIR)):
    scene.frame_set(frame)
    for index, label, yaw in DIRECTIONS:
        root.rotation_euler = (0.0, 0.0, math.radians(yaw))
        if pose == "idle":
            filename = "rat-final-sample-idle-%02d-%s.png" % (index, label.lower())
        else:
            filename = "rat-final-sample-walk-f%02d-%02d-%s.png" % (frame, index, label.lower())
        path = os.path.join(output_dir, filename)
        pixels = render_quantized(scene, path)
        all_frames[(pose, index)] = pixels
        colors = sorted(set(pixels))
        unique_by_file[os.path.relpath(path, ARTIFACTS).replace("\\", "/")] = len(colors) - (1 if PALETTE[0] in colors else 0)
        alpha_values.update(pixel[3] for pixel in pixels)
        opaque = [i for i, pixel in enumerate(pixels) if pixel[3] == 255]
        bounds[os.path.relpath(path, ARTIFACTS).replace("\\", "/")] = {
            "min_x": min(i % CELL for i in opaque),
            "max_x": max(i % CELL for i in opaque),
            "min_y": min(i // CELL for i in opaque),
            "max_y": max(i // CELL for i in opaque),
        }
        records.append({
            "pose": pose,
            "frame": frame,
            "direction_index": index,
            "direction": label,
            "source_yaw_degrees": "%.6f" % yaw,
            "file": os.path.relpath(path, ARTIFACTS).replace("\\", "/"),
            "phase": "neutral_idle" if pose == "idle" else "advancing_diagonal_pair_lift",
        })

root.rotation_euler = (0.0, 0.0, 0.0)
scene.frame_set(1)
bpy.ops.wm.save_as_mainfile(filepath=BLEND_PATH)
backup_path = BLEND_PATH + "1"
if os.path.isfile(backup_path):
    os.remove(backup_path)

with open(FRAME_MAP, "w", newline="", encoding="utf-8") as handle:
    writer = csv.DictWriter(
        handle,
        fieldnames=("pose", "frame", "direction_index", "direction", "source_yaw_degrees", "file", "phase"),
    )
    writer.writeheader()
    writer.writerows(records)

contact_frames = [all_frames[("idle", i)] for i in range(8)] + [all_frames[("walk-key", i)] for i in range(8)]
cw, ch, cp = compose_nearest(contact_frames, cols=8, rows=2, scale=2)
save_png(CONTACT_SHEET, cw, ch, cp)

turn_frames = [all_frames[("idle", i)] for i in (0, 2, 4, 6)]
tw, th, tp = compose_nearest(
    turn_frames,
    cols=4,
    rows=1,
    scale=4,
    background=(24, 21, 20, 255),
    pad_y=64,
)
save_png(TURNAROUND, tw, th, tp)

settings = {
    "asset_id": "rat-final-appearance-sample-v1",
    "status": "near-final-appearance-candidate-not-user-approved",
    "blender_version": bpy.app.version_string,
    "created_from": "new low-poly model; completed v5b and Blender work were read-only visual/technical references",
    "appearance": {
        "species_direction": "warm brown urban sewer rat",
        "tone_ratio": {"cute": 60, "uncanny": 40},
        "silhouette": "low center of gravity; separated head; two large ears; four distinct paws; near-body-length thin tail",
        "face": "compact pointed muzzle; small dark reflective eyes; no oversized cartoon eyes",
        "gait": "quick diagonal-pair scurry; restrained body bob and counter-swing tail; no rabbit hopping",
        "virus_influence": "non-gore: reflective eye, angular planes, low posture",
    },
    "source": {
        "blend": os.path.relpath(BLEND_PATH, ARTIFACTS).replace("\\", "/"),
        "reproduction_script": "source/create_rat_final_appearance_sample.py",
        "source_forward": "+X",
        "root_pivot": [0.0, 0.0, 0.0],
        "root_transform_keyframes": 0,
    },
    "timeline": {
        "frames": [1, 8],
        "fps": 8,
        "idle_sample_frame": 1,
        "walk_sample_frame": WALK_KEY,
        "loop_contract": "frame 8 advances to frame 1",
    },
    "camera": {
        "name": camera.name,
        "projection": camera.data.type,
        "location": list(camera.location),
        "target": list(TARGET),
        "orthographic_scale": camera.data.ortho_scale,
        "direction_yaws_degrees": [{"index": i, "direction": d, "yaw": y} for i, d, y in DIRECTIONS],
    },
    "lighting": {
        "key": {"name": sun.name, "type": sun.data.type, "energy": sun.data.energy},
        "fill": {"name": fill.name, "type": fill.data.type, "energy": fill.data.energy},
        "animated": False,
    },
    "canvas": {
        "width_px": CELL,
        "height_px": CELL,
        "rgba": True,
        "transparent_background": True,
        "binary_alpha": True,
        "dithering": "none",
        "shared_palette_limit": 32,
        "shared_palette_defined_opaque_colors": len(PALETTE) - 1,
        "contact_sheet": {"width_px": cw, "height_px": ch, "scale": "2x nearest"},
        "turnaround_preview": {"width_px": tw, "height_px": th, "scale": "4x nearest", "views": ["S", "W", "N", "E"]},
    },
    "unity_handoff": {
        "pixels_per_unit": 64,
        "custom_pivot_normalized": [0.5, 0.25],
        "pivot_px_top_left": list(PIVOT),
        "world_width_units": 2.0,
        "integration_status": "not approved; do not import yet",
    },
    "verification": {
        "sample_png_count": len(records),
        "idle_png_count": 8,
        "walk_key_png_count": 8,
        "all_sample_dimensions": [CELL, CELL],
        "alpha_values": sorted(alpha_values),
        "bounds": bounds,
        "minimum_bbox_margin_px": min(
            min(
                item["min_x"],
                CELL - 1 - item["max_x"],
                item["min_y"],
                CELL - 1 - item["max_y"],
            )
            for item in bounds.values()
        ),
        "per_file_bbox_margin_px": {
            name: min(
                item["min_x"],
                CELL - 1 - item["max_x"],
                item["min_y"],
                CELL - 1 - item["max_y"],
            )
            for name, item in bounds.items()
        },
        "all_outputs_within_canvas": all(
            4 <= item["min_x"] <= item["max_x"] <= CELL - 5
            and 4 <= item["min_y"] <= item["max_y"] <= CELL - 5
            for item in bounds.values()
        ),
        "edge_contact_files": [
            name for name, item in bounds.items()
            if item["min_x"] == 0 or item["max_x"] == CELL - 1
            or item["min_y"] == 0 or item["max_y"] == CELL - 1
        ],
        "shared_ground": "paw soles authored at common Z; only advancing diagonal pair lifts",
        "model_objects": sorted(obj.name for obj in bpy.context.scene.objects if obj.type in {"MESH", "CURVE"}),
    },
}
with open(SETTINGS_PATH, "w", encoding="utf-8") as handle:
    json.dump(settings, handle, ensure_ascii=False, indent=2)
    handle.write("\n")

used_opaque = sorted({
    pixel for frame_pixels in all_frames.values() for pixel in frame_pixels if pixel[3] == 255
})
palette_stats = {
    "palette_contract": "one shared palette across all 16 sample PNGs",
    "defined_opaque_colors": len(PALETTE) - 1,
    "used_opaque_colors": len(used_opaque),
    "used_rgba": [list(color) for color in used_opaque],
    "alpha_values": sorted(alpha_values),
    "binary_alpha": sorted(alpha_values) == [0, 255],
    "dithering": False,
    "per_file_opaque_color_count": unique_by_file,
    "maximum_per_file_opaque_colors": max(unique_by_file.values()),
}
with open(PALETTE_STATS, "w", encoding="utf-8") as handle:
    json.dump(palette_stats, handle, ensure_ascii=False, indent=2)
    handle.write("\n")

if os.path.isdir(RAW_DIR) and not os.listdir(RAW_DIR):
    os.rmdir(RAW_DIR)

assert len(records) == 16
assert sorted(alpha_values) == [0, 255]
assert len(used_opaque) <= 32
assert cw == 2048 and ch == 512
assert tw == 2048
assert settings["verification"]["all_outputs_within_canvas"]
assert settings["verification"]["minimum_bbox_margin_px"] >= 4
print(json.dumps({
    "blend": BLEND_PATH,
    "sample_png_count": len(records),
    "idle_png_count": 8,
    "walk_key_png_count": 8,
    "cell": [CELL, CELL],
    "shared_palette_used_opaque": len(used_opaque),
    "alpha_values": sorted(alpha_values),
    "contact_sheet": [cw, ch],
    "turnaround": [tw, th],
}, ensure_ascii=False))
