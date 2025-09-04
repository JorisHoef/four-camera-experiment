namespace JorisHoef.Interactions.UX
{
    public interface ITextProvider
    {
#region Public Methods
        bool TryGet(string key, out string value);
#endregion
    }
}