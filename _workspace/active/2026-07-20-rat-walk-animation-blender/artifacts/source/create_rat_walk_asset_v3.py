"""Create an exaggerated-motion v3 from the read-only v2 Blender source.

Run inside Blender.  This intentionally writes only v3 files and restores the
root transform after directional renders; directions are never keyframed.
"""
import bpy
import csv
import json
import math
import os

ROOT = r"C:\project\Last-Host"
OUT = os.path.join(ROOT, "_workspace", "active", "2026-07-20-rat-walk-animation-blender", "artifacts")
SOURCE = os.path.join(OUT, "source", "rat-walk-trial-v2.blend")
BLEND = os.path.join(OUT, "source", "rat-walk-trial-v3-exaggerated-motion.blend")
RENDERS = os.path.join(OUT, "renders-v3")
MAP = os.path.join(OUT, "walk-frame-map-v3.csv")
SETTINGS = os.path.join(OUT, "render-settings-walk-v3.json")
os.makedirs(RENDERS, exist_ok=True)

DIRECTIONS = (
    (0, "S", -53.130102), (1, "SW", -25.904474), (2, "W", 36.869898), (3, "NW", 99.644270),
    (4, "N", 126.869898), (5, "NE", 154.095526), (6, "E", -143.130102), (7, "SE", -80.355730),
)
PHASES = {
    1: "left-front_right-rear_contact", 2: "lift_and_advance", 3: "pass",
    4: "lift_and_advance", 5: "right-front_left-rear_contact", 6: "lift_and_advance",
    7: "pass", 8: "lift_and_return_to_loop",
}

def clamp01(value):
    return max(0.0, min(1.0, value))

def leg_weight(x, y, cx, cy):
    return max(0.0, 1.0 - ((x-cx)/0.48)**2 - ((y-cy)/0.34)**2)

def author_exaggerated_keys(rat):
    """Replace only v3 shape-key coordinates: no root transform animation."""
    keys = rat.data.shape_keys.key_blocks
    basis = keys["Basis"]
    leg_centers = ((-0.70, 0.47), (-0.70, -0.47), (0.66, 0.47), (0.66, -0.47))
    # rear-left, rear-right, front-left, front-right; FL/RR share contact.
    pair_sign = (-1.0, 1.0, 1.0, -1.0)
    for frame in range(1, 9):
        cycle = (frame - 1) * math.pi / 4.0
        key = keys["walk_f%02d" % frame]
        for index, point in enumerate(key.data):
            base = basis.data[index].co
            x, y, z = base.x, base.y, base.z
            dx = dy = dz = 0.0
            for leg_index, (cx, cy) in enumerate(leg_centers):
                radial = leg_weight(x, y, cx, cy)
                lower = radial * clamp01((0.72-z)/0.50)
                paw = lower * clamp01((0.42-z)/0.42)
                gait = pair_sign[leg_index] * math.cos(cycle)
                swing = max(0.0, -gait)
                # v3: about twice v2 stride, clearly lifted swing paw.
                dx += 0.40 * gait * paw + 0.09 * gait * max(0.0, lower-paw)
                dz += 0.28 * swing * paw
            # Two small body pulses per loop; they are mesh deformation, not root motion.
            torso = clamp01((z-0.46)/0.52) * clamp01((x+1.30)/0.45)
            dz += 0.045 * (math.sin(cycle) ** 2) * torso
            # Tail counter-swing stays compact enough for the 64px canvas.
            if x < -1.35 and z < 0.82:
                tail = clamp01((-x-1.35)/1.45)
                dy += -0.11 * math.sin(cycle) * tail
            point.co = (x + dx, y + dy, max(0.0, z + dz))

def clear_root_animation(root):
    root.animation_data_clear()
    root.location = (0.0, 0.0, 0.0)
    root.rotation_euler = (0.0, 0.0, 0.0)
    root.scale = (1.0, 1.0, 1.0)

