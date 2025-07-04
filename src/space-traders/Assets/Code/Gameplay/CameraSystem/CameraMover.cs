using DG.Tweening;
using UnityEngine;


namespace Assets.Code.Gameplay.CameraSystem
{
    internal sealed class CameraMover : MonoBehaviour
    {
        private Transform _target;

        private void LateUpdate()
        {
            if (!_target) return;

            transform.position = _target.position;
        }

        internal void SetNewTarget(Transform newTarget)
        {
            _target = null;
            DOTween.Kill(gameObject);

            var seq = DOTween.Sequence(gameObject).SetUpdate(true)
                .Append(transform.DOMove(newTarget.position, 1f))
                .AppendCallback(() => _target = newTarget);
        }
    }
}
