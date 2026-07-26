"""Independent A2 revision-6 final-candidate Blender renderer.

Run preview first:
  A2_R6_MODE=preview blender --background --python this_file.py
Then run full after visual acceptance:
  A2_R6_MODE=full blender --background --python this_file.py
"""
import bpy
import csv
import hashlib
import json
import math
import os
import struct
import zlib
from mathutils import Vector

ROOT = r"C:\project\Last-Host"
TASK = os.path.join(ROOT, "_workspace", "active", "2026-07-24-rat-final-appearance-sample")
ARTIFACTS = os.path.join(TASK, "artifacts", "a2-blender-revision-6")
SOURCE_DIR = os.path.join(ARTIFACTS, "source")
PREVIEW_DIR = os.path.join(ARTIFACTS, "previews")
IDLE_DIR = os.path.join(ARTIFACTS, "renders", "idle")
WALK_DIR = os.path.join(ARTIFACTS, "renders", "walk-key")
RAW_DIR = os.path.join(ARTIFACTS, "raw-renders")
MODE = os.environ.get("A2_R6_MODE", "preview").lower()
if MODE not in {"preview", "full"}:
    raise RuntimeError("A2_R6_MODE must be preview or full")
for directory in (SOURCE_DIR, PREVIEW_DIR, IDLE_DIR, WALK_DIR, RAW_DIR):
    os.makedirs(directory, exist_ok=True)

CELL = 128
SOURCE_CELL = 512
WALK_KEY = 4
TARGET = Vector((-0.10, 0.10, 0.78))
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
COLORS = {
    "coat_dark": (58, 42, 34),
    "coat_warm": (94, 68, 51),
    "coat_light": (132, 101, 76),
    "belly": (151, 132, 107),
    "skin_dark": (101, 63, 60),
    "skin": (148, 94, 89),
    "skin_light": (188, 135, 124),
    "eye": (30, 24, 22),
    "glint": (235, 223, 197),
    "nose": (43, 31, 28),
}


def srgb_to_linear(v):
    v /= 255.0
    return v / 12.92 if v <= 0.04045 else ((v + 0.055) / 1.055) ** 2.4


def linear_to_srgb(v):
    v = max(0.0, min(1.0, v))
    return 12.92 * v if v <= 0.0031308 else 1.055 * (v ** (1.0 / 2.4)) - 0.055


def material(name, rgb):
    mat = bpy.data.materials.new("RatA2R6_" + name)
    color = tuple(srgb_to_linear(c) for c in rgb) + (1.0,)
    mat.diffuse_color = color
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    bsdf.inputs["Base Color"].default_value = color
    bsdf.inputs["Roughness"].default_value = 0.94
    if "Specular IOR Level" in bsdf.inputs:
        bsdf.inputs["Specular IOR Level"].default_value = 0.14
    return mat


def parent(obj, root):
    obj.parent = root
    obj.matrix_parent_inverse.identity()
    return obj


def add_uv(name, loc, scale, mat, root, segments=24, rings=12):
    bpy.ops.mesh.primitive_uv_sphere_add(
        segments=segments, ring_count=rings, radius=1.0, location=loc
    )
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    obj.data.materials.append(mat)
    for polygon in obj.data.polygons:
        polygon.use_smooth = True
    return parent(obj, root)


def add_ico(name, loc, scale, mat, root, subdivisions=2):
    bpy.ops.mesh.primitive_ico_sphere_add(
        subdivisions=subdivisions, radius=1.0, location=loc
    )
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    obj.data.materials.append(mat)
    for polygon in obj.data.polygons:
        polygon.use_smooth = subdivisions >= 2
    return parent(obj, root)


