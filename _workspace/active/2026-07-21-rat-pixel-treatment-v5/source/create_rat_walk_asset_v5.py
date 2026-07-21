"""Render the v5 rat walk source with single-sample, pixel-oriented settings.

v4 is read only.  This script preserves v4's meshes, keyed walk poses, root,
camera, lighting, direction yaw and 128px canvas.  It emits raw single-sample
RGBA frames for the separate deterministic 16-colour / binary-alpha step.
"""
import csv
import json
import math
import os

import bpy

ROOT = r"C:\project\Last-Host"
TASK = os.path.join(ROOT, "_workspace", "active", "2026-07-21-rat-pixel-treatment-v5")
INPUT_BLEND = os.path.join(ROOT, "_workspace", "active", "2026-07-21-character-sprite-resolution-standard", "source", "rat-walk-trial-v4-128px.blend")
OUTPUT_BLEND = os.path.join(TASK, "source", "rat-walk-trial-v5-pixel-treatment.blend")
RAW_RENDERS = os.path.join(TASK, "raw-renders-v5")
FRAME_MAP = os.path.join(TASK, "walk-frame-map-v5.csv")
SETTINGS = os.path.join(TASK, "render-settings-walk-v5.json")

DIRECTIONS = (
    (0, "S", -53.130102), (1, "SW", -25.904474), (2, "W", 36.869898), (3, "NW", 99.644270),
    (4, "N", 126.869898), (5, "NE", 154.095526), (6, "E", -143.130102), (7, "SE", -80.355730),
)
PHASES = {
    1: "left-front_right-rear_contact", 2: "lift_and_advance", 3: "pass",
    4: "lift_and_advance", 5: "right-front_left-rear_contact", 6: "lift_and_advance",
    7: "pass", 8: "lift_and_return_to_loop",
}


def no_root_motion(root):
    root.animation_data_clear()
    root.location = (0.0, 0.0, 0.0)
    root.rotation_euler = (0.0, 0.0, 0.0)
    root.scale = (1.0, 1.0, 1.0)


os.makedirs(os.path.dirname(OUTPUT_BLEND), exist_ok=True)
os.makedirs(RAW_RENDERS, exist_ok=True)
bpy.ops.wm.open_mainfile(filepath=INPUT_BLEND)
scene = bpy.context.scene
rat = bpy.data.objects["RatWalkMesh"]
root = bpy.data.objects["RatWalkRoot"]
camera = bpy.data.objects["RatWalkCamera"]
light = bpy.data.objects["RatWalkKeyLight"]

scene.frame_start, scene.frame_end = 1, 8
scene.render.resolution_x = 128
scene.render.resolution_y = 128
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = "PNG"
scene.render.image_settings.color_mode = "RGBA"
scene.render.film_transparent = True
# Eevee single sampling and near-zero reconstruction filter prevent Blender
# from adding multi-sample edge smoothing before the explicit pixel pass.
scene.eevee.taa_samples = 1
scene.eevee.taa_render_samples = 1
scene.render.filter_size = 0.01
no_root_motion(root)

records, image_checks = [], []
for frame in range(1, 9):
    scene.frame_set(frame)
    for index, direction, yaw in DIRECTIONS:
        root.rotation_euler = (0.0, 0.0, math.radians(yaw))
        filename = "rat-walk-v5-f%02d-%02d-%s.png" % (frame, index, direction.lower())
        output_path = os.path.join(RAW_RENDERS, filename)
        scene.render.filepath = output_path
        bpy.ops.render.render(write_still=True)
        image = bpy.data.images.load(output_path, check_existing=False)
        image_checks.append({"file": filename, "width": image.size[0], "height": image.size[1], "channels": image.channels})
        bpy.data.images.remove(image)
        records.append({
            "frame": frame, "direction_index": index, "direction": direction,
            "source_yaw_degrees": "%.6f" % yaw, "file": "renders-v5/" + filename,
            "phase": PHASES[frame],
        })

