using UnityEngine;
using UnityEngine.InputSystem;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    public sealed class InputReferencesContainer : MonoBehaviour
    {
        [field: SerializeField] public InputActionReference DoubleClick { get; private set; }
    }
}
