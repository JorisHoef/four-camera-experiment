using UnityEngine;

namespace JorisHoef.Interactions.UnityBridge
{
    internal static class ComponentLookupExtensions
    {
        public static bool TryGetComponentInParent<T>(this Component c, out T value) where T : class
        {
            value = c.GetComponentInParent<T>();
            return value != null;
        }
    }
}