def add_body(root, mats):
    bpy.ops.mesh.primitive_uv_sphere_add(
        segments=36, ring_count=18, radius=1.0, location=(-0.34, 0.0, 0.86)
    )
    obj = bpy.context.object
    obj.name = "Body"
    obj.scale = (1.54, 0.72, 0.80)
    for key in ("coat_dark", "coat_warm", "coat_light", "belly"):
        obj.data.materials.append(mats[key])
    for vertex in obj.data.vertices:
        x = vertex.co.x
        hind = 1.0 + 0.20 * math.exp(-((x + 0.43) / 0.43) ** 2)
        front = 1.0 - 0.16 * max(0.0, x)
        vertex.co.y *= hind * front
        vertex.co.z *= (1.0 + 0.16 * math.exp(-((x + 0.48) / 0.52) ** 2)) * front
        vertex.co.z += 0.06 * (1.0 - min(1.0, x * x))
    for polygon in obj.data.polygons:
        center = sum((obj.data.vertices[i].co for i in polygon.vertices), Vector()) / len(polygon.vertices)
        # One continuous coat: dark dorsal cap, warm flank, small beige
        # underside.  No oval side panel or shell-like cross stripe.
        polygon.material_index = 0 if center.z > 0.48 else 3 if center.z < -0.48 else 1
        polygon.use_smooth = True
    return parent(obj, root)


def add_head(root, mats):
    bpy.ops.mesh.primitive_uv_sphere_add(
        segments=32, ring_count=16, radius=1.0, location=(1.10, 0.0, 0.98)
    )
    obj = bpy.context.object
    obj.name = "Head"
    obj.scale = (0.78, 0.56, 0.68)
    for key in ("coat_warm", "coat_light", "belly"):
        obj.data.materials.append(mats[key])
    for vertex in obj.data.vertices:
        taper = 1.0 - 0.27 * max(0.0, vertex.co.x)
        vertex.co.y *= taper
        vertex.co.z *= taper
        vertex.co.z += 0.04 * max(0.0, -vertex.co.x)
    for polygon in obj.data.polygons:
        center = sum((obj.data.vertices[i].co for i in polygon.vertices), Vector()) / len(polygon.vertices)
        polygon.material_index = 2 if center.z < -0.38 else 0
        polygon.use_smooth = True
    return parent(obj, root)


def add_snout(root, mat):
    bpy.ops.mesh.primitive_cone_add(
        vertices=20,
        radius1=0.36,
        radius2=0.095,
        depth=0.98,
        location=(1.68, 0.0, 0.83),
        rotation=(0.0, math.radians(90.0), 0.0),
    )
    obj = bpy.context.object
    obj.name = "Snout"
    obj.scale = (1.0, 0.88, 0.82)
    obj.data.materials.append(mat)
    for polygon in obj.data.polygons:
        polygon.use_smooth = True
    return parent(obj, root)


def add_tail(root, mat):
    curve = bpy.data.curves.new("RatA2R6_TailCurve", "CURVE")
    curve.dimensions = "3D"
    curve.resolution_u = 1
    curve.bevel_depth = 0.060
    curve.bevel_resolution = 1
    spline = curve.splines.new("POLY")
    points = (
        (0.00, 0.00, 0.00), (-0.30, 0.08, -0.06), (-0.52, 0.30, -0.10),
        (-0.66, 0.65, -0.11), (-0.70, 1.00, -0.09),
        (-0.64, 1.40, -0.04), (-0.50, 1.75, 0.02),
    )
    spline.points.add(len(points) - 1)
    for p, co in zip(spline.points, points):
        p.co = (*co, 1.0)
    obj = bpy.data.objects.new("Tail", curve)
    bpy.context.scene.collection.objects.link(obj)
    obj.location = (-1.43, 0.0, 0.58)
    curve.materials.append(mat)
    return parent(obj, root)


def add_whiskers(root, mat):
    for side_name, sign in (("L", 1.0), ("R", -1.0)):
        for index, z_offset in enumerate((-0.045, 0.055), 1):
            curve = bpy.data.curves.new("RatA2R6_WhiskerCurve_%s_%02d" % (side_name, index), "CURVE")
            curve.dimensions = "3D"
            curve.resolution_u = 1
            curve.bevel_depth = 0.026
            curve.bevel_resolution = 0
            spline = curve.splines.new("POLY")
            points = (
                (1.69, sign * 0.26, 0.87 + z_offset),
                (1.98, sign * 0.48, 0.87 + z_offset * 0.5),
                (2.20, sign * 0.72, 0.86 + z_offset * 0.25),
            )
            spline.points.add(len(points) - 1)
            for point, co in zip(spline.points, points):
                point.co = (*co, 1.0)
            obj = bpy.data.objects.new("Whisker_%s_%02d" % (side_name, index), curve)
            bpy.context.scene.collection.objects.link(obj)
            curve.materials.append(mat)
            parent(obj, root)


