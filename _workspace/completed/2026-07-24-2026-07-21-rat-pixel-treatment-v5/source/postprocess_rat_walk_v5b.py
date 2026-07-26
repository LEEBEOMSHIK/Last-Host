"""Build v5b PNGs: shared 32-colour palette, hard alpha, no dither.

Anti-aliased coverage is intentionally replaced with opaque intermediate
brightness colours from one set-wide palette.  A conservative component pass
removes only low-contrast, one-pixel quantisation specks; it preserves dark or
bright focal accents and thin silhouette elements.
"""
import glob, json, math, os
from collections import Counter, deque
from PIL import Image

TASK=r"C:\project\Last-Host\_workspace\active\2026-07-21-rat-pixel-treatment-v5"
RAW=os.path.join(TASK,"raw-renders-v5b")
OUT=os.path.join(TASK,"renders-v5b")
REPORT=os.path.join(TASK,"pixel-statistics-v5b.json")
LIMIT, THRESHOLD = 32, 128

def binary(image):
    rgba=image.convert("RGBA")
    alpha=rgba.getchannel("A").point(lambda v:255 if v>=THRESHOLD else 0)
    return rgba.convert("RGB"), alpha

def distance(a,b):
    return math.sqrt(sum((left-right)**2 for left,right in zip(a,b)))

def cleanup(image, alpha):
    """Remove only low-contrast isolated palette specks within solid regions."""
    pixels=image.load(); mask=alpha.load(); width,height=image.size
    visited=set(); changed=0
    for y in range(height):
      for x in range(width):
        if (x,y) in visited or mask[x,y] != 255: continue
        colour=pixels[x,y]; component=[]; queue=[(x,y)]; visited.add((x,y))
        while queue:
          px,py=queue.pop(); component.append((px,py))
          for dy in (-1,0,1):
            for dx in (-1,0,1):
              nx,ny=px+dx,py+dy
              if dx==0 and dy==0 or nx<0 or ny<0 or nx>=width or ny>=height: continue
              if (nx,ny) not in visited and mask[nx,ny]==255 and pixels[nx,ny]==colour:
                visited.add((nx,ny)); queue.append((nx,ny))
        if len(component) != 1: continue
        px,py=component[0]; neighbours=[]
        for dy in (-1,0,1):
          for dx in (-1,0,1):
            nx,ny=px+dx,py+dy
            if dx==0 and dy==0 or nx<0 or ny<0 or nx>=width or ny>=height: continue
            if mask[nx,ny]==255: neighbours.append(pixels[nx,ny])
        if len(neighbours) < 5: continue
        modal,count=Counter(neighbours).most_common(1)[0]
        # Avoid erasing intentional eye/highlight accents and exposed 1px lines.
        if count >= 4 and distance(colour,modal) <= 52:
          pixels[px,py]=modal; changed += 1
    return changed

paths=sorted(glob.glob(os.path.join(RAW,"rat-walk-v5b-*.png")))
if len(paths)!=64: raise RuntimeError("Expected 64 raw v5b PNGs, got %d"%len(paths))
os.makedirs(OUT,exist_ok=True)
raw=[]; opaque=[]
for path in paths:
    rgb,alpha=binary(Image.open(path)); raw.append((path,rgb,alpha))
    opaque.extend(pixel for pixel,a in zip(rgb.getdata(),alpha.getdata()) if a==255)
if not opaque: raise RuntimeError("No opaque pixels")
side=1024; sample=Image.new("RGB",(side,(len(opaque)+side-1)//side),(0,0,0))
sample.putdata(opaque+[(0,0,0)]*(sample.width*sample.height-len(opaque)))
palette_image=sample.quantize(colors=LIMIT,method=Image.Quantize.MEDIANCUT,dither=Image.Dither.NONE)
palette=palette_image.getpalette()[:LIMIT*3]
palette_rgb=[palette[index:index+3] for index in range(0,len(palette),3)]
stats={"png_count":0,"palette_limit_nontransparent":LIMIT,"alpha_threshold":THRESHOLD,"dithering":"none","shared_palette_rgb":palette_rgb,"frames":[]}
for path,rgb,alpha in raw:
    quantized=rgb.quantize(palette=palette_image,dither=Image.Dither.NONE).convert("RGB")
    cleaned=cleanup(quantized,alpha)
    final=Image.merge("RGBA",(*quantized.split(),alpha)); output=os.path.join(OUT,os.path.basename(path)); final.save(output,format="PNG",optimize=False)
    check=Image.open(output).convert("RGBA"); data=list(check.getdata()); alphas=[p[3] for p in data]; colours={p[:3] for p in data if p[3]==255}
    stats["frames"].append({"file":os.path.basename(output),"width":check.width,"height":check.height,"nontransparent_colours":len(colours),"alpha_values":sorted(set(alphas)),"intermediate_alpha_pixels":sum(a not in (0,255) for a in alphas),"isolated_low_contrast_pixels_merged":cleaned})
stats["png_count"]=len(stats["frames"]); stats["maximum_nontransparent_colours"]=max(f["nontransparent_colours"] for f in stats["frames"]); stats["total_intermediate_alpha_pixels"]=sum(f["intermediate_alpha_pixels"] for f in stats["frames"]); stats["total_isolated_low_contrast_pixels_merged"]=sum(f["isolated_low_contrast_pixels_merged"] for f in stats["frames"]); stats["all_dimensions"]=sorted({(f["width"],f["height"]) for f in stats["frames"]})
with open(REPORT,"w",encoding="utf-8") as handle: json.dump(stats,handle,ensure_ascii=False,indent=2);handle.write("\n")
assert stats["png_count"]==64 and stats["all_dimensions"]==[(128,128)] and stats["maximum_nontransparent_colours"]<=LIMIT and stats["total_intermediate_alpha_pixels"]==0
print(json.dumps({"png_count":stats["png_count"],"max_colours":stats["maximum_nontransparent_colours"],"intermediate_alpha":stats["total_intermediate_alpha_pixels"],"cleanup_merged":stats["total_isolated_low_contrast_pixels_merged"]},ensure_ascii=False))
