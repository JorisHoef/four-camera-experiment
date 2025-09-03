using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Interactions.UnityBridge
{
    [AddComponentMenu("JorisHoef/Interactions/Unity Bridge/Default Interactable Resolver")]
    public sealed class DefaultInteractableResolver : MonoBehaviour, IInteractableResolver
    {
#region Public Methods
        public bool TryResolve(Collider collider, out IInteractable interactable)
        {
            if (collider.TryGetComponentInParent(out ColliderInteractableAdapter adapter))
            {
                IInteractable refI = adapter.Reference;
                if (refI != null)
                {
                    interactable = refI;
                    return true;
                }
            }

            if (collider.TryGetComponentInParent(out IInteractable found))
            {
                interactable = found;
                return true;
            }

            interactable = null;
            return false;
        }
#endregion
    }
}