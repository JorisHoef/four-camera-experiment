using EditorAttributes;
using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Interactions.UnityBridge
{
    /// <summary>
    ///     Adapter that bridges a <see cref="Collider" /> in the scene to any <see cref="IInteractable" />.
    ///     <para>
    ///         Use this when your interactable is not directly a MonoBehaviour on the same GameObject
    ///         as the collider. Common cases:
    ///     </para>
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 Linking a <see cref="ScriptableObject" /> interactable (e.g. <c>SOMessageInteractable</c>)
    ///                 to a collider in the scene.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Forwarding collider hits to an interactable component elsewhere in the hierarchy.
    ///             </description>
    ///         </item>
    ///     </list>
    ///     <para>
    ///         Not needed if the interactable is already a component implementing
    ///         <see cref="IInteractable" /> on the same GameObject as the collider; in that case, the resolver
    ///         can find it directly.
    ///     </para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("JorisHoef/Interactions/Unity Bridge/Collider Interactable Adapter")]
    public sealed class ColliderInteractableAdapter : MonoBehaviour
    {
#region Serialized Fields
        [SerializeField]
        [Tooltip("Drag a component/scriptable that implements IInteractable, or a GameObject that has one.")]
        [TypeFilter(typeof(IInteractable))]
        private Object _reference;
#endregion

#region Public Properties
        public IInteractable Reference => TryResolve(_reference, out IInteractable i) ? i : null;
#endregion

#region Unity Methods
        private void OnValidate()
        {
            if (_reference != null && !TryResolve(_reference, out _))
            {
                Debug.LogError($"[{nameof(ColliderInteractableAdapter)}] Assigned object '{_reference.name}' does not expose an {nameof(IInteractable)} on itself or the GameObject. "
                              +
                               $"Drag the specific component/asset that implements {nameof(IInteractable)} or a GameObject that has one.",
                               this);
            }
        }
#endregion

#region Private Methods
        private static bool TryResolve(Object obj, out IInteractable interactable)
        {
            interactable = null;
            if (obj == null)
            {
                return false;
            }

            if (obj is IInteractable asI)
            {
                interactable = asI;
                return true;
            }

            if (obj is Component comp)
            {
                if (comp.TryGetComponent(out IInteractable fromComp))
                {
                    interactable = fromComp;
                    return true;
                }
                return false;
            }

            if (obj is GameObject go)
            {
                if (go.TryGetComponent(out IInteractable fromGo))
                {
                    interactable = fromGo;
                    return true;
                }
                return false;
            }

            return false;
        }
#endregion
    }
}