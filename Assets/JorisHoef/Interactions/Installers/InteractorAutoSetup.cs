using System;
using System.Reflection;
using JorisHoef.Interactions.BuiltIns;
using JorisHoef.Interactions.Runtime;
using JorisHoef.Interactions.UnityBridge;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JorisHoef.Interactions.Installers
{
    /// <summary>
    ///     Ensures an Interactor is fully wired with default providers on this GameObject:
    ///     - RayFromTransform (origin set to Camera.main if unset)
    ///     - KeyboardInteractInput
    ///     - DefaultInteractableResolver
    ///     Add this ONE component and you’re done. It wires on Reset/OnValidate in Editor and on Awake at runtime.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Interactor))]
    [RequireComponent(typeof(RayFromTransform))]
    [RequireComponent(typeof(KeyboardInteractInput))]
    [RequireComponent(typeof(DefaultInteractableResolver))]
    [AddComponentMenu("JorisHoef/Interactions/Installers/Interactor (Auto Setup)")]
    [DefaultExecutionOrder(InteractionConstants.AUTO_SETUP)]
    public sealed class InteractorAutoSetup : MonoBehaviour
    {
#region Constants and Fields
        private FieldInfo _rayField;
        private FieldInfo _inputField;
        private FieldInfo _resolverField;
        private FieldInfo _rayOriginField;
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
            _rayField ??= t.GetField("_rayProviderObj", BindingFlags.Instance | BindingFlags.NonPublic);
            _inputField ??= t.GetField("_inputObj", BindingFlags.Instance | BindingFlags.NonPublic);
            _resolverField ??= t.GetField("_resolverObj", BindingFlags.Instance | BindingFlags.NonPublic);

            _rayOriginField ??= typeof(RayFromTransform)
                   .GetField("_origin", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private void Wire()
        {
            GameObject go = gameObject;

            Interactor interactor = go.GetComponent<Interactor>();
            RayFromTransform ray = go.GetComponent<RayFromTransform>();
            KeyboardInteractInput input = go.GetComponent<KeyboardInteractInput>();
            DefaultInteractableResolver resolver = go.GetComponent<DefaultInteractableResolver>();

            if (ray != null && _rayOriginField != null && _rayOriginField.GetValue(ray) == null && Camera.main)
            {
                _rayOriginField.SetValue(ray, Camera.main.transform);
#if UNITY_EDITOR
                EditorUtility.SetDirty(ray);
#endif
            }

#if UNITY_EDITOR
            SerializedObject so = new SerializedObject(interactor);
            so.Update();

            SerializedProperty spRay = so.FindProperty("_rayProviderObj");
            SerializedProperty spInput = so.FindProperty("_inputObj");
            SerializedProperty spResolver = so.FindProperty("_resolverObj");

            if (spRay.objectReferenceValue == null)
            {
                spRay.objectReferenceValue = ray;
            }
            if (spInput.objectReferenceValue == null)
            {
                spInput.objectReferenceValue = input;
            }
            if (spResolver.objectReferenceValue == null)
            {
                spResolver.objectReferenceValue = resolver;
            }

            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(interactor);
#else
            // Runtime fallback: reflection (non-persistent, but fine in builds)
            if (_rayField != null && _rayField.GetValue(interactor) == null)         _rayField.SetValue(interactor, ray);
            if (_inputField != null && _inputField.GetValue(interactor) == null)     _inputField.SetValue(interactor, input);
            if (_resolverField != null && _resolverField.GetValue(interactor) == null) _resolverField.SetValue(interactor, resolver);
#endif
        }
#endregion
    }
}