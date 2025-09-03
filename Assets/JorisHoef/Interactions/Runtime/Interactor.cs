using System;
using EditorAttributes;
using JorisHoef.Interactions.Core;
using JorisHoef.Interactions.Logic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JorisHoef.Interactions.Runtime
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-50)]
    [AddComponentMenu("JorisHoef/Interactions/Runtime/Interactor")]
    public sealed class Interactor : MonoBehaviour
    {
#region Constants and Fields
        private RaycastHit _lastHit;
#endregion

#region Serialized Fields
        [Header("Providers (Composition)")]
        [SerializeField]
        [TypeFilter(typeof(IRayProvider))]
        private Object _rayProviderObj;

        [SerializeField]
        [TypeFilter(typeof(IInteractInput))]
        private Object _inputObj;

        [SerializeField]
        [TypeFilter(typeof(IInteractableResolver))]
        private Object _resolverObj;

        [Header("Raycast")]
        [SerializeField] private float _maxDistance = 3f;

        [SerializeField] private LayerMask _mask = ~0;

        [Header("Logging")]
        [SerializeField] private bool _logBlocked = true;

        [SerializeField] private bool _logFailures = true;
        [SerializeField] private bool _logSuccessMessages = true;
#endregion

#region Public Events and Delegates
        /// <summary>Fires when focus changes (old, new).</summary>
        public event Action<IInteractable, IInteractable> FocusChanged;

        /// <summary>Fires after Interact() returns.</summary>
        public event Action<IInteractable, InteractionResult> InteractionCompleted;

        /// <summary>Fires when input pressed but CanInteract() returned false.</summary>
        public event Action<IInteractable, string> InteractionBlocked;
#endregion

#region Public Properties
        public IInteractable Current { get; private set; }

        /// <summary>
        ///     What the user will see in a screenspace UI like "Press {Button}" to Open Door"
        /// </summary>
        public string CurrentPrompt => Current?.Prompt;
#endregion

#region Private Properties
        private IRayProvider RayProvider => (IRayProvider)_rayProviderObj;
        private IInteractInput InputTrigger => (IInteractInput)_inputObj;
        private IInteractableResolver Resolver => (IInteractableResolver)_resolverObj;
#endregion

#region Unity Methods
        private void Awake()
        {
            if (RayProvider == null)
            {
                throw new MissingComponentException("Interactor: RayProvider missing.");
            }
            if (InputTrigger == null)
            {
                throw new MissingComponentException("Interactor: Input missing.");
            }
            if (Resolver == null)
            {
                throw new MissingComponentException("Interactor: Resolver missing.");
            }
        }

        private void Update()
        {
            Ray ray = RayProvider.GetRay();

            bool found = InteractionLogic.TryRaycastResolve(ray, _maxDistance, _mask, Resolver, out RaycastHit hit,
                                                            out IInteractable interactable);

            IInteractable next = found ? interactable : null;
            if (InteractionLogic.FocusChanged(Current, next))
            {
                InteractionContext lostCtx = new InteractionContext(gameObject, in _lastHit, Time.time);
                if (Current is IFocusable lost)
                {
                    lost.OnFocusLost(in lostCtx);
                }

                IInteractable prev = Current;
                Current = next;
                _lastHit = hit;

                InteractionContext gainCtx = new InteractionContext(gameObject, in _lastHit, Time.time);
                if (Current is IFocusable gain)
                {
                    gain.OnFocusGained(in gainCtx);
                }

                FocusChanged?.Invoke(prev, Current);
            }

            if (!InputTrigger.TriggerPressedThisFrame() || Current == null)
            {
                return;
            }

            InteractionContext ctx = new InteractionContext(gameObject, in _lastHit, Time.time);

            if (BlockedPath(ctx))
            {
                return;
            }

            Execute(ctx);
        }
#endregion

#region Private Methods
        /// <summary>
        ///     Finds and returns which object gave us the interactionContext
        /// </summary>
        /// <param name="target"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private static Object GetBestContextObject(IInteractable target, in InteractionContext ctx)
        {
            if (target is Object uo)
            {
                return uo;
            }

            GameObject hitGO = ctx.Hit.collider ? ctx.Hit.collider.gameObject : null;
            if (hitGO)
            {
                return hitGO;
            }

            return ctx.InteractorGO;
        }

        private void Execute(InteractionContext ctx)
        {
            InteractionResult result = Current.Interact(in ctx);

            InteractionCompleted?.Invoke(Current, result);

            Object contextObj = result.ContextObject ?? GetBestContextObject(Current, in ctx) ?? this;
            string origin = string.IsNullOrEmpty(result.OriginTypeName)
                                    ? Current.GetType().Name
                                    : result.OriginTypeName;
            string stackText = string.IsNullOrEmpty(result.OriginStack) ? "" : $"\n{result.OriginStack}";

            if (!result.Success)
            {
                if (_logFailures)
                {
                    Debug.LogWarning($"[Interactor] Interaction FAILED on {origin}: {result.Message}{stackText}",
                                     contextObj);
                }
            }
            else if (_logSuccessMessages && !string.IsNullOrEmpty(result.Message))
            {
                Debug.Log($"[Interactor] {result.Message} (from {origin}){stackText}", contextObj);
            }
        }

        private bool BlockedPath(InteractionContext ctx)
        {
            if (!Current.CanInteract(in ctx))
            {
                string reason = (Current as ICanInteractReason)?.GetCannotInteractReason(in ctx)
                             ?? "Blocked by CanInteract == false.";
                InteractionBlocked?.Invoke(Current, reason);

                if (_logBlocked)
                {
                    Object contextObj = GetBestContextObject(Current, in ctx) ?? this;
                    Debug.LogWarning($"[Interactor] Cannot interact with {Current.GetType().Name}: {reason}",
                                     contextObj);
                }
                return true;
            }
            return false;
        }
#endregion
    }
}