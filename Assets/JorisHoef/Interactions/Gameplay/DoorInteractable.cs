using System.Collections;
using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Gameplay
{
    /// <summary>
    ///     Simple sliding door: toggles between closed and open by moving the target transform
    ///     along a chosen local axis by a fixed distance over a given duration, using an AnimationCurve.
    ///     No physics; purely kinematic.
    /// </summary>
    public sealed class DoorSlideInteractable : MonoBehaviour, IInteractable, IFocusable, ICanInteractReason
    {
#region Constants and Fields
        private Vector3 _closedLocalPos;
        private bool _isOpen;
        private bool _isMoving;
        private Coroutine _moveRoutine;
#endregion

#region Serialized Fields
        [Header("Target")]
        [SerializeField] private Transform _door;

        [Header("Motion")]
        [SerializeField] private Vector3 _localAxis = Vector3.up;

        [SerializeField] private float _distance = 2f;
        [SerializeField] private float _duration = 0.6f;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("UX")]
        [Tooltip("What the user will see in a screenspace UI like \"Press {Button}\" to Open Door\"")]
        [SerializeField]
        private string _prompt = "Open/Close";

        [SerializeField] private bool _startOpen;
#endregion

#region Public Properties
        public string Prompt => _prompt;
#endregion

#region Private Properties
        private Vector3 OpenLocalPos => _closedLocalPos
                                      + Vector3.Scale(_localAxis.normalized,
                                                      new Vector3(_distance, _distance, _distance));
#endregion

#region Unity Methods
        private void Reset()
        {
            _door = transform;
        }

        private void Awake()
        {
            if (_door == null)
            {
                _door = transform;
            }

            _closedLocalPos = _door.localPosition;

            if (_startOpen)
            {
                _door.localPosition = OpenLocalPos;
                _isOpen = true;
            }
        }
#endregion

#region Public Methods
        public bool CanInteract(in InteractionContext ctx) => !_isMoving;

        public InteractionResult Interact(in InteractionContext ctx)
        {
            if (_isMoving)
            {
                return InteractionResult.FailureFrom(this, "Door is moving.");
            }

            Vector3 from = _door.localPosition;
            Vector3 to = _isOpen ? _closedLocalPos : OpenLocalPos;

            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
            }
            _moveRoutine = StartCoroutine(AnimateTo(from, to));

            _isOpen = !_isOpen;
            return InteractionResult.SuccessFrom(this, _isOpen ? "Door opened." : "Door closed.");
        }

        public string GetCannotInteractReason(in InteractionContext ctx) => _isMoving ? "Door is moving." : null;

        public void OnFocusGained(in InteractionContext ctx)
        {
            /* optional highlight */
        }

        public void OnFocusLost(in InteractionContext ctx)
        {
            /* optional unhighlight */
        }
#endregion

#region Private Methods
        private IEnumerator AnimateTo(Vector3 from, Vector3 to)
        {
            _isMoving = true;
            float t = 0f;
            float dur = Mathf.Max(0.0001f, _duration);
            while (t < dur)
            {
                t += Time.deltaTime;
                float u = Mathf.Clamp01(t / dur);
                float k = _curve.Evaluate(u);
                _door.localPosition = Vector3.LerpUnclamped(from, to, k);
                yield return null;
            }
            _door.localPosition = to;
            _isMoving = false;
            _moveRoutine = null;
        }
#endregion
    }
}