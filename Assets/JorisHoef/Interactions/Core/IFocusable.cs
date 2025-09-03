namespace JorisHoef.Interactions.Core
{
    public interface IFocusable
    {
        void OnFocusGained(in InteractionContext ctx);
        void OnFocusLost(in InteractionContext ctx);
    }
}