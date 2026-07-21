"""Render the read-only v4 rat walk asset for the higher-density v5b trial.

This neither modifies v4 nor reuses the v5a output.  The 3D source contract
(camera, key light, direction yaw, shape-key walk, root and ground contact) is
kept; a separate postprocess creates the v5b pixels.
"""
import csv
import json
import math
import os
import bpy

ROOT = r"C:\project\Last-Host"
TASK = os.path.join(ROOT, "_workspace", "active", "2026-07-21-rat-pixel-treatment-v5")
INPUT_BLEND = os.path.join(ROOT, "_workspace", "active", "2026-07-21-character-sprite-resolution-standard", "source", "rat-walk-trial-v4-128px.blend")
OUTPUT_BLEND = os.path.join(TASK, "source", "rat-walk-trial-v5b-high-density-pixel.blend")
RAW_RENDERS = os.path.join(TASK, "raw-renders-v5b")
FRAME_MAP = os.path.join(TASK, "walk-frame-map-v5b.csv")
SETTINGS = os.path.join(TASK, "render-settings-walk-v5b.json")
DIRECTIONS = ((0, "S", -53.130102), (1, "SW", -25.904474), (2, "W", 36.869898), (3, "NW", 99.644270), (4, "N", 126.869898), (5, "NE", 154.095526), (6, "E", -143.130102), (7, "SE", -80.355730))
PHASES = {1: "left-front_right-rear_contact", 2: "lift_and_advance", 3: "pass", 4: "lift_and_advance", 5: "right-front_left-rear_contact", 6: "lift_and_advance", 7: "pass", 8: "lift_and_return_to_loop"}

def no_root_motion(root):
    root.animation_data_clear()
    root.location, root.rotation_euler, root.scale = (0, 0, 0), (0, 0, 0), (1, 1, 1)

os.makedirs(os.path.dirname(OUTPUT_BLEND), exist_ok=True)
os.makedirs(RAW_RENDERS, exist_ok=True)
bpy.ops.wm.open_mainfile(filepath=INPUT_BLEND)
scene = bpy.context.scene
rat, root = bpy.data.objects["RatWalkMesh"], bpy.data.objects["RatWalkRoot"]
camera, light = bpy.data.objects["RatWalkCamera"], bpy.data.objects["RatWalkKeyLight"]
scene.frame_start, scene.frame_end = 1, 8
scene.render.resolution_x, scene.render.resolution_y, scene.render.resolution_percentage = 128, 128, 100
scene.render.image_settings.file_format, scene.render.image_settings.color_mode = "PNG", "RGBA"
scene.render.film_transparent = True
scene.eevee.taa_samples = 1
scene.eevee.taa_render_samples = 1
scene.render.filter_size = 0.01
no_root_motion(root)
records, checks = [], []
for frame in range(1, 9):
    scene.frame_set(frame)
    for index, direction, yaw in DIRECTIONS:
        root.rotation_euler = (0, 0, math.radians(yaw))
        filename = "rat-walk-v5b-f%02d-%02d-%s.png" % (frame, index, direction.lower())
        path = os.path.join(RAW_RENDERS, filename)
        scene.render.filepath = path
        bpy.ops.render.render(write_still=True)
        image = bpy.data.images.load(path, check_existing=False)
        checks.append((image.size[0], image.size[1], image.channels))
        bpy.data.images.remove(image)
        records.append({"frame": frame, "direction_index": index, "direction": direction, "source_yaw_degrees": "%.6f" % yaw, "file": "renders-v5b/" + filename, "phase": PHASES[frame]})
no_root_motion(root)
scene.frame_set(1)
bpy.ops.wm.save_as_mainfile(filepath=OUTPUT_BLEND)
with open(FRAME_MAP, "w", newline="", encoding="utf-8") as handle:
    writer = csv.DictWriter(handle, fieldnames=("frame", "direction_index", "direction", "source_yaw_degrees", "file", "phase"))
    writer.writeheader(); writer.writerows(records)
keys = rat.data.shape_keys.key_blocks
key_minimums = {"f%02d" % f: min(point.co.z for point in keys["walk_f%02d" % f].data) for f in range(1, 9)}
root_fcurves = 0 if not root.animation_data or not root.animation_data.action else len(root.animation_data.action.fcurves)
settings = {
  "asset_id": "rat-walk-trial-v5b-high-density-pixel", "status": "trial-placeholder-not-final-art", "blender_version": bpy.app.version_string,
  "source": {"read_only_input": INPUT_BLEND, "output_blend": OUTPUT_BLEND, "content_change_from_v4": "single-sample raw render for a separate shared-32-colour v5b pixel treatment", "root_pivot": [0,0,0], "source_forward": "+X"},
  "timeline": {"frames": [1,8], "loop_repeat": "f08 advances to f01", "fps": 8},
  "camera": {"name":camera.name, "projection":camera.data.type, "location":list(camera.location), "orthographic_scale":camera.data.ortho_scale},
  "lighting": {"name":light.name, "type":light.data.type, "location":list(light.location), "energy":light.data.energy},
  "canvas": {"width_px":128, "height_px":128, "transparent_background":True, "color_mode":"RGBA", "eevee_taa_samples":1, "eevee_taa_render_samples":1, "reconstruction_filter_size":scene.render.filter_size},
  "pixel_postprocess": {"script":"source/postprocess_rat_walk_v5b.py", "shared_palette_limit_nontransparent":32, "alpha":"binary 0/255", "edge_treatment":"opaque intermediate-brightness palette clusters", "dithering":"none", "isolated_pixel_cleanup":"only low-contrast one-pixel components surrounded by a dominant opaque neighbour colour"},
  "unity_handoff": {"pixels_per_unit":64, "custom_pivot_normalized":[0.5,0.25], "pivot_px_top_left":[64,96]},
  "animation": {"shape_keys":["walk_f%02d"%f for f in range(1,9)], "frame_phase_sequence":[PHASES[f] for f in range(1,9)], "root_location_scale_z_keyed":False, "root_transform_keyframes":root_fcurves},
  "directions":[{"index":i,"direction":d,"source_yaw_degrees":y} for i,d,y in DIRECTIONS],
  "verification_before_postprocess":{"png_count":len(records),"all_png_dimensions":[128,128],"all_png_channels":"RGBA","per_key_min_z":key_minimums,"root_location":list(root.location),"root_scale":list(root.scale),"root_fcurves":root_fcurves,"loop_transition":"f08 -> f01","camera_lighting_preserved_from_v4":True}
}
with open(SETTINGS,"w",encoding="utf-8") as handle: json.dump(settings,handle,ensure_ascii=False,indent=2); handle.write("\n")
assert len(records)==64 and all(item==(128,128,4) for item in checks)
assert min(key_minimums.values()) >= -1e-8 and root_fcurves==0 and tuple(root.location)==(0,0,0) and tuple(root.scale)==(1,1,1)
print(json.dumps({"blend":OUTPUT_BLEND,"raw_png_count":len(records),"size":[128,128],"min_sole_z":min(key_minimums.values()),"root_fcurves":root_fcurves},ensure_ascii=False))
