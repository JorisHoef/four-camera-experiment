using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Interactions.Gameplay
{
    [CreateAssetMenu(menuName = "JorisHoef/Interactions/Message", fileName = "SOMessageInteractable")]
    public sealed class SOMessageInteractable : ScriptableObject, IInteractable
    {
#region Constants and Fields
        [TextArea] [SerializeField] private string _message = "Hello!";
#endregion

#region Public Methods
        public bool CanInteract(in InteractionContext ctx) => true;

        public InteractionResult Interact(in InteractionContext ctx) => InteractionResult.SuccessFrom(this, _message);
#endregion
    }
}