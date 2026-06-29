# Rat Host Prototype

## Goal

The `Rat Host Prototype` is the first playable validation target for `The Last Host`. It should prove whether the core loop works before the project expands into more hosts, stages, endings, and human-world systems.

## Prototype Question

Is it fun and understandable to:

1. control a host,
2. feel immune pressure rising,
3. switch into an internal virus minigame,
4. earn a mutation,
5. return to host control with a visible change?

## Included Scope

- Small sewer map
- Rat host
- Basic rat movement
- Simple hazards or blocked paths
- Immune alert gauge
- Mode transition at 100% immune alert
- Internal virus arena
- Virus movement
- White blood cell enemy type
- Mutation fragment collection
- Success/failure result
- Mutation choice screen with three options
- Rat mode resumes after mutation selection

## Excluded Scope

- Full story campaign
- Insect tutorial
- Multiple host chain
- Human host gameplay
- Hospital and laboratory systems
- Vaccine development
- Final endings
- Full roguelike run structure
- Permanent progression
- Finished art assets

## Core Loop

1. Spawn as virus-controlled rat in the sewer.
2. Explore a compact map.
3. Immune alert rises over time or through risky actions.
4. At 100% immune alert, switch to internal virus mode.
5. Survive and collect mutation fragments.
6. On success, show three mutation choices.
7. Apply selected mutation.
8. Return to rat control with a changed stat or ability.

## Rat Host Mode

### Purpose

Rat mode validates whether host control can stand as the main exploration layer.

### Minimum Mechanics

- Move
- Turn
- Interact with simple points of interest
- Trigger immune alert over time
- Show host health and immune alert

### Possible Prototype Hazards

- toxic puddle
- human light cone
- blocked pipe
- noisy surface

Only one or two hazards are needed for the first validation build.

## Internal Virus Mode

### Purpose

Virus mode validates the tension break: the player leaves the host body and fights or survives inside the immune system.

### Minimum Mechanics

- Move virus avatar
- Avoid white blood cells
- Collect mutation fragments
- Survive until the target count or timer condition is met

### Recommended Initial Success Condition

Collect a fixed number of mutation fragments before virus stability reaches zero.

## Mutation Choices

The first prototype should use three choices from the draft:

| Mutation | Category | Prototype Effect |
| --- | --- | --- |
| 잠복 강화 | Coexistence / mutation | immune alert rises slower |
| 신경 조종 | Control | rat movement or interaction improves |
| 포유류 적응 | Transmission | prepares future host expansion; in prototype, improves reward or objective access |

The third option needs a temporary prototype effect if no second host exists yet.

## Minimal Gauges

- Host health
- Immune alert
- Virus stability or virus health
- Mutation fragments

Optional for later:

- erosion
- coexistence
- human risk
- vaccine development

## Prototype Acceptance Criteria

The prototype is successful if a tester can understand the loop without explanation:

- why immune alert is rising
- why the game switches to virus mode
- what to do in the minigame
- what mutation was gained
- how the mutation changed rat-mode play

## Main Risks

- The mode switch may feel like an interruption instead of tension.
- Rat movement may feel too generic without host-specific behavior.
- Mutation choices may feel abstract if the first prototype has only one host.
- Pixel-style 3D may require render tuning before it reads as intentional rather than low fidelity.

## Approval Needed Before Implementation

1. Confirm rat-only first prototype or include insect tutorial.
2. Confirm URP.
3. Confirm Unity version.
4. Confirm first mutation effects.
5. Confirm initial control scheme.
6. Confirm whether placeholder assets are acceptable for all prototype visuals.
