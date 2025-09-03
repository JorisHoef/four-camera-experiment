using UnityEngine;

namespace JorisHoef.Interactions.Core
{
    public readonly struct InteractionContext
    {
        public GameObject InteractorGO { get; }
        public RaycastHit Hit { get; }
        public float Time { get; }

        public InteractionContext(GameObject interactorGO, in RaycastHit hit, float time)
        {
            InteractorGO = interactorGO;
            Hit = hit;
            Time = time;
        }
    }
}