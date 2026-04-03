# Miner's Escape

Miner's Escape is a 3D Unity game for a course (3D Game Development) project, where the player explores cave-like levels, collects minerals, fights goblins, and progresses through multiple levels by gathering minerals.

## Game Overview

The player starts in Level 1 and must:
- explore the cave
- collect minerals by using a pickaxe
- fight goblins using a pistol
- reach the gate to the next level after collecting enough materials

The game contains 3 gameplay levels.

## Main Features

### Player
- third-person style movement 
- mouse-controlled camera
- walk and run animation system
- weapon/tool switching:
  - pickaxe mode
  - pistol mode

### Mining
- the player can mine minerals using the pickaxe
- after enough hits, the mineral is destroyed
- the player receives materials in the inventory

### Shooting
- the player can shoot bullets from the pistol

### Goblins
- goblins spawn in waves
- they navigate using Unity AI Navigation / NavMesh

### UI
- player HP bar
- material counter
- warning message when the player tries to enter the next level without enough materials

### Level 1
- the player starts here
- goblins spawn in waves
- the player must collect enough materials to go to Level 2

### Level 2
- continues the same core gameplay with a different player model
- the player must again collect enough materials to proceed

### Level 3
- final level
- the player must collect all required minerals and kill all goblins
- after completing all requirements, the Game Won scene is loaded

## Controls

### Movement
- `W` / `A` / `S` / `D` — move
- `Left Shift` — run

### Actions
- `1` — equip pickaxe
- `2` — equip pistol
- `Left Mouse Button`
  - dig when pickaxe is equipped
  - shoot when pistol is equipped
