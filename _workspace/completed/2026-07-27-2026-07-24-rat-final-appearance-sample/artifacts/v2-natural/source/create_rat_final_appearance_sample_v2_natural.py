"""Create the non-destructive v2-natural rat sample from the v1 recipe.

The v1 source and outputs are read-only inputs.  This script executes a
controlled variant of the proven v1 renderer and writes exclusively beneath
artifacts/v2-natural/.  It keeps the same camera language, direction map,
palette contract, animation poses and technical verification.
"""
import bpy
import hashlib
import json
import os

ROOT = r"C:\project\Last-Host"
TASK = os.path.join(ROOT, "_workspace", "active", "2026-07-24-rat-final-appearance-sample")
V1_ARTIFACTS = os.path.join(TASK, "artifacts")
V1_SCRIPT = os.path.join(V1_ARTIFACTS, "source", "create_rat_final_appearance_sample.py")
V2_ARTIFACTS = os.path.join(V1_ARTIFACTS, "v2-natural")
V2_SCRIPT_RELATIVE = "source/create_rat_final_appearance_sample_v2_natural.py"
COMPARISON_PATH = os.path.join(V2_ARTIFACTS, "rat-final-appearance-v1-v2-natural-comparison-2048.png")


def replace_once(source, old, new, label):
    count = source.count(old)
    if count != 1:
        raise RuntimeError("%s replacement expected once, found %d" % (label, count))
    return source.replace(old, new, 1)


def replace_first(source, old, new, label):
    if old not in source:
        raise RuntimeError("%s replacement source not found" % label)
    return source.replace(old, new, 1)


with open(V1_SCRIPT, encoding="utf-8") as handle:
    source = handle.read()

source = replace_first(
    source,
    'ARTIFACTS = os.path.join(TASK, "artifacts")',
    'ARTIFACTS = os.path.join(TASK, "artifacts", "v2-natural")',
    "output root",
)
source = replace_first(
    source,
    '"rat-final-appearance-sample-v1.blend"',
    '"rat-final-appearance-sample-v2-natural.blend"',
    "blend filename",
)
source = replace_first(
    source,
    '"rat-final-appearance-contact-sheet-2048.png"',
    '"rat-final-appearance-v2-natural-contact-sheet-2048.png"',
    "contact sheet filename",
)
source = replace_first(
    source,
    '"rat-final-appearance-turnaround-preview-2048.png"',
    '"rat-final-appearance-v2-natural-turnaround-preview-2048.png"',
    "turnaround filename",
)
source = source.replace(
    '"rat-final-sample-idle-%02d-%s.png"',
    '"rat-final-sample-v2-natural-idle-%02d-%s.png"',
)
source = source.replace(
    '"rat-final-sample-walk-f%02d-%02d-%s.png"',
    '"rat-final-sample-v2-natural-walk-f%02d-%02d-%s.png"',
)
source = replace_once(
    source,
    '"rat-final-appearance-sample-v1"',
    '"rat-final-appearance-sample-v2-natural"',
    "asset id",
)
source = replace_once(
    source,
    '"source/create_rat_final_appearance_sample.py"',
    '"source/create_rat_final_appearance_sample_v2_natural.py"',
    "reproduction script setting",
)
source = replace_once(
    source,
    '"RatFinalSampleRoot"',
    '"RatFinalSampleV2NaturalRoot"',
    "root object name",
)
source = replace_once(
    source,
    '"near-final-candidate-not-user-approved"',
    '"near-final-v2-natural-candidate-not-user-approved"',
    "root status",
)
source = replace_once(
    source,
    '"brown sewer rat; cute 60 / uncanny 40"',
    '"natural warm-brown sewer rat; cute 60 / uncanny 40"',
    "appearance direction",
)
source = replace_once(
    source,
    '"near-final-appearance-candidate-not-user-approved"',
    '"near-final-v2-natural-candidate-not-user-approved"',
    "settings status",
)
source = replace_once(
    source,
    '"new low-poly model; completed v5b and Blender work were read-only visual/technical references"',
    '"v2-natural low-poly model; v1 recipe and completed work were read-only references"',
    "source note",
)

