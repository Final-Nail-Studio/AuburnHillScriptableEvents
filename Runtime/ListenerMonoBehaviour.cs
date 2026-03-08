/*
 * AuburnHill Scriptable Events
 * 
 * ListenerMonoBehaviour.cs
 * 
 * Description:
 * This abstract base class provides a structured way to manage event subscriptions 
 * for MonoBehaviours. It automatically subscribes and unsubscribes listeners when 
 * the GameObject is enabled or disabled, preventing memory leaks and ensuring 
 * event handlers are properly managed.
 * 
 * Features:
 * - Automates event subscription in `OnEnable()` and unsubscription in `OnDisable()`.
 * - Provides helper methods `ManageEvent<T>()` and `ManageEvent()` to simplify event handling.
 * - Designed for use with the AuburnHill Scriptable Events system.
 * 
 * Usage:
 * - Inherit from `ListenerMonoBehaviour` in any MonoBehaviour that listens for game events.
 * - Override `ManageListeners(ManageListenersMode mode)` to define event handling.
 * - Use `ManageEvent<T>()` inside `ManageListeners()` to handle specific events.
 * 
 * Example:
 * ```
 * public class ExampleListener : ListenerMonoBehaviour
 * {
 *     [SerializeField] private GameEvent<string> _messageEvent;
 *  
 *     public override void ManageListeners(ManageListenersMode mode)
 *     {
 *         ManageEvent(_messageEvent, OnMessageReceived, mode);
 *     }
 *  
 *     private void OnMessageReceived(string message)
 *     {
 *         Debug.Log($"Received message: {message}");
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
    public abstract class ListenerMonoBehaviour : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            ManageListeners(ManageListenersMode.Subscribe);
        }

        protected virtual void OnDisable()
        {
            ManageListeners(ManageListenersMode.Unsubscribe);
        }

        public abstract void ManageListeners(ManageListenersMode mode);

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


