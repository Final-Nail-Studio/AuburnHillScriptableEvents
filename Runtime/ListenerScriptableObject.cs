/*
 * AuburnHill Scriptable Events
 * 
 * ListenerScriptableObject.cs
 * 
 * Description:
 * This abstract base class provides a structured way to manage event subscriptions 
 * for ScriptableObjects. It automates event management by subscribing and unsubscribing
 * listeners when the asset is enabled or disabled, ensuring proper event handling 
 * across multiple game sessions.
 * 
 * Features:
 * - Automates event subscription on OnEnable() and unsubscription on OnDisable().
 * - Provides helper methods `ManageEvent<T>()` and `ManageEvent()` to simplify event handling.
 * - Designed for use with the AuburnHill Scriptable Events system.
 * 
 * Usage:
 * - Inherit from `ListenerScriptableObject` in any ScriptableObject that listens for game events.
 * - Override `ManageListeners(ManageListenersMode mode)` to define event handling.
 * - Use `ManageEvent<T>()` inside `ManageListeners()` to handle specific events.
 * 
 * Example:
 * ```
 * public class PlayerDataListener : ListenerScriptableObject
 * {
 *     [SerializeField] private GameEvent<PlayerData> _playerDataUpdated;
 *  
 *     public override void ManageListeners(ManageListenersMode mode)
 *     {
 *         ManageEvent(_playerDataUpdated, OnPlayerDataUpdated, mode);
 *     }
 *  
 *     private void OnPlayerDataUpdated(PlayerData data)
 *     {
 *         Debug.Log($"Player data updated: {data}");
 *     }
 * }
 * ```
 * 
 * Author: AuburnHill
 * License: MIT License
 * Copyright 2025 AuburnHill. All rights reserved.
 */


using System;
using UnityEngine;

namespace AuburnHill.ScriptableEvents
{
    public abstract class ListenerScriptableObject : ScriptableObject
    {
        protected virtual void OnEnable()
        {
            ManageListeners(ManageListenersMode.Subscribe);
        }

        protected virtual void OnDisable()
        {
            ManageListeners(ManageListenersMode.Unsubscribe);
        }

        protected abstract void ManageListeners(ManageListenersMode mode);

        /// <summary>
        /// Manages subscriptions for generic GameEvent<T>.
        /// </summary>
        protected void ManageEvent<T>(GameEvent<T> gameEvent, Action<T> callback, ManageListenersMode mode)
        {
            if (gameEvent == null)
            {
                return;
            }

            if (mode == ManageListenersMode.Subscribe)
            {
                if (gameEvent.LoggingEnabled)
                {
                    Debug.Log($"{GetType().Name} is subscribing to {gameEvent.name}", this);
                }
                gameEvent.Subscribe(callback);
            }
            else
            {
                if (gameEvent.LoggingEnabled)
                {
                    Debug.Log($"{GetType().Name} is unsubscribing from {gameEvent.name}", this);
                }
                gameEvent.Unsubscribe(callback);
            }
        }

        /// <summary>
        /// Manages subscriptions for non-generic GameEvent (void event).
        /// </summary>
        protected void ManageEvent(GameEvent gameEvent, Action callback, ManageListenersMode mode)
        {
            if (gameEvent == null)
            {
                return;
            }

            if (mode == ManageListenersMode.Subscribe)
            {
                if (gameEvent.LoggingEnabled)
                {
                    Debug.Log($"{GetType().Name} is subscribing to {gameEvent.name}", this);
                }
                gameEvent.Subscribe(callback);
            }
            else
            {
                if (gameEvent.LoggingEnabled)
                {
                    Debug.Log($"{GetType().Name} is unsubscribing from {gameEvent.name}", this);
                }
                gameEvent.Unsubscribe(callback);
            }
        }
    }
}