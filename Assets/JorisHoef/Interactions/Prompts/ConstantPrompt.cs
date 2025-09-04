using UnityEngine;

namespace JorisHoef.Interactions.Prompts
{
    [AddComponentMenu("JorisHoef/Interactions/UX/Prompt (Constant)")]
    public sealed class ConstantPrompt : MonoBehaviour, IPromptProvider
    {
#region Serialized Fields
        [SerializeField] private bool _hasPrompt = true;
        [SerializeField] private string _prompt = "Use";
#endregion

#region Public Properties
        public bool HasPrompt => _hasPrompt;
        public string Prompt => _prompt;
#endregion
    }
}