def build_model():
    for obj in list(bpy.context.scene.objects):
        bpy.data.objects.remove(obj, do_unlink=True)
    for datablock in list(bpy.data.materials):
        bpy.data.materials.remove(datablock)
    mats = {name: material(name, rgb) for name, rgb in COLORS.items()}
    root = bpy.data.objects.new("RatFinalAppearanceA2R6Root", None)
    bpy.context.scene.collection.objects.link(root)
    body = add_body(root, mats)
    neck = add_uv("Neck", (0.70, 0.0, 0.91), (0.69, 0.56, 0.64), mats["coat_warm"], root)
    head = add_head(root, mats)
    chin = add_uv("Chin", (1.29, 0.0, 0.72), (0.48, 0.43, 0.25), mats["belly"], root, 20, 10)
    snout = add_snout(root, mats["coat_light"])
    nose = add_ico("Nose", (2.18, 0.0, 0.83), (0.22, 0.19, 0.18), mats["nose"], root)
    add_ico("NoseGlint", (2.300, -0.052, 0.905), (0.050, 0.035, 0.035), mats["glint"], root, 1)
    for side, y in (("L", 0.62), ("R", -0.62)):
        outer = add_uv("EarOuter_" + side, (0.80, y, 1.53), (0.45, 0.11, 0.58), mats["skin_dark"], root, 28, 14)
        inner = add_uv("EarInner_" + side, (0.82, y * 1.025, 1.54), (0.32, 0.048, 0.43), mats["skin_light"], root, 28, 14)
        turn = math.radians(37)
        outer.rotation_euler.z = turn
        inner.rotation_euler.z = turn
        add_uv("Cheek_" + side, (1.34, y * 0.77, 0.88), (0.28, 0.042, 0.21), mats["coat_light"], root, 20, 10)
        add_ico("Eye_" + side, (1.49, y * 0.68, 1.14), (0.205, 0.145, 0.190), mats["eye"], root)
        add_ico("EyeGlint_" + side, (1.595, y * 0.70, 1.235), (0.055, 0.038, 0.052), mats["glint"], root, 1)
    add_whiskers(root, mats["belly"])
    paw_specs = {
        "Paw_FL": ((0.78, 0.50, 0.09), (0.40, 0.20, 0.08)),
        "Paw_FR": ((0.78, -0.50, 0.09), (0.40, 0.20, 0.08)),
        "Paw_RL": ((-0.92, 0.53, 0.10), (0.50, 0.24, 0.09)),
        "Paw_RR": ((-0.92, -0.53, 0.10), (0.50, 0.24, 0.09)),
    }
    paws = {}
    for name, (loc, scale) in paw_specs.items():
        paw = add_uv(name, loc, scale, mats["skin"], root, 20, 10)
        paws[name] = paw
        leg_loc = (loc[0] - 0.04, loc[1] * 0.94, 0.36)
        leg_scale = (0.23 if "F" in name[-2:] else 0.30, 0.16, 0.33)
        leg = add_uv("Leg_" + name[-2:], leg_loc, leg_scale, mats["coat_light"] if "F" in name[-2:] else mats["coat_warm"], root)
        leg.parent = paw
        leg.matrix_parent_inverse = paw.matrix_world.inverted()
        sign = 1 if loc[1] > 0 else -1
        for toe_index, yoff in enumerate((-0.065, 0.065), 1):
            toe = add_uv(
                "Toe_%s_%02d" % (name[-2:], toe_index),
                (loc[0] + 0.29, loc[1] + sign * yoff, 0.05),
                (0.18, 0.045, 0.034),
                mats["skin_light"], root, 12, 6,
            )
            toe.parent = paw
            toe.matrix_parent_inverse = paw.matrix_world.inverted()
    tail = add_tail(root, mats["skin"])
    root["sample_status"] = "a2-r6-final-candidate-not-user-approved"
    root["appearance_direction"] = "A2 natural warm-brown sewer rat; cute 60 / uncanny 40"
    root["source_forward"] = "+X"
    return root, body, neck, head, chin, snout, nose, paws, tail


