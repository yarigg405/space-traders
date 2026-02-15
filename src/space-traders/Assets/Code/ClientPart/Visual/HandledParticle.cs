using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;
using VContainer;


namespace Assets.Code.ClientPart.Visual
{
    internal sealed class HandledParticle : MonoBehaviour
    {
        [SerializeField] private VisualEffect _vfx;

        [Inject] private readonly ParticlesHandler _particlesHandler;

        private const string _teleportOffset = "TeleportOffset";
        private const string _needTeleportProperty = "ApplyTeleport";

        private void OnEnable()
        {
            _particlesHandler.AddParticle(this);
        }

        private void OnDisable()
        {
            _particlesHandler.RemoveParticle(this);
        }

        internal void TeleportOffset(Vector3 deltaV3)
        {
            _vfx.SetVector3(_teleportOffset, deltaV3);
            _vfx.SetBool(_needTeleportProperty, true);

            SetTeleportOff().Forget();
        }

        private async UniTask SetTeleportOff()
        {
            await UniTask.NextFrame();
            _vfx.SetBool(_needTeleportProperty, false);
        }
    }
}