# Moderate surface-density increase plus smooth normals on body-scale forms.
source = replace_first(
    source,
    "for polygon in obj.data.polygons:\n        polygon.use_smooth = False\n    return parent_to_root(obj, root)",
    "for polygon in obj.data.polygons:\n        polygon.use_smooth = subdivisions >= 2\n    return parent_to_root(obj, root)",
    "ico smoothing",
)
source = replace_first(
    source,
    "for polygon in obj.data.polygons:\n        polygon.use_smooth = False\n    return parent_to_root(obj, root)",
    "for polygon in obj.data.polygons:\n        polygon.use_smooth = True\n    return parent_to_root(obj, root)",
    "uv smoothing",
)
source = replace_once(
    source,
    """def add_snout(name, location, material, root):
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
""",
    """def add_snout(name, location, material, root):
    bpy.ops.mesh.primitive_cone_add(
        vertices=12,
        radius1=0.36,
        radius2=0.13,
        depth=0.78,
        location=location,
        rotation=(0.0, math.radians(90.0), 0.0),
    )
    obj = bpy.context.object
    obj.name = name
    obj.scale = (1.0, 0.88, 0.88)
    obj.data.materials.append(material)
    for polygon in obj.data.polygons:
        polygon.use_smooth = True
    return parent_to_root(obj, root)
""",
    "natural snout",
)

source = replace_once(
    source,
    """    body = add_ico("Body", (-0.30, 0.0, 0.78), (1.38, 0.68, 0.62), mats["coat_warm"], root, 2)
    add_ico("BackCoat", (-0.50, 0.0, 0.93), (1.15, 0.61, 0.50), mats["coat_dark"], root, 1)
    belly = add_ico("Belly", (-0.17, 0.0, 0.51), (1.02, 0.58, 0.31), mats["belly"], root, 2)
    neck = add_ico("Neck", (0.63, 0.0, 0.82), (0.58, 0.56, 0.53), mats["coat_light"], root, 1)
    head = add_ico("Head", (0.93, 0.0, 0.89), (0.70, 0.56, 0.55), mats["coat_light"], root, 2)
    snout = add_snout("Snout", (1.38, 0.0, 0.78), mats["belly"], root)
    nose = add_ico("Nose", (1.80, 0.0, 0.78), (0.17, 0.15, 0.14), mats["nose"], root, 1)
""",
    """    body = add_ico("Body", (-0.30, 0.0, 0.77), (1.40, 0.68, 0.60), mats["coat_warm"], root, 3)
    add_ico("BackCoat", (-0.50, 0.0, 0.91), (1.17, 0.62, 0.47), mats["coat_dark"], root, 2)
    belly = add_ico("Belly", (-0.17, 0.0, 0.50), (1.00, 0.56, 0.28), mats["belly"], root, 3)
    neck = add_ico("Neck", (0.63, 0.0, 0.81), (0.60, 0.54, 0.50), mats["coat_warm"], root, 2)
    head = add_ico("Head", (0.94, 0.0, 0.87), (0.69, 0.54, 0.52), mats["coat_warm"], root, 3)
    snout = add_snout("Snout", (1.40, 0.0, 0.77), mats["coat_light"], root)
    nose = add_ico("Nose", (1.81, 0.0, 0.77), (0.16, 0.14, 0.13), mats["nose"], root, 2)
""",
    "natural body and face",
)
source = replace_once(
    source,
    """        add_uv("EarOuter_" + side, (0.72, y, 1.27), (0.32, 0.12, 0.40), mats["skin_dark"], root, 10, 5)
        add_uv("EarInner_" + side, (0.74, y * 1.055, 1.28), (0.23, 0.055, 0.29), mats["skin_light"], root, 10, 5)
        eye = add_ico("Eye_" + side, (1.17, y * 0.83, 1.00), (0.095, 0.070, 0.095), mats["eye"], root, 1)
""",
    """        add_uv("EarOuter_" + side, (0.72, y, 1.25), (0.31, 0.12, 0.38), mats["skin_dark"], root, 14, 7)
        add_uv("EarInner_" + side, (0.74, y * 1.055, 1.26), (0.22, 0.055, 0.27), mats["skin_light"], root, 14, 7)
        eye = add_ico("Eye_" + side, (1.18, y * 0.83, 0.98), (0.090, 0.067, 0.090), mats["eye"], root, 2)
""",
    "natural ears and eyes",
)
source = replace_once(
    source,
    """    paw_specs = {
        "Paw_FL": ((0.64, 0.48, 0.13), (0.35, 0.20, 0.13)),
        "Paw_FR": ((0.64, -0.48, 0.13), (0.35, 0.20, 0.13)),
        "Paw_RL": ((-0.77, 0.50, 0.14), (0.42, 0.23, 0.15)),
        "Paw_RR": ((-0.77, -0.50, 0.14), (0.42, 0.23, 0.15)),
    }
    paws = {}
    for name, (location, scale) in paw_specs.items():
        paws[name] = add_uv(name, location, scale, mats["skin"], root, 8, 4)
""",
    """    paw_specs = {
        "Paw_FL": ((0.66, 0.47, 0.085), (0.32, 0.18, 0.075)),
        "Paw_FR": ((0.66, -0.47, 0.085), (0.32, 0.18, 0.075)),
        "Paw_RL": ((-0.77, 0.49, 0.090), (0.36, 0.20, 0.080)),
        "Paw_RR": ((-0.77, -0.49, 0.090), (0.36, 0.20, 0.080)),
    }
    paws = {}
    for name, (location, scale) in paw_specs.items():
        paws[name] = add_uv(name, location, scale, mats["skin"], root, 12, 6)
""",
    "natural paws",
)

