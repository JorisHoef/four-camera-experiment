using System;
using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.Interactions.UX
{
    [CreateAssetMenu(menuName = "JorisHoef/Interactions/UX/Text Table", fileName = "Interactions_Texts")]
    public sealed class TextTable : ScriptableObject, ITextProvider
    {
#region Constants and Fields
        private Dictionary<string, string> _map;
#endregion

#region Serialized Fields
        [SerializeField] private List<Entry> _entries = new List<Entry>();
#endregion

#region Unity Methods
        private void OnEnable()
        {
            _map = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (Entry e in _entries)
            {
                if (!string.IsNullOrEmpty(e.Key))
                {
                    _map[e.Key] = e.Value ?? string.Empty;
                }
            }
        }
#endregion

#region Public Methods
        public bool TryGet(string key, out string value)
        {
            if (_map != null && _map.TryGetValue(key, out value))
            {
                return true;
            }

            value = null; // ensure assigned
            return false;
        }
#endregion

#region Nested Types
        [Serializable]
        private struct Entry
        {
#region Serialized Fields
            public string _key;
            [TextArea] public string _value;
#endregion

#region Public Properties
            public string Key
            {
                get => _key;
                set => _key = value;
            }

            public string Value
            {
                get => _value;
                set => _value = value;
            }
#endregion
        }
#endregion
    }
}