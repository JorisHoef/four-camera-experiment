using JorisHoef.Interactions.UX;
using UnityEngine;

namespace JorisHoef.Interactions.Prompts
{
    [AddComponentMenu("JorisHoef/Interactions/UX/Prompt (Text Key)")]
    public sealed class PromptFromKey : MonoBehaviour, IPromptProvider
    {
#region Serialized Fields
        [SerializeField] private string _key = "prompt.use";
#endregion

#region Public Properties
        public bool HasPrompt => true;
        public string Prompt => Texts.Get(_key, "Use");
#endregion
    }
}