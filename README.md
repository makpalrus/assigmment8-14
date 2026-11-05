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
- JSON progress, fastest complete-run time, unlocked levels and volume settings.
- Play opens the three-level route with icons and progress reset with confirmation. Saved unlocked levels can be selected.
- One stopwatch counts active play time across levels 1, 2 and 3, and stops on pause. BEST SCORE shows the lowest complete-run time; direct starts at levels 2 or 3 are practice and cannot set a record.
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

Game uses LevelLoader to instantiate the layout and prefab waves from Level_01, Level_02 or Level_03 in Assets/Resources/Data. Change balance in these assets. The save file is stored in Application.persistentDataPath.

UI objects and their Button/Slider events are saved in MainMenu and the three LevelLayouts prefabs. AudioManager.prefab contains four AudioSources with assigned mixer groups, MusicManager, PauseAudio and SfxPlayer. SceneMusic stores each scene's track. LevelLoader has three Inspector-assigned LevelData references; GameUI stores the layout's player, camera and patrol points, and handles panels, coins, hearts and the timer. MainMenuController includes level selection and reset confirmation. Small GameTimer, LevelBindings, ButtonSound and LevelSelectUI helper scripts have been merged. Audio events use direct AudioClip fields and PlayJump/PlayCoin methods; projectiles use Rigidbody2D and trigger collisions. See Documentation/Simple-project.txt for a lecture-by-lecture explanation. No runtime UI builder or Resources.Load remains in Assets/Scripts. See Documentation/Inspector-setup.txt.

## Platform

Target: Android, landscape orientation.

The interface has been tested in Unity Device Simulator on a 16:9 phone, a phone with a notch and a 4:3 tablet. Testing on a physical Android device remains planned.

## Assets and audio

Visual assets come from the supplied Simple 2D Platformer BE2 pack. The interface uses Unity uGUI and built-in graphics.

Music and sound effects are procedurally synthesized WAV files created with Codex assistance.

## Mobile interface

- Main menu: press Play to choose a level.
- HUD: shows hearts and collected coins.
- Controls: use the joystick to move and the Jump button to jump.
- Pause menu: Resume, Restart and Menu.
- The interface uses Canvas Scaler, anchors and Safe Area.