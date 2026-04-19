using Assets.Code.UI.Infrastructure.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Assets.Code.UI.Infrastructure.Impl
{
    public sealed class UIManager : IUIManager, IDisposable
    {
        public event Action<IScreen> OnScreenOpened;
        public event Action<IScreen> OnScreenClosed;

        private readonly IScreensProvider _screensProvider;
        private readonly INavigationIntentFactory _intentFactory;
        private readonly UiNavigationStack _navigation;

        private bool _isNavigating;
        private CancellationTokenSource _navigationCts;
        private IScreen _currentScreen;

        private readonly HashSet<Type> _openedScreens = new();
        private readonly HashSet<Type> _openedModals = new();

        public UIManager(IScreensProvider screensProvider, INavigationIntentFactory intentFactory)
        {
            _screensProvider = screensProvider;
            _intentFactory = intentFactory;
            _navigation = new();
        }


        public void GoToScreen<TScreen>(object args = null) where TScreen : IScreen
        {
            var request = _intentFactory.Create(typeof(TScreen), args);
            _navigation.Push(request);
            ExecuteRequest(request).Forget();
        }

        private async UniTask ExecuteRequest(INavigationRequest request)
        {
            if (_isNavigating)
                return;

            _isNavigating = true;

            _navigationCts?.Cancel();
            _navigationCts = new CancellationTokenSource();

            var token = _navigationCts.Token;

            try
            {
                IScreen opened = null;
                var closed = _currentScreen;

                if (request is IAsyncNavigationIntent asyncIntent)
                {
                    var data = await asyncIntent.Load(token);

                    if (token.IsCancellationRequested)
                        return;

                    var finalIntent = asyncIntent.Create(data);
                    opened = finalIntent.Execute(_screensProvider);
                }
                else if (request is INavigationIntent syncIntent)
                {
                    opened = syncIntent.Execute(_screensProvider);
                }

                if (closed != null)
                {
                    closed.Hide();
                    _openedScreens.Remove(closed.GetType());
                    OnScreenClosed?.Invoke(closed);
                }

                if (opened != null)
                {
                    _currentScreen = opened;

                    _openedScreens.Add(opened.GetType());
                    OnScreenOpened?.Invoke(opened);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
            finally
            {
                _isNavigating = false;
            }
        }


        public void BackToPreviousScreen()
        {
            var request = _navigation.Pop();

            if (request == null)
                return;

            ExecuteRequest(request).Forget();
        }


        public void OpenModal<TPopup>(object args = null) where TPopup : IScreen
        {
            var type = typeof(TPopup);

            if (_openedModals.Contains(type))
                return;

            var modal = _screensProvider.GetScreen<TPopup>();
            modal.Show(args);
            _openedModals.Add(type);
            OnScreenOpened?.Invoke(modal);
        }

        public void CloseModal<TPopup>() where TPopup : IScreen
        {
            var type = typeof(TPopup);

            if (!_openedModals.Contains(type))
                return;

            var modal = _screensProvider.GetScreen<TPopup>();
            modal.Hide();
            _openedModals.Remove(type);
            OnScreenClosed?.Invoke(modal);
        }

        public void CloseAllModals()
        {
            foreach (var type in _openedModals)
            {
                var modal = _screensProvider.GetScreen(type);
                modal.Hide();
                OnScreenClosed?.Invoke(modal);
            }

            _openedModals.Clear();
        }


        public void ClearHistory()
        {
            _navigation.Clear();
        }

        void IDisposable.Dispose()
        {
            CloseAllModals();

            _navigationCts?.Cancel();
            _navigationCts?.Dispose();
        }
    }
}