# Execute the proven renderer with only the controlled v2 substitutions above.
exec(compile(source, os.path.join(V2_ARTIFACTS, V2_SCRIPT_RELATIVE) + "::transformed-v1-renderer", "exec"), globals())


def load_palette_png(path):
    image = bpy.data.images.load(path, check_existing=False)
    width, height = image.size
    data = list(image.pixels)
    pixels = []
    for y in range(height - 1, -1, -1):
        for x in range(width):
            offset = (y * width + x) * 4
            alpha = 255 if data[offset + 3] >= 0.5 else 0
            if alpha == 0:
                pixels.append((0, 0, 0, 0))
            else:
                pixels.append(tuple(
                    int(round(linear_to_srgb(data[offset + channel]) * 255.0))
                    for channel in range(3)
                ) + (255,))
    bpy.data.images.remove(image)
    return pixels


# v1/v2 idle pairs: S,SW,W,NW on row 1 and N,NE,E,SE on row 2.
comparison_frames = []
for index, label, _yaw in DIRECTIONS:
    v1_name = "rat-final-sample-idle-%02d-%s.png" % (index, label.lower())
    v1_path = os.path.join(V1_ARTIFACTS, "renders", "idle", v1_name)
    comparison_frames.extend((load_palette_png(v1_path), all_frames[("idle", index)]))
comparison_width, comparison_height, comparison_pixels = compose_nearest(
    comparison_frames,
    cols=8,
    rows=2,
    scale=2,
)
save_png(COMPARISON_PATH, comparison_width, comparison_height, comparison_pixels)


def sha256(path):
    digest = hashlib.sha256()
    with open(path, "rb") as handle:
        for block in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


with open(SETTINGS_PATH, encoding="utf-8") as handle:
    settings_v2 = json.load(handle)
settings_v2["appearance"]["natural_v2_changes"] = {
    "surface": "moderately increased body/head segments with smooth normals; low-poly silhouette retained",
    "shading": "large organic coat/back/belly/muzzle clusters; reduced triangular facet breakup",
    "face": "soft twelve-sided wedge muzzle and reduced cream boundary",
    "feet": "flatter oval paws with smaller vertical mass",
    "preserved": "ear, eye, nose and tail direction; gait poses; camera language; palette contract",
}
settings_v2["comparison"] = {
    "file": os.path.relpath(COMPARISON_PATH, V2_ARTIFACTS).replace("\\", "/"),
    "width_px": comparison_width,
    "height_px": comparison_height,
    "layout": "each direction is v1 then v2-natural; S/SW/W/NW row 1, N/NE/E/SE row 2",
    "scale": "2x nearest",
}
settings_v2["v1_read_only_reference_sha256"] = {
    "blend": sha256(os.path.join(V1_ARTIFACTS, "source", "rat-final-appearance-sample-v1.blend")),
    "script": sha256(V1_SCRIPT),
    "contact_sheet": sha256(os.path.join(V1_ARTIFACTS, "rat-final-appearance-contact-sheet-2048.png")),
}
with open(SETTINGS_PATH, "w", encoding="utf-8") as handle:
    json.dump(settings_v2, handle, ensure_ascii=False, indent=2)
    handle.write("\n")

with open(PALETTE_STATS, encoding="utf-8") as handle:
    palette_v2 = json.load(handle)
palette_v2["variant"] = "v2-natural"
palette_v2["comparison_generated"] = os.path.basename(COMPARISON_PATH)
with open(PALETTE_STATS, "w", encoding="utf-8") as handle:
    json.dump(palette_v2, handle, ensure_ascii=False, indent=2)
    handle.write("\n")

assert comparison_width == 2048 and comparison_height == 512
assert settings_v2["verification"]["minimum_bbox_margin_px"] >= 4
assert palette_v2["used_opaque_colors"] <= 32
print(json.dumps({
    "variant": "v2-natural",
    "output": V2_ARTIFACTS,
    "blend": BLEND_PATH,
    "samples": len(records),
    "minimum_bbox_margin_px": settings_v2["verification"]["minimum_bbox_margin_px"],
    "used_opaque_colors": palette_v2["used_opaque_colors"],
    "comparison": [comparison_width, comparison_height],
}, ensure_ascii=False))
