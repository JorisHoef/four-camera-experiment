using UnityEngine;

namespace JorisHoef.Interactions.Prompts
{
    [AddComponentMenu("JorisHoef/Interactions/UX/Custom Prompt")]
    public sealed class CustomPrompt : MonoBehaviour, IPromptProvider
    {
#region Serialized Fields
        [SerializeField] private string _prompt = "Hack Terminal";
#endregion

#region Public Properties
        public bool HasPrompt => true;
        public string Prompt => _prompt;
#endregion
    }
}