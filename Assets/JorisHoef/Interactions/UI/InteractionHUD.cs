using System.Collections;
using JorisHoef.Interactions.Core;
using JorisHoef.Interactions.Prompts;
using JorisHoef.Interactions.Runtime;
using TMPro;
using UnityEngine;

namespace JorisHoef.Interactions.UI
{
    [AddComponentMenu("JorisHoef/Interactions/UI/Interaction HUD")]
    public sealed class InteractionHUD : MonoBehaviour
    {
#region Constants and Fields
        private Coroutine _toastRoutine;
#endregion

#region Serialized Fields
        [Header("References")]
        [SerializeField] private Interactor _interactor;

        [SerializeField] private TMP_Text _promptLabel;
        [SerializeField] private TMP_Text _toastLabel;
        [SerializeField] private float _toastDuration = 2f;
#endregion

#region Unity Methods
        private void Start()
        {
            _promptLabel.text = string.Empty;
            _toastLabel.text = string.Empty;
        }

        private void OnEnable()
        {
            _interactor.InteractionBlocked += OnBlocked;
            _interactor.InteractionCompleted += OnCompleted;
            _interactor.FocusChanged += OnFocusChanged;
        }

        private void OnDisable()
        {
            _interactor.InteractionBlocked -= OnBlocked;
            _interactor.InteractionCompleted -= OnCompleted;
            _interactor.FocusChanged -= OnFocusChanged;
        }
#endregion

#region Private Methods
        private void ShowToast(string message)
        {
            _toastLabel.text = message;

            if (_toastRoutine != null)
            {
                StopCoroutine(_toastRoutine);
            }

            _toastRoutine = StartCoroutine(ClearToastAfterDelay());
        }

        private IEnumerator ClearToastAfterDelay()
        {
            yield return new WaitForSeconds(_toastDuration);

            _toastLabel.text = string.Empty;
            _toastRoutine = null;
        }
#endregion

#region Event Handlers
        private void OnFocusChanged(IInteractable prev, IInteractable next)
        {
            if (next == null || !_interactor.TryGetFocusedGameObject(out GameObject go))
            {
                _promptLabel.text = string.Empty;
                return;
            }

            IPromptProvider provider = go.GetComponentInParent<IPromptProvider>();
            if (provider != null && provider.HasPrompt && !string.IsNullOrEmpty(provider.Prompt))
            {
                _promptLabel.text = $"[E] {provider.Prompt}";
            }
            else
            {
                _promptLabel.text = string.Empty;
            }
        }

        private void OnBlocked(IInteractable target, string reason)
        {
            ShowToast(reason);
        }

        private void OnCompleted(IInteractable target, InteractionResult result)
        {
            if (!string.IsNullOrEmpty(result.Message))
            {
                ShowToast(result.Message);
            }
        }
#endregion
    }
}