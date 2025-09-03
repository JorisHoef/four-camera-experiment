using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Interactions.Logic
{
    public static class InteractionLogic
    {
#region Public Methods
        public static bool TryRaycastResolve(in Ray ray,
                                             float maxDistance,
                                             LayerMask mask,
                                             IInteractableResolver resolver,
                                             out RaycastHit hit,
                                             out IInteractable interactable)
        {
            if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
            {
                Collider col = hit.collider;
                if (col != null && resolver.TryResolve(col, out interactable))
                {
                    return true;
                }
            }
            interactable = null;
            hit = default;
            return false;
        }

        public static bool FocusChanged(IInteractable a, IInteractable b) => !ReferenceEquals(a, b);
#endregion
    }
}