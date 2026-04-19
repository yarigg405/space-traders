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

        private readonly HashSet<Type> _openedScreens = new();
        private readonly HashSet<Type> _openedModals = new();

        public UIManager(IScreensProvider screensProvider, INavigationIntentFactory intentFactory)
        {
            _screensProvider = screensProvider;
            _intentFactory = intentFactory;

            _navigation = new(screensProvider);
        }


        public void GoToScreen<TScreen>(object args = null) where TScreen : IScreen
        {
            var request = _intentFactory.Create(typeof(TScreen), args);
            if (request is IAsyncNavigationIntent asyncIntent)
            {
                HandleAsync(asyncIntent, args).Forget();
            }
            else
            {
                var intent = request as INavigationIntent;
                HandleSync(intent);
            }
        }

        private void HandleSync(INavigationIntent intent)
        {
            if (_isNavigating)
                return;

            var (opened, closed) = _navigation.Push(intent);

            if (closed != null)
            {
                _openedScreens.Remove(closed.GetType());
                OnScreenClosed?.Invoke(closed);
            }

            if (opened != null)
            {
                _openedScreens.Add(opened.GetType());
                OnScreenOpened?.Invoke(opened);
            }
        }

        private async UniTask HandleAsync(IAsyncNavigationIntent intent, object args)
        {
            if (_isNavigating)
                return;

            _isNavigating = true;

            _navigationCts?.Cancel();
            _navigationCts = new CancellationTokenSource();

            var token = _navigationCts.Token;

            try
            {
                var data = await intent.Load(token, args);

                if (token.IsCancellationRequested)
                    return;

                var finalIntent = intent.Create(data);
                var (opened, closed) = _navigation.Push(finalIntent);

                if (closed != null)
                {
                    _openedScreens.Remove(closed.GetType());
                    OnScreenClosed?.Invoke(closed);
                }

                if (opened != null)
                {
                    _openedScreens.Add(opened.GetType());
                    OnScreenOpened?.Invoke(opened);
                }
            }
            catch (OperationCanceledException)
            {
                //...
            }
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
            var (closed, opened) = _navigation.Pop();

            if (closed == null)
                return;

            _openedScreens.Remove(closed.GetType());
            OnScreenClosed?.Invoke(closed);

            if (opened != null)
            {
                _openedScreens.Add(opened.GetType());
                OnScreenOpened?.Invoke(opened);
            }
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
