namespace JorisHoef.Interactions.Core
{
    /// <summary>
    ///     Optional. Implement on an IInteractable if you can explain why CanInteract() is false.
    /// </summary>
    public interface ICanInteractReason
    {
#region Public Methods
        public string GetCannotInteractReason(in InteractionContext ctx);
#endregion
    }
}