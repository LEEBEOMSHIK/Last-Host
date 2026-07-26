import bpy
import csv
import json
import math
import os
import struct
import zlib
from mathutils import Vector

ROOT = r"C:\project\Last-Host"
SOURCE_OBJ = os.path.join(ROOT, "_workspace", "completed", "2026-07-16-2026-07-16-rat-8-direction-trial-asset", "artifacts", "source", "rat-trial-v1.obj")
OUT = os.path.join(ROOT, "_workspace", "active", "2026-07-20-rat-walk-animation-blender", "artifacts")
RENDERS = os.path.join(OUT, "renders")
BLEND = os.path.join(OUT, "source", "rat-walk-trial-v1.blend")
os.makedirs(RENDERS, exist_ok=True)
os.makedirs(os.path.dirname(BLEND), exist_ok=True)

CELL = 64
PIVOT = (32, 48)
SCALE = 9.0
CAMERA_TO_OBJECT = (6.0, -8.0, 6.0)
KEY_LIGHT = (-0.45, -0.55, 1.0)
DIRECTIONS = (
    (0, "S", -53.130102), (1, "SW", -25.904474), (2, "W", 36.869898), (3, "NW", 99.644270),
    (4, "N", 126.869898), (5, "NE", 154.095526), (6, "E", -143.130102), (7, "SE", -80.355730),
)
PHASES = {
    1: "left-front_right-rear_contact",
    2: "midway_to_pass",
    3: "pass",
    4: "midway_to_opposite_contact",
    5: "right-front_left-rear_contact",
    6: "midway_to_pass",
    7: "pass",
    8: "midway_to_loop",
}
PALETTE = {
    "body": (73, 64, 59, 255), "pink": (190, 128, 124, 255),
    "eye": (27, 29, 31, 255), "eye_glint": (222, 207, 180, 255),
}

def sub(a, b): return tuple(a[i] - b[i] for i in range(3))
def dot(a, b): return sum(a[i] * b[i] for i in range(3))
def cross(a, b): return (a[1]*b[2]-a[2]*b[1], a[2]*b[0]-a[0]*b[2], a[0]*b[1]-a[1]*b[0])
def norm(a):
    length = math.sqrt(max(dot(a, a), 1e-12))
    return tuple(v / length for v in a)
def rotate_z(p, yaw):
    c, s = math.cos(yaw), math.sin(yaw)
    return (p[0]*c-p[1]*s, p[0]*s+p[1]*c, p[2])

def read_obj(path):
    verts, faces, face_mats = [], [], []
    active = "body"
    with open(path, encoding="utf-8") as handle:
        for raw in handle:
            line = raw.strip()
            if line.startswith("v "):
                _, x, y, z = line.split()
                verts.append((float(x), float(y), float(z)))
            elif line.startswith("usemtl "):
                active = line.split(maxsplit=1)[1]
            elif line.startswith("f "):
                ids = [int(item.split("/")[0])-1 for item in line.split()[1:]]
                for i in range(1, len(ids)-1):
                    faces.append((ids[0], ids[i], ids[i+1]))
                    face_mats.append(active)
    lift = -min(v[2] for v in verts)
    return [(x, y, z+lift) for x, y, z in verts], faces, face_mats, lift

BASE, FACES, FACE_MATS, GROUND_LIFT = read_obj(SOURCE_OBJ)

def walk_vertices(frame):
    # f01 pair A contact, f03 pass, f05 pair B contact, f07 pass.
    phase = math.cos((frame - 1) * math.pi / 4.0)
    leg_centers = [(-0.70, 0.47), (-0.70, -0.47), (0.66, 0.47), (0.66, -0.47)]
    # order: rear-left, rear-right, front-left, front-right.  Pair A is LF/RR.
    pair_sign = (1.0, -1.0, -1.0, 1.0)
    result = []
    for x, y, z in BASE:
        dx = 0.0
        # Sole vertices stay planted at local Z=0; only the leg volume above
        # them swings.  This keeps the common sprite ground line invariant.
        if 0.30 <= z < 0.62:
            for leg_i, (cx, cy) in enumerate(leg_centers):
                weight = max(0.0, 1.0 - ((x-cx)/0.52)**2 - ((y-cy)/0.38)**2)
                dx += 0.16 * phase * pair_sign[leg_i] * weight
        # Opposite, one-frame-lag tail motion; no Z displacement anywhere.
        dy = 0.0
        if x < -1.35 and z < 0.78:
            tail_weight = min(1.0, max(0.0, (-x-1.35)/1.45))
            dy = -0.020 * math.sin((frame-1)*math.pi/4.0) * tail_weight
        result.append((x+dx, y+dy, z))
    return result

