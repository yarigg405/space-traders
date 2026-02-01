using UnityEngine;


namespace Assets.Code.Common.Physics.Registrars
{
    public sealed class SpaceStationRegistrar : MonoBehaviour
    {
        [field: SerializeField] public EcsSphereCollider[] DockingBays { get; private set; }
    }
}