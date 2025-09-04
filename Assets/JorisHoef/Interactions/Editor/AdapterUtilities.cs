#if UNITY_EDITOR

using JorisHoef.Interactions.UnityBridge;
using UnityEditor;
using UnityEngine;

namespace JorisHoef.Interactions.Editor
{
    public static class AdapterUtilities
    {
#region Public Methods
        [MenuItem("GameObject/JorisHoef/Make Interactable (Adapter + Collider)", false, 30)]
        public static void MakeInteractable(MenuCommand cmd)
        {
            GameObject go = cmd.context as GameObject ?? Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogWarning("Select a GameObject first.");
                return;
            }

            Collider col = go.GetComponent<Collider>() ?? go.AddComponent<BoxCollider>();
            ColliderInteractableAdapter adapter = go.GetComponent<ColliderInteractableAdapter>()
                                               ?? go.AddComponent<ColliderInteractableAdapter>();
            Selection.activeGameObject = go;
            Debug.Log("Added ColliderInteractableAdapter. Assign your IInteractable in the Reference field.", adapter);
        }
#endregion
    }
}
#endif