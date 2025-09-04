namespace JorisHoef.Interactions.Core
{
    public interface IInteractable
    {
#region Public Methods
        public bool CanInteract(in InteractionContext ctx);
        public InteractionResult Interact(in InteractionContext ctx);
#endregion
    }
}