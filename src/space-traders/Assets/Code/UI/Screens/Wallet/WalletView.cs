using DG.Tweening;
using System;
using TMPro;
using UnityEngine;


namespace Assets.Code.UI.Screens.Wallet
{
    public sealed class WalletView : UIScreenView
    {
        [field: SerializeField] public GameObject LoadingSpinner { get; private set; }
        [field: SerializeField] public TextMeshProUGUI BalanceTmp { get; private set; }

        [SerializeField] private RectTransform _animationRoot;
        [SerializeField] private float _slideDuration = 0.35f;

        private Vector2 _shownPosition;
        private float _hiddenX;
        private bool _boundsCached;

        public override void Show()
        {
            gameObject.SetActive(true);
            CacheBounds();

            _animationRoot.DOKill();
            _animationRoot.anchoredPosition = new Vector2(_hiddenX, _shownPosition.y);
            _animationRoot.DOAnchorPos(_shownPosition, _slideDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);
        }

        public void PlayClose(Action onComplete)
        {
            CacheBounds();

            _animationRoot.DOKill();
            _animationRoot.DOAnchorPosX(_hiddenX, _slideDuration)
                .SetEase(Ease.InCubic)
                .SetUpdate(true)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void Hide()
        {
            _animationRoot.DOKill();
            base.Hide();
        }

        private void CacheBounds()
        {
            if (_boundsCached)
                return;

            _shownPosition = _animationRoot.anchoredPosition;
            var rootWidth = ((RectTransform)transform).rect.width;
            _hiddenX = _shownPosition.x - rootWidth;
            _boundsCached = true;
        }
    }
}
