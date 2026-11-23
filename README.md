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

### Sound Pass (Assignment 12)

`Assets/Resources/Audio/GameMix.mixer` routes Master → Music → Tracks, Master → SFX and Master → UI. Normal uses 0 dB / 22000 Hz on Tracks. Paused uses -6 dB / 800 Hz, with a 0.3 second transition in unscaled time. Master, Music and SFX sliders use `20 * Log10(Max(value, 0.0001))` and save to the Assignment 11 JSON settings.

GameAudio persists across scene loads. Two looping sources crossfade menu.wav and level.wav over one second. All three levels use the same level track, so Retry and level transitions keep the music playing. Effects use PlayOneShot with pitch variation from 0.92 to 1.08. SfxPlayer selects one of three damage clips without immediate repeats. Music and UI remain audible on pause, gameplay effects pause.

| File | Event | Source | License |
|---|---|---|---|
| menu.wav | Menu music | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| level.wav | Gameplay music | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| jump.wav | Jump | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| coin.wav | Coin pickup | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| hurt.wav | Damage | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| hurt_2.wav, hurt_3.wav | Alternative damage clips | Variations of original hurt.wav, Documentation/generate-audio.cjs | Original project audio, no third-party samples or license requirements |
| click.wav | UI click | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| unlock.wav | All coins collected | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| win.wav | Victory / level finish | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| lose.wav | Defeat | Original procedural synthesis for Skybound with Codex assistance | Original project audio, no third-party samples or license requirements |
| land.wav | Landing | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |
| heal.wav | Heart pickup | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |
| death.wav | Player death | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |
| patrolwarning.wav | Patroller wind-up | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |
| shooterwarning.wav | Shooter wind-up | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |
| chargerwarning.wav | Charger wind-up | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |
| strike.wav | Patroller strike | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |
| shoot.wav | Shooter projectile | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |
| dash.wav | Charger dash | Documentation/generate-audio.cjs, original synthesis | Original project audio, no third-party samples or license requirements |

These sounds are generated from oscillators and deterministic noise, not downloaded or copied from recordings. No external attribution is required. SFX are mono / ADPCM / Decompress On Load. Music uses Vorbis / Streaming. Listen on the target phone before submission. Phone restart proof and the student's English voice recording must be captured on the actual device.

## Development status

Lecture alignment: `Documentation/Lecture-project-audit.txt` lists matched techniques, corrected differences and the parts that are adaptations rather than literal lecture code. The sources contain partial examples, not a complete Skybound project. Best score now counts points. Level selection uses saved unlocks. Enemy debug labels use 3D TextMesh Pro and are hidden in release builds.

Implemented systems include three data-driven levels, enemy state machines, coin collection, finish gates, health, mobile controls, menus, audio and safe JSON saves.

Unity checks cover level loading, live balance, coin and gate events, progress unlocks and save recovery. Full app restart on a physical phone remains to be verified.
