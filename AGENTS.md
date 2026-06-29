# AGENTS.md

## Project

- Project name: `마지막 숙주 / The Last Host`
- Engine: Unity 3D
- Primary platform target: PC first, with later mobile portability considered only after prototype validation.
- Current phase: project preparation and design organization. Do not implement gameplay, scaffold a Unity project, or create code without explicit user approval.

## Communication

- Use Korean by default for user-facing explanations and project documents.
- Keep updates concise and practical.
- When a decision is required, ask for approval before changing project direction.

## Source Of Truth

- Original draft: `C:\project\game\last_host\last_host_game_plan.docx`
- Working documentation lives under `docs/`.
- If the `.docx` and Markdown docs conflict, treat the `.docx` as the source draft and the Markdown files as the current working interpretation until the user approves otherwise.

## Confirmed Direction

- The game is not a pure 2D sprite game.
- Visual direction is pixel-style 3D:
  - low-poly 3D models
  - pixel-style textures
  - low-resolution rendering or post-processing
  - quarter-view or top-down camera composition
- Core structure:
  - host-control 3D exploration
  - internal virus immune-system minigame
  - mutation choice and growth
- First validation target: `Rat Host Prototype`.

## Rat Host Prototype Boundary

The first playable prototype should validate only the smallest complete loop:

1. Control a rat host in a small sewer map.
2. Raise immune alert over time or by risky actions.
3. Enter an internal virus minigame at 100% immune alert.
4. Control the virus, avoid white blood cells, collect mutation fragments.
5. Choose one mutation reward.
6. Return to rat host play with a visible gameplay change.

Do not add the full host chain, story campaign, endings, vaccine systems, or human-stage content unless the user approves a broader milestone.

## Unity Direction

- Use Unity 3D.
- URP is the recommended render pipeline, but final setup still needs explicit approval before project creation.
- Prefer simple placeholder assets during prototype planning.
- Keep future implementation modular:
  - host controller
  - immune alert system
  - mode transition controller
  - virus minigame controller
  - mutation selection system
  - shared game state

## Approval Gates

Ask the user before:

- creating or modifying a Unity project
- adding packages
- generating art assets
- changing the prototype scope
- committing to a render pipeline or Unity version
- creating gameplay code
- replacing or rewriting approved docs

## Git And Files

- Do not revert user changes unless explicitly asked.
- Avoid unrelated refactors or metadata churn.
- Keep documentation changes focused and readable.
- Generated IDE files such as `.idea/` are not part of project design unless the user asks to manage them.