def build_blend():
    scene = bpy.context.scene
    for obj in list(scene.objects):
        bpy.data.objects.remove(obj, do_unlink=True)
    mesh = bpy.data.meshes.new("rat_walk_trial_mesh")
    mesh.from_pydata(BASE, [], FACES)
    mesh.update()
    rat = bpy.data.objects.new("RatWalkMesh", mesh)
    scene.collection.objects.link(rat)
    mat_index = {}
    for name, rgba in PALETTE.items():
        mat = bpy.data.materials.new("Rat_" + name)
        color = tuple(v/255.0 for v in rgba)
        mat.diffuse_color = color
        mat.use_nodes = True
        bsdf = mat.node_tree.nodes.get("Principled BSDF")
        bsdf.inputs["Base Color"].default_value = color
        bsdf.inputs["Roughness"].default_value = 1.0
        mesh.materials.append(mat)
        mat_index[name] = len(mesh.materials)-1
    for poly, name in zip(mesh.polygons, FACE_MATS):
        poly.material_index = mat_index.get(name, 0)
    root = bpy.data.objects.new("RatWalkRoot", None)
    scene.collection.objects.link(root)
    rat.parent = root
    root.location = (0, 0, 0)
    root.rotation_euler = (0, 0, 0)
    root.scale = (1, 1, 1)
    basis = rat.shape_key_add(name="Basis", from_mix=False)
    for frame in range(1, 9):
        key = rat.shape_key_add(name="walk_f%02d" % frame, from_mix=False)
        for point, co in zip(key.data, walk_vertices(frame)):
            point.co = co
        for at in range(1, 9):
            key.value = 1.0 if at == frame else 0.0
            key.keyframe_insert(data_path="value", frame=at)
        key.value = 0.0
    scene.frame_start, scene.frame_end = 1, 8
    scene.render.resolution_x, scene.render.resolution_y, scene.render.resolution_percentage = 64, 64, 100
    scene.render.image_settings.file_format = "PNG"
    scene.render.image_settings.color_mode = "RGBA"
    scene.render.film_transparent = True
    camera_data = bpy.data.cameras.new("RatWalkCamera")
    camera = bpy.data.objects.new("RatWalkCamera", camera_data)
    scene.collection.objects.link(camera)
    camera.location = CAMERA_TO_OBJECT
    camera.rotation_euler = ((Vector((0,0,0.82))-camera.location).to_track_quat("-Z", "Y")).to_euler()
    camera_data.type = "ORTHO"
    camera_data.ortho_scale = CELL / SCALE
    scene.camera = camera
    light_data = bpy.data.lights.new("RatWalkKeyLight", type="SUN")
    light_data.energy = 2.0
    light = bpy.data.objects.new("RatWalkKeyLight", light_data)
    scene.collection.objects.link(light)
    light.location = (-4.5, -5.5, 10.0)
    light.rotation_euler = ((Vector((0,0,0))-light.location).to_track_quat("-Z", "Y")).to_euler()
    bpy.ops.wm.save_as_mainfile(filepath=BLEND)
    return root

CAM_FORWARD = norm(CAMERA_TO_OBJECT)
CAM_RIGHT = norm(cross(CAM_FORWARD, (0.0, 0.0, 1.0)))
CAM_UP = norm(cross(CAM_RIGHT, CAM_FORWARD))
LIGHT = norm(KEY_LIGHT)
def project(p): return (dot(p, CAM_RIGHT), dot(p, CAM_UP), dot(p, CAM_FORWARD))
def face_color(material, normal):
    if dot(normal, CAM_FORWARD) < 0: normal = tuple(-v for v in normal)
    brightness = 0.55 + 0.45 * max(0.0, dot(normal, LIGHT))
    if material == "body":
        return (73,64,59,255) if brightness < .68 else ((112,96,86,255) if brightness < .84 else (151,129,113,255))
    if material == "pink": return (140,87,88,255) if brightness < .78 else (190,128,124,255)
    return PALETTE[material]
def render(vertices, yaw):
    rotated = [rotate_z(v, yaw) for v in vertices]
    projected = [project(v) for v in rotated]
    screen = [(PIVOT[0]+SCALE*p[0], PIVOT[1]-SCALE*p[1], p[2]) for p in projected]
    pixels, depth = [(0,0,0,0)]*(CELL*CELL), [-1e30]*(CELL*CELL)
    for face, material in zip(FACES, FACE_MATS):
        ia, ib, ic = face
        a,b,c = rotated[ia],rotated[ib],rotated[ic]
        color = face_color(material, norm(cross(sub(b,a),sub(c,a))))
        pa,pb,pc = screen[ia],screen[ib],screen[ic]
        min_x,max_x = max(0,int(math.floor(min(pa[0],pb[0],pc[0])))), min(CELL-1,int(math.ceil(max(pa[0],pb[0],pc[0]))))
        min_y,max_y = max(0,int(math.floor(min(pa[1],pb[1],pc[1])))), min(CELL-1,int(math.ceil(max(pa[1],pb[1],pc[1]))))
        denom = (pb[1]-pc[1])*(pa[0]-pc[0])+(pc[0]-pb[0])*(pa[1]-pc[1])
        if abs(denom) < 1e-9: continue
        for py in range(min_y,max_y+1):
            sy=py+.5
            for px in range(min_x,max_x+1):
                sx=px+.5
                wa=((pb[1]-pc[1])*(sx-pc[0])+(pc[0]-pb[0])*(sy-pc[1]))/denom
                wb=((pc[1]-pa[1])*(sx-pc[0])+(pa[0]-pc[0])*(sy-pc[1]))/denom
                wc=1-wa-wb
                if wa < -1e-7 or wb < -1e-7 or wc < -1e-7: continue
                z=wa*pa[2]+wb*pb[2]+wc*pc[2]
                idx=py*CELL+px
                if z > depth[idx]: depth[idx],pixels[idx]=z,color
    return pixels