bpy.ops.wm.open_mainfile(filepath=SOURCE)
scene = bpy.context.scene
rat = bpy.data.objects["RatWalkMesh"]
root = bpy.data.objects["RatWalkRoot"]
camera = bpy.data.objects["RatWalkCamera"]
light = bpy.data.objects["RatWalkKeyLight"]
author_exaggerated_keys(rat)
clear_root_animation(root)
scene.frame_start, scene.frame_end = 1, 8
scene.render.resolution_x = 64
scene.render.resolution_y = 64
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = "PNG"
scene.render.image_settings.color_mode = "RGBA"
scene.render.film_transparent = True

records, checks = [], []
for frame in range(1, 9):
    scene.frame_set(frame)
    for index, label, yaw_degrees in DIRECTIONS:
        root.rotation_euler = (0.0, 0.0, math.radians(yaw_degrees))
        filename = "rat-walk-v3-f%02d-%02d-%s.png" % (frame, index, label.lower())
        path = os.path.join(RENDERS, filename)
        scene.render.filepath = path
        bpy.ops.render.render(write_still=True)
        image = bpy.data.images.load(path, check_existing=False)
        checks.append((filename, tuple(image.size)))
        bpy.data.images.remove(image)
        records.append({"frame": frame, "direction_index": index, "direction": label,
                        "source_yaw_degrees": "%.6f" % yaw_degrees,
                        "file": "renders-v3/" + filename, "phase": PHASES[frame]})

clear_root_animation(root)
scene.frame_set(1)
bpy.ops.wm.save_as_mainfile(filepath=BLEND)

with open(MAP, "w", newline="", encoding="utf-8") as handle:
    writer = csv.DictWriter(handle, fieldnames=("frame", "direction_index", "direction", "source_yaw_degrees", "file", "phase"))
    writer.writeheader()
    writer.writerows(records)

keys = rat.data.shape_keys.key_blocks
key_minimums = {"f%02d" % frame: min(point.co.z for point in keys["walk_f%02d" % frame].data) for frame in range(1, 9)}
root_fcurves = 0 if not root.animation_data or not root.animation_data.action else len(root.animation_data.action.fcurves)
settings = {
    "asset_id": "rat-walk-trial-v3-exaggerated-motion",
    "status": "trial-placeholder-not-final-art",
    "blender_version": bpy.app.version_string,
    "source": {"read_only_input": SOURCE, "output_blend": BLEND, "root_pivot": [0, 0, 0], "source_forward": "+X"},
    "timeline": {"frames": [1, 8], "loop_repeat": "frame 9 is frame 1", "fps": 8},
    "camera": {"name": camera.name, "projection": camera.data.type, "location": list(camera.location), "orthographic_scale": camera.data.ortho_scale},
    "lighting": {"name": light.name, "type": light.data.type, "location": list(light.location), "energy": light.data.energy},
    "canvas": {"width_px": 64, "height_px": 64, "transparent_background": True, "rendered_with": "bpy.ops.render.render(write_still=True)"},
    "pivot_px_top_left": [32, 48],
    "animation": {"root_location_scale_z_keyed": False, "root_transform_keyframes": root_fcurves,
                  "leg_motion": "exaggerated diagonal-pair trot: 0.40 local-X paw stride and 0.28 local-Z swing lift",
                  "body_motion": "0.045 local-Z mesh-only two-pulse rhythm", "tail_motion": "0.11 local-Y counter-swing"},
    "directions": [{"index": i, "direction": d, "source_yaw_degrees": y} for i, d, y in DIRECTIONS],
    "verification": {"png_count": len(records), "all_png_dimensions": [64, 64], "per_key_min_z": key_minimums,
                     "root_location": list(root.location), "root_scale": list(root.scale), "root_fcurves": root_fcurves}
}
with open(SETTINGS, "w", encoding="utf-8") as handle:
    json.dump(settings, handle, ensure_ascii=False, indent=2)
    handle.write("\n")

assert len(records) == 64
assert all(size == (64, 64) for _, size in checks)
assert min(key_minimums.values()) >= -1e-8
assert root_fcurves == 0
print(json.dumps({"blend": BLEND, "png_count": len(records), "min_sole_z": min(key_minimums.values()), "root_fcurves": root_fcurves}, ensure_ascii=False))
