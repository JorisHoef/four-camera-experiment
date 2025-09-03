using JorisHoef.Interactions.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JorisHoef.Interactions.BuiltIns
{
    [AddComponentMenu("JorisHoef/Interactions/BuiltIns/Keyboard Interact Input")]
    public sealed class KeyboardInteractInput : MonoBehaviour, IInteractInput
    {
#region Serialized Fields
        [SerializeField] private Key _primary = Key.E;
        [SerializeField] private Key _secondary = Key.Space;
#endregion

#region Public Methods
        public bool TriggerPressedThisFrame()
        {
            Keyboard kb = Keyboard.current;
            if (kb == null)
            {
                return false;
            }

            return kb[_primary].wasPressedThisFrame || kb[_secondary].wasPressedThisFrame;
        }
#endregion
    }
}