def png_bytes(pixels):
    raw=bytearray()
    for y in range(CELL):
        raw.append(0)
        for pixel in pixels[y*CELL:(y+1)*CELL]: raw.extend(pixel)
    def chunk(kind,data): return struct.pack(">I",len(data))+kind+data+struct.pack(">I",zlib.crc32(kind+data)&0xffffffff)
    return b"\x89PNG\r\n\x1a\n"+chunk(b"IHDR",struct.pack(">IIBBBBB",CELL,CELL,8,6,0,0,0))+chunk(b"IDAT",zlib.compress(bytes(raw),9))+chunk(b"IEND",b"")

root = build_blend()
records=[]
bounds=[]
for frame in range(1,9):
    vertices=walk_vertices(frame)
    assert min(v[2] for v in vertices) >= -1e-8
    assert max(abs(v[2]-BASE[i][2]) for i,v in enumerate(vertices)) < 1e-8
    for index,label,yaw_degrees in DIRECTIONS:
        file_name="rat-walk-f%02d-%02d-%s.png" % (frame,index,label.lower())
        path=os.path.join(RENDERS,file_name)
        pixels=render(vertices,math.radians(yaw_degrees))
        with open(path,"wb") as handle: handle.write(png_bytes(pixels))
        alpha=[i for i,p in enumerate(pixels) if p[3]]
        bounds.append((frame,index,min(i%CELL for i in alpha),max(i%CELL for i in alpha),min(i//CELL for i in alpha),max(i//CELL for i in alpha)))
        records.append({"frame":frame,"direction_index":index,"direction":label,"source_yaw_degrees":"%.6f"%yaw_degrees,"file":"renders/"+file_name,"phase":PHASES[frame]})

with open(os.path.join(OUT,"walk-frame-map.csv"),"w",newline="",encoding="utf-8") as handle:
    writer=csv.DictWriter(handle,fieldnames=("frame","direction_index","direction","source_yaw_degrees","file","phase"))
    writer.writeheader(); writer.writerows(records)
settings={
    "asset_id":"rat-walk-trial-v1","status":"trial-placeholder-not-final-art",
    "blender_version":bpy.app.version_string,
    "source":{"read_only_input":SOURCE_OBJ,"root_pivot":[0,0,0],"source_forward":"+X","ground_lift_applied_to_mesh_vertices":GROUND_LIFT},
    "timeline":{"frames":[1,8],"loop_repeat":"frame 9 is frame 1","fps":8},
    "camera":{"projection":"orthographic","camera_to_object":list(CAMERA_TO_OBJECT),"orthographic_scale":CELL/SCALE,"scale_px_per_unit":SCALE,"animated_per_frame":False},
    "lighting":{"type":"single_sun_key","direction":list(KEY_LIGHT),"animated_per_frame":False},
    "canvas":{"width_px":64,"height_px":64,"transparent_background":True,"anti_aliasing":False,"filter_preview":"Point/Nearest"},
    "pivot_px_top_left":list(PIVOT),
    "animation":{"root_location_scale_z_keyed":False,"root_transform_keyframes":0,"leg_motion":"diagonal pairs alternate in local X; no Z displacement","tail_motion":"small opposite one-frame-lag local Y swing"},
    "directions":[{"index":i,"direction":d,"source_yaw_degrees":y} for i,d,y in DIRECTIONS],
    "renderer":"deterministic orthographic triangle z-buffer run inside Blender background Python; no anti-aliasing"
}
with open(os.path.join(OUT,"render-settings-walk.json"),"w",encoding="utf-8") as handle: json.dump(settings,handle,ensure_ascii=False,indent=2); handle.write("\n")

assert len(records)==64
assert all(0 <= b[2] <= b[3] < 64 and 0 <= b[4] <= b[5] < 64 for b in bounds)
print(json.dumps({"blend":BLEND,"png_count":len(records),"min_sole_z":min(v[2] for v in BASE),"max_root_vertical_motion":0.0,"bounds":bounds},ensure_ascii=False))
