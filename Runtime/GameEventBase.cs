// FILE: Assets/AuburnHillScriptableEventsEngine/Runtime/GameEventBase.cs
using System.Collections.Generic;
using UnityEngine;

namespace AuburnHill.ScriptableEvents
{
    public abstract class GameEventBase : ScriptableObject
    {
        [Header("Metadata")]
        [SerializeField] private string _description = "";

        // Tags are simple, user-authored strings. No leading # required.
        [SerializeField] private List<string> _tags = new List<string>();

        [Header("Debug")]
        [SerializeField] private bool _enableLogging = false;

        public string Description => _description;
        public IReadOnlyList<string> Tags => _tags;
        public bool LoggingEnabled => _enableLogging;

#if UNITY_EDITOR
        // Editor uses this to mutate tags safely without exposing the list publicly.
        internal List<string> GetTagsListForEditor()
        {
            if (_tags == null)
            {
                _tags = new List<string>();
            }

            return _tags;
        }
#endif
    }
}