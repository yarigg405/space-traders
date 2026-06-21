using Assets.Code.ClientPart.View;
using DG.Tweening;
using System;
using UnityEngine;


namespace Assets.Code.ClientPart.Visual
{
    internal sealed class ShipAnimatedMover : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _behaviour;
        [SerializeField] private Transform _viewRoot;

        private IDisposable _subscription;

        private void Start()
        {
            _subscription = _behaviour.Entity.ViewModel.IsAnimatedMoving.Subscribe(OnAnimationChanged, true);
        }

        private void OnDestroy()
        {
            DOTween.Kill(_viewRoot);
            _subscription?.Dispose();
            _subscription = null;
        }

        private void OnAnimationChanged(bool animated)
        {
            DOTween.Kill(_viewRoot);

            if (animated)
            {
                var container = _behaviour.Entity.AnimatedMovingDataContainer;
                _viewRoot.transform.position = container.From;
                _viewRoot.DOMove(container.To, container.MaxTime).SetEase(Ease.InOutCubic);
            }

            else
            {
                _viewRoot.DOLocalMove(Vector3.zero, 0.3f);
            }
        }
    }
}
