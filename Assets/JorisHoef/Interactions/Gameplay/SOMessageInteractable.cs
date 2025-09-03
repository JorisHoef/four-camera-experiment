using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Gameplay
{
    [CreateAssetMenu(menuName = "Interactables/Message")]
    public sealed class SOMessageInteractable : ScriptableObject, IInteractable
    {
#region Constants and Fields
        [SerializeField] private string _prompt = "Read";
        [TextArea] [SerializeField] private string _message = "Hello!";
#endregion

#region Public Properties
        public string Prompt => _prompt;
#endregion

#region Public Methods
        public bool CanInteract(in InteractionContext ctx) => true;

        public InteractionResult Interact(in InteractionContext ctx) => InteractionResult.SuccessFrom(this, _message);
#endregion
    }
}