using JorisHoef.Interactions.Core;

namespace JorisHoef.Interactions
{
    public interface IFocusVisual
    {
#region Public Methods
        void ShowFocus(in InteractionContext ctx);
        void HideFocus(in InteractionContext ctx);
#endregion
    }
}