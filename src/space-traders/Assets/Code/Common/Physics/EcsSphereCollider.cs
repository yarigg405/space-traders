using UnityEngine;


namespace Assets.Code.Common.Physics
{
    public class EcsSphereCollider : MonoBehaviour
    {
        [field: SerializeField] public float Radius { get; private set; }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}