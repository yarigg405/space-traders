using UnityEngine;
using UnityEngine.VFX;


namespace Assets.Code.ClientPart.Visual.Player
{
    internal sealed class PlayerWarpEffectView : MonoBehaviour
    {
        [SerializeField] private VisualEffect _warpVfx;

        private void OnEnable()
        {
            HideWarp();
        }

        internal void ShowWarp()
        {
            _warpVfx.Play();
        }

        internal void HideWarp()
        {
            _warpVfx.Stop();
        }
    }
}
