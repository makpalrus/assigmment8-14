# SKYBOUND

SKYBOUND is a single-player 2D mobile platformer built with Unity 6.

Jump between platforms, collect every coin and reach the finish flag to unlock the next level.

## Features

- Three levels loaded from LevelData assets with different backgrounds and platform layouts.
- Six collectible coins in level one and ten in levels two and three.
- Finish flags that unlock after collecting all coins.
- Three hearts, spike damage and temporary invulnerability.
- Main menu, pause, restart and victory screens.
- Mobile joystick and a round Jump button.
- Music and sound effects.
- JSON progress, completed three-level run times and previous-run comparison, unlocked levels and volume settings.
- Play opens the three-level route with icons and progress reset with confirmation. Each run starts at level one.
- A level stopwatch counts active play time and stops on pause.
- ScriptableObject assets for player, enemy and item balance.
- UI adapted for different screen sizes and safe areas.

## Controls

| Action | Mobile | Keyboard |
|---|---|---|
| Move | On-screen joystick | A/D or arrow keys |
| Jump | Jump button | Space |
| Pause | Pause button | Escape |

## Run the project

1. Open the project in Unity 6000.3.0f1.
2. Open `Assets/Scenes/MainMenu.unity`.
3. Press the Unity Play button, then PLAY in the game and start level one. Complete levels one, two and three to save a run result.

Scenes in build order:
1. MainMenu
2. Game

Game uses LevelLoader to instantiate the layout and enemies from Level_01, Level_02 or Level_03 in Assets/Resources/Data. Change balance in these assets. The save file is stored in Application.persistentDataPath.

## Platform

Target: Android, landscape orientation.

The interface has been tested in Unity Device Simulator on a 16:9 phone, a phone with a notch and a 4:3 tablet. Testing on a physical Android device remains planned.

## Assets and audio

Visual assets come from the supplied Simple 2D Platformer BE2 pack. The interface uses Unity uGUI and built-in graphics.

Music and sound effects are procedurally synthesized WAV files created with Codex assistance.

## Development status

Implemented systems include three data-driven levels, enemy state machines, coin collection, finish gates, health, mobile controls, menus, audio and safe JSON saves.

Unity checks cover level loading, live balance, coin and gate events, progress unlocks and save recovery. Full app restart on a physical phone remains to be verified.
