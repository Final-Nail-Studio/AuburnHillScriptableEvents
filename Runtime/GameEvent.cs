/*
 * AuburnHill Scriptable Events
 * 
 * GameEvent.cs
 * 
 * Description:
 * This class defines a **non-generic** ScriptableObject-based event system 
 * that allows Unity objects to communicate without direct references. It is 
 * used for events that **do not carry any data** (i.e., "void" events).
 * 
 * Usage:
 * - Create a new **GameEvent** asset in Unity.
 * - Use `Raise()` to trigger the event.
 * - Subscribe to the event with `Subscribe(callback)`.
 * - Unsubscribe when no longer needed to prevent memory leaks.
 * 
 * Example:
 * ```
 * [SerializeField] private GameEvent onPlayerDeath;
 * void Start() { onPlayerDeath.Subscribe(OnPlayerDied); }
 * void OnPlayerDied() { Debug.Log("Player has died!"); }
 * ```
 * 
 * Features:
 * - Designed for **global event-driven communication** in Unity.
 * - Supports **dynamic subscriptions** and unsubscriptions.
 * - Logs event activity when logging is enabled.
 * 
 * See Also:
 * - `GameEvent<T>` for events that carry data.
 * 
 * Author: AuburnHill
 * License: MIT License
 * Copyright © 2025 AuburnHill. All rights reserved.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AuburnHill.ScriptableEvents
{
    [CreateAssetMenu(menuName = "AuburnHill/Game Event/Basic/Void")]
    public class GameEvent : GameEventBase
    {
        public event Action OnRaised;

        public void Raise()
        {
            if (LoggingEnabled)
            {
                Debug.Log($"[GameEvent] Raised in {name}", this);
            }

            OnRaised?.Invoke();
        }

        public void Subscribe(Action callback)
        {
            if (LoggingEnabled)
            {
                Debug.Log($"[GameEvent] Subscribed: {callback.Method.Name} in {name}", this);
            }

            OnRaised -= callback;
            OnRaised += callback;
        }

        public void Unsubscribe(Action callback)
        {
            if (LoggingEnabled)
            {
                Debug.Log($"[GameEvent] Unsubscribed: {callback.Method.Name} in {name}", this);
            }

            OnRaised -= callback;
        }
    }

    /*
     * AuburnHill Scriptable Events
     * 
     * GameEvent<T>.cs
     * 
     * Description:
     * This class defines a generic ScriptableObject-based event system that allows 
     * Unity objects to communicate without direct references. It enables the raising 
     * of events with specific data types and supports dynamic subscriptions.
     * 
     * Usage:
     * - Create a new GameEvent asset in Unity.
     * - Use `Raise(data)` to trigger the event.
     * - Subscribe to the event with `Subscribe(callback)`.
     * - Unsubscribe when no longer needed to prevent memory leaks.
     * 
     * Example:
     * [SerializeField] private GameEvent<int> scoreEvent;
     * void Start() { scoreEvent.Subscribe(OnScoreChanged); }
     * void OnScoreChanged(int newScore) { Debug.Log("New Score: " + newScore); }
     * 
     * Author: AuburnHill
     * License: MIT License
     * Copyright © 2025 AuburnHill. All rights reserved.
     */

    public abstract class GameEvent<T> : GameEventBase
    {
        /// <summary>
        /// Event triggered when the event is raised.
        /// </summary>
        public event Action<T> OnRaised;

        /// <summary>
        /// Raises the event and notifies all subscribed listeners.
        /// </summary>
        /// <param name="data">The data associated with this event.</param>
        public void Raise(T data)
        {
            if (LoggingEnabled)
            {
                Debug.Log($"[GameEvent<{typeof(T).Name}>] Raised with data: {data} in {name}", this);
            }

            OnRaised?.Invoke(data);
        }

        /// <summary>
        /// Subscribes a listener to this event.
        /// </summary>
        /// <param name="callback">The function to invoke when the event is raised.</param>
        public void Subscribe(Action<T> callback)
        {
            if (LoggingEnabled)
            {
                Debug.Log($"[GameEvent<{typeof(T).Name}>] Subscribed: {callback.Method.Name} in {name}", this);
            }

            OnRaised -= callback;
            OnRaised += callback;
        }

        /// <summary>
        /// Unsubscribes a listener from this event.
        /// </summary>
        /// <param name="callback">The function to remove from event invocation.</param>
        public void Unsubscribe(Action<T> callback)
        {
            if (LoggingEnabled)
            {
                Debug.Log($"[GameEvent<{typeof(T).Name}>] Unsubscribed: {callback.Method.Name} in {name}", this);
            }

            OnRaised -= callback;
        }
    }
}