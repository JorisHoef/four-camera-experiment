using System;
using System.Reflection;
using JorisHoef.Interactions.BuiltIns;
using JorisHoef.Interactions.Runtime;
using JorisHoef.Interactions.UnityBridge;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JorisHoef.Interactions.Installers
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Interactor))]
    [RequireComponent(typeof(RayFromTransform))]
    [RequireComponent(typeof(KeyboardInteractInput))]
    [RequireComponent(typeof(DefaultInteractableResolver))]
    [AddComponentMenu("JorisHoef/Interactions/Installers/Interactor (Auto Setup)")]
    public sealed class InteractorAutoSetup : MonoBehaviour
    {
#region Constants and Fields
        private FieldInfo _rayField;
        private FieldInfo _inputField;
        private FieldInfo _resolverField;
#endregion

#region Unity Methods
        private void Awake()
        {
            CacheFields();
            Wire();
        }

        private void Reset()
        {
            CacheFields();
            Wire();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            CacheFields();
            Wire();
#endif
        }
#endregion

#region Private Methods
        private void CacheFields()
        {
            Type t = typeof(Interactor);
            _rayField = _rayField ?? t.GetField("_rayProviderObj", BindingFlags.Instance | BindingFlags.NonPublic);
            _inputField = _inputField ?? t.GetField("_inputObj", BindingFlags.Instance | BindingFlags.NonPublic);
            _resolverField =
                    _resolverField ?? t.GetField("_resolverObj", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private void Wire()
        {
            GameObject go = gameObject;
            Interactor interactor = go.GetComponent<Interactor>();
            Object ray = go.GetComponent<RayFromTransform>();
            Object input = go.GetComponent<KeyboardInteractInput>();
            Object resolver = go.GetComponent<DefaultInteractableResolver>();

            if (_rayField != null)
            {
                _rayField.SetValue(interactor, ray);
            }
            if (_inputField != null)
            {
                _inputField.SetValue(interactor, input);
            }
            if (_resolverField != null)
            {
                _resolverField.SetValue(interactor, resolver);
            }
        }
#endregion
    }
}