def animate(body, neck, head, chin, snout, nose, paws, tail):
    objects = (body, neck, head, chin, snout, nose, tail, *paws.values())
    bases = {obj.name: obj.location.copy() for obj in objects}
    signs = {"Paw_FL": 1, "Paw_RR": 1, "Paw_FR": -1, "Paw_RL": -1}
    # Frame 0 is an explicit neutral idle: no stride offset, no lifted paw,
    # no body pulse and no tail counter-sway.
    for name, paw in paws.items():
        paw.location = bases[name]
        paw.keyframe_insert("location", frame=0)
    for obj in (body, neck, head, chin, snout, nose):
        obj.location = bases[obj.name]
        obj.keyframe_insert("location", frame=0)
    tail.location = bases[tail.name]
    tail.rotation_euler = (0, 0, 0)
    tail.keyframe_insert("location", frame=0)
    tail.keyframe_insert("rotation_euler", frame=0)

    for frame in range(1, 9):
        cycle = (frame - 1) * math.pi / 4
        for name, paw in paws.items():
            gait = math.cos(cycle) * signs[name]
            paw.location = bases[name]
            paw.location.x += 0.19 * gait
            paw.location.z += 0.085 * max(0.0, -gait)
            paw.keyframe_insert("location", frame=frame)
        pulse = 0.010 * math.sin(cycle) ** 2
        for obj, factor in ((body, 1.0), (neck, 0.75), (head, 0.55), (chin, 0.55), (snout, 0.55), (nose, 0.55)):
            obj.location = bases[obj.name]
            obj.location.z += pulse * factor
            if obj in (head, chin, snout, nose):
                obj.location.x += 0.020 * math.sin(cycle)
            obj.keyframe_insert("location", frame=frame)
        tail.location = bases[tail.name]
        tail.rotation_euler = (0, 0, math.radians(-3.0 * math.sin(cycle)))
        tail.keyframe_insert("location", frame=frame)
        tail.keyframe_insert("rotation_euler", frame=frame)


def configure():
    scene = bpy.context.scene
    scene.frame_start, scene.frame_end, scene.render.fps = 0, 8, 8
    scene.render.engine = "BLENDER_EEVEE"
    scene.render.resolution_x = SOURCE_CELL
    scene.render.resolution_y = SOURCE_CELL
    scene.render.resolution_percentage = 100
    scene.render.image_settings.file_format = "PNG"
    scene.render.image_settings.color_mode = "RGBA"
    scene.render.image_settings.color_depth = "8"
    scene.render.film_transparent = True
    scene.render.filter_size = 0.01
    scene.view_settings.look = "AgX - Medium High Contrast"
    scene.world.color = (0.016, 0.013, 0.012)
    cam_data = bpy.data.cameras.new("RatA2R6Camera")
    cam = bpy.data.objects.new("RatA2R6Camera", cam_data)
    scene.collection.objects.link(cam)
    cam.location = TARGET + Vector((6, -8, 6))
    cam.rotation_euler = (TARGET - cam.location).to_track_quat("-Z", "Y").to_euler()
    cam_data.type = "ORTHO"
    cam_data.ortho_scale = 5.30
    scene.camera = cam
    sun_data = bpy.data.lights.new("RatA2R6Key", "SUN")
    sun_data.energy, sun_data.angle = 2.4, math.radians(6)
    sun = bpy.data.objects.new("RatA2R6Key", sun_data)
    scene.collection.objects.link(sun)
    sun.location = (-4.5, -5.5, 9)
    sun.rotation_euler = (Vector((0, 0, 0.5)) - sun.location).to_track_quat("-Z", "Y").to_euler()
    fill_data = bpy.data.lights.new("RatA2R6Fill", "AREA")
    fill_data.energy, fill_data.shape, fill_data.size = 100, "DISK", 5
    fill = bpy.data.objects.new("RatA2R6Fill", fill_data)
    scene.collection.objects.link(fill)
    fill.location = (1.5, 3, 4.5)
    fill.rotation_euler = (TARGET - fill.location).to_track_quat("-Z", "Y").to_euler()
    return scene, cam


