using System.Collections;
using JorisHoef.Interactions.Core;
using JorisHoef.Interactions.UX;
using UnityEngine;

namespace JorisHoef.Interactions.Gameplay
{
    [AddComponentMenu("JorisHoef/Interactions/Gameplay/Door Slide Interactable")]
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

        [SerializeField] private bool _startOpen;
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!GetComponent<Collider>() && GetComponentInChildren<Collider>() == null)
            {
                Debug.LogWarning($"[{nameof(DoorSlideInteractable)}] No Collider found on this object or its children. "
                                +
                                 "Interactor raycasts need a collider to detect this interactable. " +
                                 "Add a collider here/underneath, or mount this via a ColliderInteractableAdapter on a collider GameObject.",
                                 this);
            }
        }
#endif
#endregion

#region Public Methods
        public bool CanInteract(in InteractionContext ctx) => !_isMoving;

        public InteractionResult Interact(in InteractionContext ctx)
        {
            if (_isMoving)
            {
                return InteractionResult.FailureFrom(this, Texts.Get("door.busy", "Busy."));
            }

            Vector3 from = _door.localPosition;
            Vector3 to = _isOpen ? _closedLocalPos : OpenLocalPos;

            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
            }
            _moveRoutine = StartCoroutine(AnimateTo(from, to));

            _isOpen = !_isOpen;
            string msgKey = _isOpen ? "door.opened" : "door.closed";
            return InteractionResult.SuccessFrom(this, Texts.Get(msgKey, _isOpen ? "Opened." : "Closed."));
        }

        public string GetCannotInteractReason(in InteractionContext ctx) =>
                _isMoving ? Texts.Get("door.busy", "Busy.") : null;

        public void OnFocusGained(in InteractionContext ctx) { }
        public void OnFocusLost(in InteractionContext ctx) { }
#endregion

#region Private Methods
        private IEnumerator AnimateTo(Vector3 from, Vector3 to)
        {
            _isMoving = true;
            float t = 0f, dur = Mathf.Max(0.0001f, _duration);
            while (t < dur)
            {
                t += Time.deltaTime;
                float k = _curve.Evaluate(Mathf.Clamp01(t / dur));
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