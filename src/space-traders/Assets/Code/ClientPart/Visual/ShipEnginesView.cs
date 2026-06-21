using Assets.Code.ClientPart.View;
using System;
using UnityEngine;


namespace Assets.Code.ClientPart.Visual
{
    internal sealed class ShipEnginesView : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _behaviour;

        [SerializeField] private GameObject[] _commonMoveObjects;
        [SerializeField] private GameObject[] _warpMoveObjects;

        private IDisposable _isWarpingSubscription;
        private IDisposable _animationSubscription;

        private void Start()
        {
            _isWarpingSubscription = _behaviour.Entity.ViewModel.IsWarping.Subscribe(OnWarpChanged, true);
            _animationSubscription = _behaviour.Entity.ViewModel.IsAnimatedMoving.Subscribe(OnAnimationChanged, true);
        }

        private void OnDestroy()
        {
            _isWarpingSubscription?.Dispose();
            _isWarpingSubscription = null;

            _animationSubscription?.Dispose();
            _animationSubscription = null;
        }

        private void OnWarpChanged(bool isWarping)
        {
            foreach (var obj in _commonMoveObjects)
            {
                obj.SetActive(!isWarping);
            }

            foreach (var obj in _warpMoveObjects)
            {
                obj.SetActive(isWarping);
            }
        }

        private void OnAnimationChanged(bool animated)
        {
            foreach (var obj in _commonMoveObjects)
            {
                obj.SetActive(!animated);
            }
        }
    }
}
