#if UNITY_EDITOR

using System.Reflection;
using JorisHoef.Interactions.Installers;
using JorisHoef.Interactions.Runtime;
using JorisHoef.Interactions.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.Interactions.Editor
{
    public static class InteractionsQuickActions
    {
#region Public Methods
        [MenuItem("GameObject/JorisHoef/Interactor (Auto Setup)", false, 10)]
        public static void CreateInteractorAuto(MenuCommand cmd)
        {
            GameObject go = new GameObject("Interactor_AutoSetup");
            GameObjectUtility.SetParentAndAlign(go, cmd.context as GameObject);

            InteractorAutoSetup auto = go.AddComponent<InteractorAutoSetup>();

            EditorUtility.SetDirty(auto);
            Selection.activeGameObject = go;
        }

        [MenuItem("GameObject/JorisHoef/Interaction HUD (TMP)", false, 11)]
        public static void CreateHUD(MenuCommand cmd)
        {
            // Canvas
            GameObject canvasGO = new GameObject("InteractionHUD_Canvas");
            GameObjectUtility.SetParentAndAlign(canvasGO, cmd.context as GameObject);
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Prompt
            GameObject promptGO = new GameObject("Prompt");
            promptGO.transform.SetParent(canvasGO.transform, false);
            TextMeshProUGUI promptTMP = promptGO.AddComponent<TextMeshProUGUI>();
            promptTMP.text = "";
            promptTMP.fontSize = 28;
            promptTMP.alignment = TextAlignmentOptions.BottomLeft;
            promptTMP.rectTransform.anchorMin = new Vector2(0, 0);
            promptTMP.rectTransform.anchorMax = new Vector2(0, 0);
            promptTMP.rectTransform.pivot = new Vector2(0, 0);
            promptTMP.rectTransform.anchoredPosition = new Vector2(24, 24);

            // Toast
            GameObject toastGO = new GameObject("Toast");
            toastGO.transform.SetParent(canvasGO.transform, false);
            TextMeshProUGUI toastTMP = toastGO.AddComponent<TextMeshProUGUI>();
            toastTMP.text = "";
            toastTMP.fontSize = 24;
            toastTMP.alignment = TextAlignmentOptions.BottomJustified;
            toastTMP.rectTransform.anchorMin = new Vector2(0.5f, 0.15f);
            toastTMP.rectTransform.anchorMax = new Vector2(0.5f, 0.15f);
            toastTMP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            toastTMP.rectTransform.anchoredPosition = Vector2.zero;

            // HUD
            InteractionHUD hud = canvasGO.AddComponent<InteractionHUD>();
            hud.GetType().GetField("_promptLabel", BindingFlags.NonPublic | BindingFlags.Instance)
              ?.SetValue(hud, promptTMP);
            hud.GetType().GetField("_toastLabel", BindingFlags.NonPublic | BindingFlags.Instance)
              ?.SetValue(hud, toastTMP);

            Interactor interactor = Object.FindFirstObjectByType<Interactor>();
            if (interactor != null)
            {
                hud.GetType().GetField("_interactor", BindingFlags.NonPublic | BindingFlags.Instance)
                  ?.SetValue(hud, interactor);
            }
            else
            {
                Debug.LogWarning("[Interaction HUD] No Interactor found in scene; assign one to the HUD.", hud);
            }

            Selection.activeGameObject = canvasGO;
        }
#endregion
    }
}
#endif