#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace JorisHoef.Interactions.Editor
{
    [CustomEditor(typeof(InteractionsReadme))]
    public sealed class InteractionsReadmeEditor : UnityEditor.Editor
    {
#region Public Methods
        public override void OnInspectorGUI()
        {
            InteractionsReadme rd = (InteractionsReadme)target;

            // Title
            EditorGUILayout.LabelField("JorisHoef Interactions — README", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            // Overview
            EditorGUILayout.HelpBox(rd.Overview, MessageType.Info);

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("Quick Actions", EditorStyles.boldLabel);

            // Buttons
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Add Interactor (Auto Setup)"))
                {
                    InteractionsQuickActions.CreateInteractorAuto(new MenuCommand(null));
                }
                if (GUILayout.Button("Add Interaction HUD (TMP)"))
                {
                    InteractionsQuickActions.CreateHUD(new MenuCommand(null));
                }
            }

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("Tips", EditorStyles.boldLabel);
            EditorGUILayout
                   .LabelField("• Put interactables on objects with colliders, or use a ColliderInteractableAdapter.");
            EditorGUILayout
                   .LabelField("• For ScriptableObject-based interactables, drag the asset into the Adapter's Reference.");
            EditorGUILayout
                   .LabelField("• Prompt shows what will happen; InteractionResult message shows what happened.");

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("Defaults", EditorStyles.boldLabel);
            rd.DefaultRayDistance = EditorGUILayout.FloatField("Ray Distance", rd.DefaultRayDistance);
            rd.DefaultMask = EditorGUILayout.LayerField("Layer (for docs only)", rd.DefaultMask.value);
        }
#endregion
    }
}
#endif