"""Create the final v5 PNGs with a shared 16-colour palette and hard alpha.

Run with the workstation Python that has Pillow after Blender has produced
raw-renders-v5.  No dither is used.  The same adaptive palette is calculated
from every opaque pixel across the full 8-direction, 8-frame walk set so colour
choices cannot flicker merely because a different frame is shown.
"""
import glob
import json
import os
from collections import Counter

from PIL import Image

TASK = r"C:\project\Last-Host\_workspace\active\2026-07-21-rat-pixel-treatment-v5"
RAW = os.path.join(TASK, "raw-renders-v5")
OUT = os.path.join(TASK, "renders-v5")
REPORT = os.path.join(TASK, "pixel-statistics-v5.json")
ALPHA_THRESHOLD = 128
PALETTE_LIMIT = 16


def binary_alpha(image):
    rgba = image.convert("RGBA")
    alpha = rgba.getchannel("A").point(lambda value: 255 if value >= ALPHA_THRESHOLD else 0)
    rgb = rgba.convert("RGB")
    return rgb, alpha


paths = sorted(glob.glob(os.path.join(RAW, "rat-walk-v5-*.png")))
if len(paths) != 64:
    raise RuntimeError("Expected 64 raw v5 PNGs, got %d" % len(paths))
os.makedirs(OUT, exist_ok=True)

# A compact 1024x1024 sample sheet is sufficient for one shared median-cut
# palette while retaining every opaque source pixel (at most 1,048,576).
opaque = []
raw_data = []
for path in paths:
    rgb, alpha = binary_alpha(Image.open(path))
    raw_data.append((path, rgb, alpha))
    opaque.extend(pixel for pixel, a in zip(rgb.getdata(), alpha.getdata()) if a == 255)
if not opaque:
    raise RuntimeError("No opaque pixels in v5 raw output")
side = 1024
sample = Image.new("RGB", (side, (len(opaque) + side - 1) // side), (0, 0, 0))
sample.putdata(opaque + [(0, 0, 0)] * (sample.width * sample.height - len(opaque)))
palette_image = sample.quantize(colors=PALETTE_LIMIT, method=Image.Quantize.MEDIANCUT, dither=Image.Dither.NONE)

stats = {"frames": [], "palette_limit_nontransparent": PALETTE_LIMIT, "alpha_threshold": ALPHA_THRESHOLD, "dithering": "none"}
for path, rgb, alpha in raw_data:
    indexed = rgb.quantize(palette=palette_image, dither=Image.Dither.NONE)
    quantized = indexed.convert("RGB")
    final = Image.merge("RGBA", (*quantized.split(), alpha))
    output = os.path.join(OUT, os.path.basename(path))
    final.save(output, format="PNG", optimize=False)
    check = Image.open(output).convert("RGBA")
    alphas = list(check.getchannel("A").getdata())
    colors = {pixel[:3] for pixel in check.getdata() if pixel[3] == 255}
    stats["frames"].append({"file": os.path.basename(output), "width": check.width, "height": check.height, "nontransparent_colours": len(colors), "alpha_values": sorted(set(alphas)), "intermediate_alpha_pixels": sum(1 for value in alphas if value not in (0, 255))})

stats["png_count"] = len(stats["frames"])
stats["maximum_nontransparent_colours"] = max(frame["nontransparent_colours"] for frame in stats["frames"])
stats["total_intermediate_alpha_pixels"] = sum(frame["intermediate_alpha_pixels"] for frame in stats["frames"])
stats["all_dimensions"] = sorted({(frame["width"], frame["height"]) for frame in stats["frames"]})
with open(REPORT, "w", encoding="utf-8") as handle:
    json.dump(stats, handle, ensure_ascii=False, indent=2)
    handle.write("\n")

assert stats["png_count"] == 64
assert stats["all_dimensions"] == [(128, 128)]
assert stats["maximum_nontransparent_colours"] <= PALETTE_LIMIT
assert stats["total_intermediate_alpha_pixels"] == 0
print(json.dumps({"png_count": stats["png_count"], "max_colours": stats["maximum_nontransparent_colours"], "intermediate_alpha": stats["total_intermediate_alpha_pixels"]}, ensure_ascii=False))