no_root_motion(root)
scene.frame_set(1)
bpy.ops.wm.save_as_mainfile(filepath=OUTPUT_BLEND)
with open(FRAME_MAP, "w", newline="", encoding="utf-8") as handle:
    writer = csv.DictWriter(handle, fieldnames=("frame", "direction_index", "direction", "source_yaw_degrees", "file", "phase"))
    writer.writeheader()
    writer.writerows(records)

keys = rat.data.shape_keys.key_blocks
key_names = ["walk_f%02d" % frame for frame in range(1, 9)]
key_minimums = {"f%02d" % frame: min(point.co.z for point in keys["walk_f%02d" % frame].data) for frame in range(1, 9)}
root_fcurves = 0 if not root.animation_data or not root.animation_data.action else len(root.animation_data.action.fcurves)
settings = {
    "asset_id": "rat-walk-trial-v5-pixel-treatment", "status": "trial-placeholder-not-final-art",
    "blender_version": bpy.app.version_string,
    "source": {"read_only_input": INPUT_BLEND, "output_blend": OUTPUT_BLEND, "root_pivot": [0, 0, 0], "source_forward": "+X", "content_change_from_v4": "single-sample raw render; deterministic postprocess applies binary alpha and a shared 16-colour palette"},
    "timeline": {"frames": [1, 8], "loop_repeat": "f08 advances to f01", "fps": 8},
    "camera": {"name": camera.name, "projection": camera.data.type, "location": list(camera.location), "orthographic_scale": camera.data.ortho_scale},
    "lighting": {"name": light.name, "type": light.data.type, "location": list(light.location), "energy": light.data.energy},
    "canvas": {"width_px": 128, "height_px": 128, "transparent_background": True, "color_mode": "RGBA", "raw_rendered_with": "bpy.ops.render.render(write_still=True)", "eevee_taa_samples": 1, "eevee_taa_render_samples": 1, "reconstruction_filter_size": scene.render.filter_size},
    "pixel_postprocess": {"script": "source/postprocess_rat_walk_v5.py", "palette_limit_nontransparent": 16, "alpha": "binary threshold >= 128", "dithering": "none", "palette": "one shared adaptive palette derived from all v5 opaque source pixels"},
    "unity_handoff": {"pixels_per_unit": 64, "custom_pivot_normalized": [0.5, 0.25], "pivot_px_top_left": [64, 96]},
    "animation": {"shape_keys": key_names, "frame_phase_sequence": [PHASES[frame] for frame in range(1, 9)], "root_location_scale_z_keyed": False, "root_transform_keyframes": root_fcurves},
    "directions": [{"index": index, "direction": direction, "source_yaw_degrees": yaw} for index, direction, yaw in DIRECTIONS],
    "verification_before_postprocess": {"png_count": len(records), "all_png_dimensions": [128, 128], "all_png_channels": "RGBA", "per_key_min_z": key_minimums, "root_location": list(root.location), "root_scale": list(root.scale), "root_fcurves": root_fcurves, "loop_transition": "f08 -> f01", "camera_lighting_preserved_from_v4": True},
}
with open(SETTINGS, "w", encoding="utf-8") as handle:
    json.dump(settings, handle, ensure_ascii=False, indent=2)
    handle.write("\n")

assert len(records) == 64
assert all(check["width"] == 128 and check["height"] == 128 and check["channels"] == 4 for check in image_checks)
assert set(keys.keys()) >= set(key_names)
assert min(key_minimums.values()) >= -1e-8
assert root_fcurves == 0
assert tuple(root.location) == (0.0, 0.0, 0.0) and tuple(root.scale) == (1.0, 1.0, 1.0)
print(json.dumps({"blend": OUTPUT_BLEND, "raw_png_count": len(records), "size": [128, 128], "min_sole_z": min(key_minimums.values()), "root_fcurves": root_fcurves}, ensure_ascii=False))
