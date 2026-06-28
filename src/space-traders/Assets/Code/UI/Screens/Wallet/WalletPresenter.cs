using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Extensions;
using Assets.Code.UI.Infrastructure.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


namespace Assets.Code.UI.Screens.Wallet
{
    public sealed class WalletPresenter : IPresenter<WalletView>
    {
        private readonly IUIManager _uiManager;
        private readonly ClientMessenger _messenger;

        private WalletView _view;
        private CancellationTokenSource _cts;

        public WalletPresenter(IUIManager uiManager, ClientMessenger messenger)
        {
            _uiManager = uiManager;
            _messenger = messenger;
        }

        void IPresenter<WalletView>.Show(WalletView view, object args)
        {
            _view = view;
            view.CloseButton.onClick.AddListener(Close);

            _cts = new();
            LoadBalanceAsync(_cts.Token).Forget();
        }

        void IPresenter<WalletView>.Hide(WalletView view)
        {
            view.CloseButton.onClick.RemoveListener(Close);

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTask LoadBalanceAsync(CancellationToken ct)
        {
            SetLoading(true);

            try
            {
                var money = await _messenger.RequestForMoney(ct);

                if (ct.IsCancellationRequested)
                    return;

                _view.BalanceTmp.text = money.ToMoney();
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                if (ct.IsCancellationRequested)
                    return;

                Debug.LogError($"Failed to load wallet balance: {e.Message}");
                _view.BalanceTmp.text = "—";
            }
            finally
            {
                if (!ct.IsCancellationRequested)
                    SetLoading(false);
            }
        }

        private void SetLoading(bool isLoading)
        {
            _view.LoadingSpinner.SetActive(isLoading);
            _view.BalanceTmp.gameObject.SetActive(!isLoading);
        }

        private void Close()
        {
            _uiManager.CloseModal<WalletScreen>();
        }
    }
}
