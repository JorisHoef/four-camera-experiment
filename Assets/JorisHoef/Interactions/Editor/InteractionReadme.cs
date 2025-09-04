using UnityEngine;

namespace JorisHoef.Interactions.Editor
{
    [CreateAssetMenu(menuName = "JorisHoef/Interactions/README", fileName = "Interactions_README")]
    public sealed class InteractionsReadme : ScriptableObject
    {
#region Serialized Fields
        [TextArea(5, 20)]
        [SerializeField]
        private string _overview =
                "JorisHoef Interactions\n\n" +
                "• Add an Interactor to your player (auto setup)\n" +
                "• Add Interaction HUD (TMP) for prompts/toasts\n" +
                "• Add interactables: components (e.g., DoorSlideInteractable) or SO assets via ColliderInteractableAdapter\n";

        [Header("Quick Defaults")]
        [SerializeField] private float _defaultRayDistance = 3f;

        [SerializeField] private LayerMask _defaultMask = ~0;
#endregion

#region Public Properties
        public string Overview
        {
            get => _overview;
            set => _overview = value;
        }

        public float DefaultRayDistance
        {
            get => _defaultRayDistance;
            set => _defaultRayDistance = value;
        }

        public LayerMask DefaultMask
        {
            get => _defaultMask;
            set => _defaultMask = value;
        }
#endregion
    }
}