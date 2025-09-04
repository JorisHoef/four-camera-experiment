using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Interactions.Highlighting
{
    [AddComponentMenu("JorisHoef/Interactions/UX/Outline On Focus")]
    public sealed class OutlineOnFocus : MonoBehaviour, IFocusVisual
    {
#region Serialized Fields
        [SerializeField] private Renderer _renderer;
        [SerializeField] private string _keyword = "_Outline";
#endregion

#region Public Methods
        public void ShowFocus(in InteractionContext ctx) => Set(true);
        public void HideFocus(in InteractionContext ctx) => Set(false);
#endregion

#region Private Methods
        private void Set(bool on)
        {
            if (!_renderer)
            {
                _renderer = GetComponentInChildren<Renderer>();
            }
            if (!_renderer)
            {
                return;
            }
            foreach (Material m in _renderer.materials)
            {
                if (on)
                {
                    m.EnableKeyword(_keyword);
                }
                else
                {
                    m.DisableKeyword(_keyword);
                }
            }
        }
#endregion
    }
}