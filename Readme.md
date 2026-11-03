# IdleFarm3D

A small 3D idle/tycoon game built in Unity. Click sections to complete tasks, earn gold, and spend it to unlock new sections and grow your farm.

## Gameplay

1. Click an unlocked section to start a task.
2. When the task finishes, you earn gold.
3. Spend gold to unlock new sections, which pay out more.
4. Progress is saved when you quit or pause the game.

## Tech

- Unity 6000.3.18f1, URP, 3D
- Unity Input System
- Data saved as JSON (`Application.persistentDataPath/save.json`)

## Architecture

Game logic is independent from the UI. UI scripts only listen to events and display state, so the UI could be removed without breaking the game.

| Script | Responsibility |
|---|---|
| `GoldManager` | Owns the gold amount. `AddGold`, `TrySpendGold`, raises `OnGoldChanged` |
| `WorkTask` | A timed task with a reward. Raises started / progress / completed events |
| `TaskManager` | Single central update loop that ticks only the running tasks |
| `Section` | Locked/unlocked state, unlock cost, routes clicks (unlock or start task) |
| `ClickInputHandler` | Raycasts from the camera on click and forwards it to the `Section` |
| `SaveManager` | Loads on startup, saves on quit/pause (gold + unlocked section IDs) |
| `GameLogger` | Single entry point for all logs, can be enabled/disabled |
| `GoldUI`, `SectionUI` | Event-driven UI, no per-frame polling |

### Event flow

```
Click -> ClickInputHandler -> Section.OnClicked
   locked   -> TryUnlock -> GoldManager.TrySpendGold -> OnUnlocked -> SectionUI
   unlocked -> WorkTask.TryStart -> TaskManager.Tick -> Complete -> GoldManager.AddGold -> OnGoldChanged -> GoldUI
```

### Design decisions

- **Central task ticking:** `WorkTask` has no `Update`. `TaskManager` ticks only active tasks, so the cost stays flat as the number of tasks grows.
- **Save order:** `SaveManager` runs first (`DefaultExecutionOrder(-100)`) so other scripts can read save data in `Awake`.
- **Simple saving:** Saves on quit/pause only. Autosave may be added later if needed.
- **Running tasks are not saved:** an in-progress task resets on load. Offline progress is planned.

## Balance (current values)

| Section | Unlock cost | Reward | Duration |
|---|---|---|---|
| Section 1 | 0 (unlocked by default) | 10 | 3 s |
| Section 2 | 50 | 30 | 4 s |
| Section 3 | 200 | 100 | 5 s |

## Debug tools

`DebugGoldInput` (editor only): `Space` adds gold, `T` tests spending, `R` resets progress.

## Roadmap

- [x] Phase 1: core loop, sections, UI, save system
- [ ] Phase 2: upgrades, workers (automation), offline earnings, more sections
- [ ] Phase 3: art pass, effects, sound, menus