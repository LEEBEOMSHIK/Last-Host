"""Create the v2 rat walk trial without altering the v1 source or renders."""
import os

HERE = os.path.dirname(os.path.abspath(__file__))
V1_SCRIPT = os.path.join(HERE, "create_rat_walk_asset.py")

with open(V1_SCRIPT, encoding="utf-8") as handle:
    source = handle.read()

old_start = source.index("def walk_vertices(frame):")
old_end = source.index("\ndef build_blend():", old_start)

walk_vertices_v2 = '''def walk_vertices(frame):
    # Diagonal-pair trot: front-left/rear-right contact on f01, then
    # front-right/rear-left contact on f05.  Swinging paws move forward
    # and upward; planted paws remain on Z=0.
    cycle = (frame - 1) * math.pi / 4.0
    leg_centers = [(-0.70, 0.47), (-0.70, -0.47), (0.66, 0.47), (0.66, -0.47)]
    # rear-left, rear-right, front-left, front-right
    pair_sign = (-1.0, 1.0, 1.0, -1.0)
    result = []
    for x, y, z in BASE:
        dx = dy = dz = 0.0
        for leg_i, (cx, cy) in enumerate(leg_centers):
            radial = max(0.0, 1.0 - ((x-cx)/0.48)**2 - ((y-cy)/0.34)**2)
            # Keep the torso steady; progressively favour lower-leg and paw vertices.
            lower = radial * max(0.0, min(1.0, (0.70-z)/0.48))
            paw = lower * max(0.0, min(1.0, (0.38-z)/0.38))
            gait = pair_sign[leg_i] * math.cos(cycle)
            swing = max(0.0, -gait)
            # The moving paw has the largest stride.  A planted leg only flexes
            # slightly at the upper leg, avoiding a sliding foot on the ground.
            dx += 0.19 * gait * paw + 0.045 * gait * max(0.0, lower-paw)
            dz += 0.13 * swing * paw
        # A restrained counter-swing keeps the tail readable without changing
        # the shared sprite ground line.
        if x < -1.35 and z < 0.78:
            tail_weight = min(1.0, max(0.0, (-x-1.35)/1.45))
            dy = -0.032 * math.sin(cycle) * tail_weight
        result.append((x+dx, y+dy, max(0.0, z+dz)))
    return result
'''

source = source[:old_start] + walk_vertices_v2 + source[old_end:]
source = source.replace('rat-walk-trial-v1.blend', 'rat-walk-trial-v2.blend')
source = source.replace('RENDERS = os.path.join(OUT, "renders")', 'RENDERS = os.path.join(OUT, "renders-v2")')
source = source.replace('"file":"renders/"+file_name', '"file":"renders-v2/"+file_name')
source = source.replace('rat_walk_trial_mesh', 'rat_walk_trial_v2_mesh')
source = source.replace('walk-frame-map.csv', 'walk-frame-map-v2.csv')
source = source.replace('render-settings-walk.json', 'render-settings-walk-v2.json')
source = source.replace('"asset_id":"rat-walk-trial-v1"', '"asset_id":"rat-walk-trial-v2"')
source = source.replace('"leg_motion":"diagonal pairs alternate in local X; no Z displacement"', '"leg_motion":"diagonal pairs alternate; swing paws move in local X and lift in Z while planted paws remain on the ground"')
# v1 prohibited all vertical vertex motion.  In v2 only the swing paw lifts;
# the shared ground plane remains the invariant.
source = source.replace('assert max(abs(v[2]-BASE[i][2]) for i,v in enumerate(vertices)) < 1e-8', 'assert min(v[2] for v in vertices) >= -1e-8')

exec(compile(source, V1_SCRIPT + " [v2 walk patch]", "exec"))
