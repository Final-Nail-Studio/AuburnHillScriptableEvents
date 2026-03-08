# How to Use AuburnHill Scriptable Events

Version: 2.0.1
Author: AuburnHill
License: MIT

## Overview

AuburnHill Scriptable Events is a lightweight event system for Unity that enables decoupled communication using ScriptableObject-based events.

At its core, the package provides `GameEvent` / `GameEvent<T>` assets that can be raised from anywhere and listened to by `MonoBehaviour`s—without direct references, static singletons, or fragile wiring.

The system is split into two cooperating packages:

* AuburnHill Scriptable Events (Engine Package)
  Core event types + runtime support + editor tooling. Safe to use in isolation.
* AuburnHill Scriptable Events – Game Package (Optional)
  Adds a registry, bootstrapper, setup tooling, and editor automation hooks for larger projects.

You can use just the engine package, or both together for a more structured workflow.

## Key Benefits

* Loosely coupled communication (no direct references between senders and listeners)
* Editor-friendly (events are assets you can inspect and assign)
* Strongly typed events (`GameEvent<T>` supports custom payload types)
* Scales from small to large projects (optional registry + automation)

## Packages and Architecture

### 1. Engine Package (Required)

Namespace examples:

```
AuburnHill.ScriptableEvents
```

Contains:

* `GameEvent` and `GameEvent<T>`
* Runtime listener base(s) (e.g., `ListenerMonoBehaviour`)
* Custom inspector support
* Game Event Creator editor tooling
* Custom `GameEvent<T>` script generator popup

This package has no opinionated game structure and can be used in any Unity project.

### 2. Game Package (Optional, recommended for larger projects)

Namespace examples:

```
AuburnHill.ScriptableEvents.Game
AuburnHill.ScriptableEvents.Game.Editor
```

Adds:

* `GameEventsRegistry` (project-owned ScriptableObject type + script file)
* `EventsBootstrapper` (scene root component holding a registry reference)
* Setup tooling (`Tools → AuburnHill → Setup Game Events`)
* Registry wiring pipeline invoked by engine tooling via reflection:

  * inserts fields into `GameEventsRegistry.cs` within a marker region
  * assigns created event assets into the registry asset after compile/domain reload

The engine package does not depend on the game package, but will invoke these hooks when the game package is present.

## Installation and Initial Setup

### Option A: Engine Package Only

1. Import AuburnHill Scriptable Events (Engine).
2. Create event assets manually or via the Game Event Creator.
3. Reference event assets directly in your scripts.

Good for prototypes and small projects where manual wiring is preferred.

### Option B: Engine + Game Package

1. Import both packages.
2. Run:

```
Tools → AuburnHill → Setup Game Events
```

Setup will (as needed):

* generate the project scripts from templates (registry, bus, bootstrapper, listener base)
* create or locate a `GameEventsRegistry` asset
* create or locate a single root-level `EventsBootstrapper` in the active scene
* assign the registry asset into the bootstrapper

This enables centralized discovery plus editor automation for event creation.

## Creating Game Events

### 1. Using the Game Event Creator Window

Open via:

```
Tools → AuburnHill → Game Event Creator
```

Workflow:

* Configure project settings (Unity-relative paths under `Assets/` only):

  * Event Assets Folder (where created event assets are saved)
  * Registry Script (path to `GameEventsRegistry.cs`)
  * Registry Asset (the `GameEventsRegistry` ScriptableObject instance)
  * Event Asset Prefix (defaults to `Evt`)
* Create events in the Create New Event area:

  * enter a name
  * choose a type (defaults to the void event type)
  * Create makes the event immediately (disabled while queue is non-empty)
  * Queue adds it to a queue for batch creation
* Batch create:

  * add multiple events to the queue
  * press Create Events to create them all

Naming rules:

* User input such as `Open Inventory` or `open_inventory` is normalized to `OpenInventory`
* Asset name becomes `Prefix + BaseName` (e.g., `EvtOpenInventory.asset`)
* Registry field name strips the prefix (e.g., `OpenInventory`)
* Existing asset name collisions are rejected (error and skip)

### 2. Creating a Custom `GameEvent<T>` Type

Use the Create Custom Event button in the creator window.

This opens a popup that:

* lets you choose any valid data type `T`
* generates a new `GameEvent<T>` script with a `CreateAssetMenu` entry
* does not create an asset

After compilation, the new type becomes available in the creator window’s Type dropdown.

### 3. Predefined Event Types

The engine package may include a set of convenience concrete event types (e.g., `GameEventInt`, `GameEventVector3`, etc.), depending on what is shipped with the engine package.

Even when using predefined types, you can always create custom event types when the payload type or naming needs are project-specific.

## Core Runtime API

### `GameEvent<T>`

A generic ScriptableObject event that broadcasts a payload of type `T`.

Typical usage:

```csharp
[SerializeField] private GameEvent<int> _scoreChanged;

private void AddScore(int amount)
{
    _scoreChanged.Raise(amount);
}
```

### `ListenerMonoBehaviour`

A base class that manages subscription/unsubscription across `OnEnable` / `OnDisable`.

Example:

```csharp
public sealed class ScoreUI : ListenerMonoBehaviour
{
    [SerializeField] private GameEvent<int> _scoreChanged;

    public override void ManageListeners(ManageListenersMode mode)
    {
        ManageEvent(_scoreChanged, OnScoreChanged, mode);
    }

    private void OnScoreChanged(int score)
    {
        Debug.Log("Score updated: " + score);
    }
}
```

### `ManageListenersMode`

```csharp
public enum ManageListenersMode
{
    Subscribe,
    Unsubscribe
}
```

## Editor Support

### GameEvent Inspector

When selecting a GameEvent asset:

* a test “Raise” control is available
* supported types can be raised with sample data
* useful for fast iteration and debugging

## Game Package Automation (Optional)

When the game package is present and configured:

* Creating an event asset via the creator tool can:

  * insert a strongly-typed field into `GameEventsRegistry.cs` inside the marker region
  * trigger compilation
  * assign the created asset into the registry asset after domain reload

If the game package is not present, event creation still works; the registry wiring steps are simply skipped.

## FAQ

Do I need the game package?
No. The engine package works on its own.

Why two packages?
To keep the engine package reusable and lightweight while allowing an opinionated project workflow via the optional game package.

Why ScriptableObject events instead of C# events?
They are inspectable, serializable, asset-based, and naturally decouple systems.

## Conclusion

AuburnHill Scriptable Events provides a clean, extensible foundation for event-driven architecture in Unity.

* Use the engine package for simple decoupling.
* Add the game package when centralized organization and editor automation become valuable.
* Author events quickly using the creator tooling, and scale up to registry-based workflows when needed.

---
