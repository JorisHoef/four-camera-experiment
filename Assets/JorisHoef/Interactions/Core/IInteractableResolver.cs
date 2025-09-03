using UnityEngine;

namespace JorisHoef.Interactions.Core
{
    public interface IInteractableResolver
    {
        bool TryResolve(Collider collider, out IInteractable interactable);
    }
}