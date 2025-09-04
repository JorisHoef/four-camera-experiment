namespace JorisHoef.Interactions.Prompts
{
    public interface IPromptProvider
    {
#region Public Properties
        bool HasPrompt { get; }
        string Prompt { get; }
#endregion
    }
}