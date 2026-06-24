# space-traders

Unity3D space-trader simulator. Authoritative client–server, both halves in one Unity project, ECS via **Entitas + Jenny/custom codegen**.

## Layout
- `src/space-traders/` — main Unity project. `src/space-traders_clone_0/` — ParrelSync clone (shares assets; runs a second editor instance for host+client testing).
- All gameplay code lives under `src/space-traders/Assets/Code/` in a **single assembly** (`Game.asmdef`) — no asmdef layering, any type can reference any other.
  - `ClientPart/` — views, camera, client gameplay features, client networking, UI elements.
  - `ServerPart/` — authoritative ECS worlds, server networking, physics, world filling.
  - `Common/` — shared components, SQLite DB, serialization, static data, time.
  - `Networking/` — shared net data types + `NetworkManager`, `ErrorCodes`.
  - `Infrastructure/` — DI (VContainer), state machine, installers, scene loading, entry points.
  - `Generated/` — Entitas/codegen output. **Never edit by hand.**
  - `UI/` — window/screen system (`UIManager`, layers, screens).

## Conventions (hard rules)
- **No comments in code.** Names and structure carry meaning.
- **All log/exception messages I write are in English.** Don't dump OS-localized exceptions raw; log a clean English line for handled cases.
- **Error codes**: kebab-case with `error-` prefix (`error-connection`), value doubles as a localization key. Constant name stays PascalCase. See `Networking/ErrorCodes.cs`.

## Entitas + codegen (important gotcha)
- Components are plain classes in `Common/Components/*.cs`, tagged `[Game]` / `[Input]` / `[Meta]`.
- The **custom codegen emits VALUE accessors**: for `GlobalPosition { double2 Value }`:
  - `entity.GlobalPosition` → `double2` (the value, NOT the component)
  - `entity.globalPosition` → the component instance
  - `entity.hasGlobalPosition`, `entity.AddGlobalPosition(v)`, `entity.ReplaceGlobalPosition(v)`
  - empty/flag components → `entity.isStation`, `entity.isPlanet`
- `ISerializeComponent` marks a component as synced server→client in entity snapshots.
- Matchers: `GameMatcher.AllOf/AnyOf/NoneOf(...)`. Groups via `context.GetGroup(matcher)`.

## DI (VContainer)
- `BootstrapInstaller` = **root scope** (singletons: networking, state machine, ECS contexts, scene loader, holders that must survive scene changes).
- `GameSceneInstaller` / `UiInstaller` = per-scene `MonoInstaller`s under `GameLifetimeScope`/`SceneLifetimeScope`.
- Scene MonoBehaviours get injected because their root is in the scope's `autoInjectGameObjects` list — they use `[Inject]` fields or a `[Inject] Construct(...)` method, and are **not** registered in DI.
- Services registered `.AsImplementedInterfaces()` and implementing `IInitializable/ITickable/IDisposable` run as entry points for that scope.

## Client / Server
- One project runs both. Server = authoritative ECS simulation; worlds keyed by **star-system identity** (`StarSystemRepository` name). Client builds views from snapshots only.
- Net transport = **Riptide**. Static `[MessageHandler]` receivers (`ClientMessengerReceiver` / `ServerMessengerReceiver`) forward to routers, wired by `*DependencySetupper` (`IInitializable`).
- `NetworkManager` owns Riptide `Server`/`Client`; they are **recreated on every `Cleanup()`** (fresh peers per session — avoids half-broken state). Server lives in a child DI scope built in `ServerStartup`.
- Request/response correlation via `NetworkRequestSystem` (leading `messageId` uint, stripped before the awaited response is returned).

## Scenes & star systems
- Star-system DB `Name` (pretty, e.g. `Sol`) is **decoupled** from the Unity scene to load (`SceneName`, e.g. `GameScene1`). On enter/undock the server sends `{ SceneName, ConfigJson }`; the client loads the scene and `SpaceSceneConfigApplier` applies the JSON config. Multiple systems can share one Unity scene.
- `SceneNames` lists Unity scene constants.

## SQLite DB
- `DataBaseManager` creates the DB **once, only if the file is missing**, at `Application.persistentDataPath/DATA_BASE.db`. ParrelSync clone uses the same path → same DB.
- To reseed (schema/data changes in `DataBaseInstaller`), **delete `DATA_BASE.db` + `-wal` + `-shm`**, then run. This wipes characters.
- PRAGMAs that return a row (e.g. `journal_mode=WAL`) must use `ExecuteScalar`, not `Execute`.
- ORM types in `Common/DataBase/ORM/`.

## Floating origin / coordinates
- World position is `double2` (`GlobalPosition` for real-space entities like ships/stations; `SkyboxCoordinates` for skybox objects like planets/suns).
- Unity position = `globalPos − playerQuadrant * QUADRANT_SIZE`, mapped to `(x, 0, y)` (see `UpdateLocalPositionSystem`). `GameConstants` holds quadrant size and `DISTANCE_UI_TO_REAL` / `DISTANCE_REAL_TO_UI`.

## UI
- **Windows / modals** (menu, station, popups): go through `UIManager` (`GoToScreen` / `OpenModal`), as `UIScreenView` prefab (in `Resources/UI/Screens/`) + `IPresenter<TView>`, registered in `UiInstaller`. Navigable, have history.
- **HUD panels** (always-on in the game scene): single **scene-placed MonoBehaviour** with `[Inject]`, NOT routed through `UIManager`. Scene load/unload is their lifecycle. Examples: `PlayerShipControlView`, `SelectedObjectControlView`, `OverviewPanel`, `NavigationIconsOverlay`.

## Navigation feature (current)
- `SelectionService` — the single shared "selected object" (event-driven).
- `NavigationRegistry` — live set of navigable objects (`AnyOf(Station, Planet)`); add new navigable types here in one place.
- `NavigationTargetExtensions` — coordinate / label / distance helpers.
- Flow: space click (`MouseClickDetector`→router) / overview row / world icon → `SelectionService` → `SelectedObjectControlView` → Warp to the selected object's coordinate.

## Running
- Open `src/space-traders` in Unity. Use ParrelSync (`Assets/ParrelSync`) to launch a second editor instance for host + client. No automated test setup observed — verify in-editor.
