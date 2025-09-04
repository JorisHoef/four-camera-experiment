using UnityEngine;

namespace JorisHoef.Interactions.Prompts
{
    [AddComponentMenu("JorisHoef/Interactions/UX/No Prompt")]
    public sealed class NoPrompt : MonoBehaviour, IPromptProvider
    {
#region Public Properties
        public bool HasPrompt => false;
        public string Prompt => string.Empty;
#endregion
    }
}