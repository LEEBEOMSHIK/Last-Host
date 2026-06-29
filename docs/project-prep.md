# The Last Host Project Preparation

Last updated: 2026-06-29

## Purpose

This document organizes the current preparation state for `마지막 숙주 / The Last Host` before implementation starts. It summarizes confirmed decisions, open approvals, repository state, and the recommended order for turning the design draft into a Unity project.

## Current Repository State

- Workspace: `C:\project\Last-Host`
- Git remote: `https://github.com/LEEBEOMSHIK/Last-Host.git`
- Current repository content is minimal. There is no Unity project yet.
- The current task is documentation and preparation only.

## Original Planning Source

- Draft document: `C:\project\game\last_host\last_host_game_plan.docx`
- The draft defines a pixel-style low-poly 3D evolution survival roguelike.
- The draft recommends validating the core loop with a `Rat Host Prototype`.

## Confirmed Decisions

- Engine: Unity 3D
- Visual style: pixel-style 3D, not pure 2D sprite art
- Rendering direction:
  - low-poly 3D models
  - pixel-style textures
  - low-resolution rendering or post-processing
- Game structure:
  - external host-control exploration
  - internal virus immune-system minigame
  - mutation selection and growth
- First prototype focus: rat host in a sewer environment

## Open Approval Items

These should be approved before implementation work starts:

1. Unity version
2. URP usage
3. PC-only prototype target
4. Whether the first prototype excludes the insect tutorial
5. Exact first prototype win/fail conditions
6. Initial control scheme
7. Initial UI gauge set
8. Placeholder asset strategy
9. Git ignore and Unity project metadata policy

## Recommended Documentation Set

- `docs/game-design-summary.md`: concise design summary from the `.docx`
- `docs/rat-host-prototype.md`: first playable prototype scope
- `docs/project-prep.md`: current preparation and approval plan
- `AGENTS.md`: project rules for Codex and future agent work

## Recommended Milestones

### Milestone 0: Preparation

- Convert the design draft into working Markdown docs.
- Confirm engine, visual direction, prototype boundary, and approval gates.
- Decide Unity version and render pipeline.

### Milestone 1: Unity Project Skeleton

- Create the Unity 3D project only after approval.
- Set up repository hygiene, `.gitignore`, folders, and render pipeline.
- Add a minimal test scene with placeholder camera and lighting.

### Milestone 2: Rat Host Core Loop

- Rat movement in a small sewer map.
- Immune alert gauge.
- Transition to internal minigame.
- Virus movement, white blood cell enemy, mutation fragment collection.
- Mutation choice and return to rat host mode.

### Milestone 3: Prototype Feel Pass

- Pixel-style render pass.
- Camera tuning.
- Basic UI clarity.
- Placeholder animations and feedback.
- Short playtest checklist.

## Immediate Next Steps

1. Review and approve these preparation docs.
2. Approve Unity version and URP direction.
3. Approve first prototype boundary.
4. Write an implementation plan.
5. Create the Unity project only after the plan is approved.
