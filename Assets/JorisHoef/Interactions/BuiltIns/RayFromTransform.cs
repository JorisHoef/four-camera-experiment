using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Interactions.BuiltIns
{
    [AddComponentMenu("JorisHoef/Interactions/BuiltIns/Ray From Transform")]
    public sealed class RayFromTransform : MonoBehaviour, IRayProvider
    {
#region Serialized Fields
        [Tooltip("Ray Origin Transform, will be this if none assigned")]
        [SerializeField] private Transform _origin;
#endregion

#region Public Properties
        public Transform Origin => _origin != null ? _origin : transform;
#endregion

#region Public Methods
        public Ray GetRay() => new Ray(Origin.position, Origin.forward);
#endregion
    }
}