using JorisHoef.Interactions.Core;
using UnityEngine;

namespace JorisHoef.Interactions.BuiltIns
{
    public sealed class RayFromTransform : MonoBehaviour, IRayProvider
    {
        [SerializeField] private Transform _origin;
        public Transform Origin => _origin != null ? _origin : transform;
        public Ray GetRay() => new Ray(Origin.position, Origin.forward);
    }
}