def nearest(r, g, b):
    return min(PALETTE[1:], key=lambda c: 3*(r-c[0])**2 + 4*(g-c[1])**2 + 2*(b-c[2])**2)


def png_bytes(width, height, pixels):
    raw = bytearray()
    for y in range(height):
        raw.append(0)
        for pixel in pixels[y*width:(y+1)*width]:
            raw.extend(pixel)
    def chunk(kind, data):
        return struct.pack(">I", len(data)) + kind + data + struct.pack(">I", zlib.crc32(kind + data) & 0xffffffff)
    return b"\x89PNG\r\n\x1a\n" + chunk(b"IHDR", struct.pack(">IIBBBBB", width, height, 8, 6, 0, 0, 0)) + chunk(b"IDAT", zlib.compress(bytes(raw), 9)) + chunk(b"IEND", b"")


def save_png(path, width, height, pixels):
    with open(path, "wb") as handle:
        handle.write(png_bytes(width, height, pixels))


def render(scene, path):
    raw_path = os.path.join(RAW_DIR, os.path.basename(path))
    scene.render.filepath = raw_path
    bpy.ops.render.render(write_still=True)
    image = bpy.data.images.load(raw_path, check_existing=False)
    width, height = image.size
    if (width, height) != (SOURCE_CELL, SOURCE_CELL):
        raise RuntimeError("Expected 512x512 source render")
    data = list(image.pixels)
    output = []
    factor = SOURCE_CELL // CELL
    for oy in range(CELL):
        sy = height - 1 - (oy * factor + factor // 2)
        for ox in range(CELL):
            sx = ox * factor + factor // 2
            offset = (sy * width + sx) * 4
            if data[offset + 3] < 0.5:
                output.append(PALETTE[0])
            else:
                rgb = tuple(int(round(linear_to_srgb(data[offset+i]) * 255)) for i in range(3))
                output.append(nearest(*rgb))
    bpy.data.images.remove(image)
    os.remove(raw_path)
    save_png(path, CELL, CELL, output)
    return output


def compose(frames, cols, rows, scale, background=(0, 0, 0, 0), pad_y=0):
    width, height = cols*CELL*scale, rows*CELL*scale + pad_y*2
    pixels = [background] * (width*height)
    for slot, src in enumerate(frames):
        ox, oy = (slot % cols)*CELL*scale, pad_y + (slot // cols)*CELL*scale
        for sy in range(CELL):
            for sx in range(CELL):
                color = src[sy*CELL+sx]
                for dy in range(scale):
                    start = (oy+sy*scale+dy)*width + ox+sx*scale
                    pixels[start:start+scale] = [color]*scale
    return width, height, pixels


root, body, neck, head, chin, snout, nose, paws, tail = build_model()
animate(body, neck, head, chin, snout, nose, paws, tail)
scene, camera = configure()

if MODE == "preview":
    preview_specs = ((2, "side", DIRECTIONS[2][2]),)
    scene.frame_set(0)
    generated = {}
    for index, label, yaw in preview_specs:
        root.rotation_euler = (0, 0, math.radians(yaw))
        path = os.path.join(PREVIEW_DIR, "rat-a2-r6-preview-%s.png" % label)
        generated[label] = render(scene, path)
    root.rotation_euler = (0, 0, 0)
    bpy.ops.wm.save_as_mainfile(filepath=os.path.join(SOURCE_DIR, "rat-final-appearance-a2-r6-preview.blend"))
    for path in (os.path.join(SOURCE_DIR, "rat-final-appearance-a2-r6-preview.blend1"),):
        if os.path.isfile(path):
            os.remove(path)
    print(json.dumps({"mode": MODE, "previews": sorted(os.listdir(PREVIEW_DIR))}, ensure_ascii=False))
else:
    records, frames, colors_by_file, alpha_values, bounds = [], {}, {}, set(), {}
    for pose, frame, folder in (("idle", 0, IDLE_DIR), ("walk-key", WALK_KEY, WALK_DIR)):
        scene.frame_set(frame)
        for index, label, yaw in DIRECTIONS:
            root.rotation_euler = (0, 0, math.radians(yaw))
            filename = (
                "rat-a2-r6-idle-%02d-%s.png" % (index, label.lower())
                if pose == "idle" else
                "rat-a2-r6-walk-key-f04-%02d-%s.png" % (index, label.lower())
            )
            path = os.path.join(folder, filename)
            pixels = render(scene, path)
            frames[(pose, index)] = pixels
            rel = os.path.relpath(path, ARTIFACTS).replace("\\", "/")
            opaque = [i for i, p in enumerate(pixels) if p[3] == 255]
            used = set(pixels)
            colors_by_file[rel] = len(used - {PALETTE[0]})
            alpha_values.update(p[3] for p in pixels)
            bounds[rel] = {
                "min_x": min(i % CELL for i in opaque), "max_x": max(i % CELL for i in opaque),
                "min_y": min(i // CELL for i in opaque), "max_y": max(i // CELL for i in opaque),
            }
            records.append({
                "pose": pose, "frame": frame, "direction_index": index,
                "direction": label, "source_yaw_degrees": "%.6f" % yaw,
                "file": rel,
                "phase": "neutral_four_paw_ground_contact" if pose == "idle" else "diagonal_FL_RR_lift",
            })
    root.rotation_euler = (0, 0, 0)
    scene.frame_set(0)
    blend_path = os.path.join(SOURCE_DIR, "rat-final-appearance-a2-r6.blend")
    bpy.ops.wm.save_as_mainfile(filepath=blend_path)
    if os.path.isfile(blend_path + "1"):
        os.remove(blend_path + "1")
    with open(os.path.join(ARTIFACTS, "frame-map.csv"), "w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=records[0].keys())
        writer.writeheader()
        writer.writerows(records)
    contact = [frames[("idle", i)] for i in range(8)] + [frames[("walk-key", i)] for i in range(8)]
    cw, ch, cp = compose(contact, 8, 2, 2)
    save_png(os.path.join(ARTIFACTS, "rat-final-appearance-a2-r6-contact-sheet-2048.png"), cw, ch, cp)
    turn = [frames[("idle", i)] for i in (0, 2, 4, 6)]
    tw, th, tp = compose(turn, 4, 1, 4, (24, 21, 20, 255), 64)
    save_png(os.path.join(ARTIFACTS, "rat-final-appearance-a2-r6-turnaround-preview-2048.png"), tw, th, tp)
    used_opaque = sorted({p for pixels in frames.values() for p in pixels if p[3] == 255})
    margins = {
        name: min(v["min_x"], CELL-1-v["max_x"], v["min_y"], CELL-1-v["max_y"])
        for name, v in bounds.items()
    }
    def paw_pose_snapshot(frame):
        scene.frame_set(frame)
        snapshot = {}
        for name, paw in sorted(paws.items()):
            world_corners = [paw.matrix_world @ Vector(corner) for corner in paw.bound_box]
            snapshot[name] = {
                "location": [round(float(v), 6) for v in paw.location],
                "world_bbox_min_z": round(min(float(v.z) for v in world_corners), 6),
            }
        return snapshot

    idle_paws = paw_pose_snapshot(0)
    walk_paws = paw_pose_snapshot(WALK_KEY)
    idle_ground_values = [item["world_bbox_min_z"] for item in idle_paws.values()]
    grounded_walk = sorted(
        name for name, item in walk_paws.items()
        if abs(item["world_bbox_min_z"] - min(v["world_bbox_min_z"] for v in walk_paws.values())) <= 0.0001
    )
    lifted_walk = sorted(set(walk_paws) - set(grounded_walk))
    idle_walk_differences = {
        DIRECTIONS[index][1]: sum(
            1 for idle_pixel, walk_pixel in zip(frames[("idle", index)], frames[("walk-key", index)])
            if idle_pixel != walk_pixel
        )
        for index in range(8)
    }
    scene.frame_set(0)
    settings = {
        "asset_id": "rat-final-appearance-a2-r6",
        "status": "candidate-not-user-approved",
        "blender_version": bpy.app.version_string,
        "source": {
            "blend": "source/rat-final-appearance-a2-r6.blend",
            "reproduction_script": "source/create_rat_final_appearance_a2_r6.py",
            "single_source_all_directions": True,
        },
        "render": {
            "source_resolution": [SOURCE_CELL, SOURCE_CELL],
            "output_resolution": [CELL, CELL],
            "downsample": "4x point sample then shared palette quantization",
            "camera": "orthographic", "orthographic_scale": camera.data.ortho_scale,
            "direction_order": [d for _, d, _ in DIRECTIONS],
        },
        "timeline": {
            "frame_start": 0,
            "frame_end": 8,
            "fps": 8,
            "idle_sample_frame": 0,
            "walk_key_sample_frame": WALK_KEY,
            "walk_loop_frames": [1, 8],
        },
        "pose_contract": {
            "idle": {
                "frame": 0,
                "phase": "neutral_four_paw_ground_contact",
                "paws": idle_paws,
                "all_four_paws_common_ground": max(idle_ground_values) - min(idle_ground_values) <= 0.0001,
            },
            "walk_key": {
                "frame": WALK_KEY,
                "phase": "diagonal_FL_RR_lift",
                "paws": walk_paws,
                "grounded_paws": grounded_walk,
                "lifted_paws": lifted_walk,
            },
            "idle_vs_walk_pixel_difference_count_by_direction": idle_walk_differences,
            "idle_and_walk_differ_in_all_directions": all(value > 0 for value in idle_walk_differences.values()),
        },
        "scope": {
            "idle_directions": 8, "walk_key_directions": 8,
            "full_64_frames": False, "unity_import": False, "runtime_atlas": False,
            "final_user_approval": "pending",
        },
        "verification": {
            "sample_png_count": len(records), "alpha_values": sorted(alpha_values),
            "bounds": bounds, "per_file_bbox_margin_px": margins,
            "minimum_bbox_margin_px": min(margins.values()),
            "all_outputs_within_canvas": min(margins.values()) >= 4,
        },
    }
    with open(os.path.join(ARTIFACTS, "render-settings.json"), "w", encoding="utf-8") as handle:
        json.dump(settings, handle, ensure_ascii=False, indent=2); handle.write("\n")
    palette = {
        "shared_palette_limit": 32, "defined_opaque_colors": len(PALETTE)-1,
        "used_opaque_colors": len(used_opaque), "used_rgba": [list(c) for c in used_opaque],
        "alpha_values": sorted(alpha_values), "binary_alpha": sorted(alpha_values) == [0, 255],
        "dithering": False, "per_file_opaque_color_count": colors_by_file,
    }
    with open(os.path.join(ARTIFACTS, "palette-statistics.json"), "w", encoding="utf-8") as handle:
        json.dump(palette, handle, ensure_ascii=False, indent=2); handle.write("\n")
    if os.path.isdir(RAW_DIR) and not os.listdir(RAW_DIR):
        os.rmdir(RAW_DIR)
    assert len(records) == 16
    assert sorted(alpha_values) == [0, 255]
    assert len(used_opaque) <= 32
    assert min(margins.values()) >= 4
    assert settings["pose_contract"]["idle"]["all_four_paws_common_ground"]
    assert settings["pose_contract"]["walk_key"]["grounded_paws"] == ["Paw_FR", "Paw_RL"]
    assert settings["pose_contract"]["walk_key"]["lifted_paws"] == ["Paw_FL", "Paw_RR"]
    assert settings["pose_contract"]["idle_and_walk_differ_in_all_directions"]
    print(json.dumps({
        "mode": MODE, "samples": len(records), "minimum_margin": min(margins.values()),
        "used_opaque_colors": len(used_opaque), "contact": [cw, ch], "turnaround": [tw, th],
    }, ensure_ascii=False))
