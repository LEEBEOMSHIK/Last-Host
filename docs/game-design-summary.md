# Game Design Summary

## Identity

- Korean title: `마지막 숙주`
- English title: `The Last Host`
- Genre: pixel-style low-poly 3D evolution survival roguelike
- Core fantasy: a cute personified virus survives by infecting hosts, adapting, and choosing whether to mutate, dominate, transmit, or coexist.

## Core Concept

The player starts as a weak virus. Survival requires moving between hosts, controlling the current host, responding to immune-system pressure, and choosing mutations that shape future play. The final direction is not simply infection or conquest. The game asks whether survival means endless mutation, domination, coexistence, extinction, or acceptance.

## Message

The draft frames humans as vulnerable members of an ecosystem rather than absolute controllers of nature. The strongest survival path is not necessarily domination. Coexistence and adaptation are positioned as meaningful alternatives to endless escalation.

## Visual Direction

The confirmed direction is pixel-style 3D:

- low-poly 3D characters and environments
- pixel-style textures
- low-resolution rendering or post-processing
- cute but strange virus design
- early natural environments that become darker as human society, hospitals, laboratories, and vaccines appear

This is not a pure 2D sprite game.

## Game Structure

The game alternates between two main modes.

### Host-Control Mode

The player controls the currently infected host in a 3D environment. Each host functions as a different playable body with unique movement, access, risks, and transmission opportunities.

Example hosts:

- insect
- mosquito
- rat
- bird
- cat
- dog
- livestock
- human

### Internal Virus Mode

When immune alert reaches a threshold, the player controls the virus inside the host. This mode is a compact top-down or fixed quarter-view arena minigame.

Expected actions:

- avoid white blood cells
- dodge antibody or hazard patterns
- collect mutation fragments
- reach or infect target cells
- survive within a time limit

## Core Loop

1. Control current host.
2. Search for next host opportunities.
3. Meet transmission or survival conditions.
4. Trigger immune-system response.
5. Play internal virus minigame.
6. Earn mutation points or fragments.
7. Choose an upgrade.
8. Return to host control with changed abilities.

## Growth Categories

### Transmission

Expands access to new hosts and environments.

### Mutation

Improves immune-system and vaccine resistance.

### Control

Improves host behavior manipulation and opens domination-oriented routes.

### Coexistence

Reduces host damage and immune pressure, supporting long-term stability and coexistence endings.

## Main Gauges

- Host health
- Immune alert
- Erosion
- Coexistence
- Human risk
- Vaccine development

The first prototype does not need every gauge. It should focus on host health, immune alert, and one or two mutation-related values.

## Stage Progression

The draft proposes progression from natural ecosystems into human-controlled spaces:

1. forest floor
2. pond and grass
3. farm
4. city sewer
5. residential area
6. hospital
7. laboratory

The first prototype starts at the city sewer stage with a rat host because it best tests host control, human-adjacent risk, and internal immune response without requiring the full early-game chain.

## Ending Directions

The draft defines five ending directions:

- supervirus
- domination
- coexistence
- extinction
- acceptance

These endings are long-term design targets and should not be implemented in the first prototype.

## Camera And Controls

- Host-control mode: quarter-view 3D is recommended.
- Internal virus mode: top-down or fixed quarter-view arena is recommended.
- The control scheme should stay consistent between modes where possible, while giving the virus mode a tighter dodge-and-collect feel.

## UI Direction

The full game may show host health, immune alert, erosion, coexistence, mutation slots, host candidates, human risk, and vaccine progress. For the first prototype, UI should be reduced to only what supports the core loop.

Recommended first prototype UI:

- host health
- immune alert
- current mode
- mutation fragments during minigame
- mutation choice screen after success

## Key Open Questions

- Should the first playable build start directly with the rat, or include an insect tutorial first?
- What is the exact success condition for the internal minigame?
- How many mutations should be available in the first prototype?
- How much host control should erosion unlock in the rat prototype?
- Should coexistence be represented mechanically in the first prototype or saved for the next milestone?
