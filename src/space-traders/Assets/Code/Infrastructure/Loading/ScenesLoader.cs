using Assets.Code.Networking;
using Assets.Code.UI.LoadingScreens;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.SceneManagement;


namespace Assets.Code.Infrastructure.Loading
{
    internal sealed class ScenesLoader : IScenesLoader
    {
        public string CurrentScene { get; private set; }
        private readonly SceneLoadingScreen _loadingScreen;
        private readonly ICancellationToken _cts;

        public ScenesLoader(SceneLoadingScreen loadingScreen, ICancellationToken cts)
        {
            _loadingScreen = loadingScreen;
            _cts = cts;
        }

        public void LoadScene(string name, Action onLoaded = null)
        {
            CurrentScene = name;
            LoadAsync(name, onLoaded,_cts.Token).Forget();
        }

        private async UniTaskVoid LoadAsync(string sceneName, Action onLoaded, CancellationToken cancellationToken)
        {
            _loadingScreen.Show();
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask().AttachExternalCancellation(cancellationToken);
            onLoaded?.Invoke();
            _loadingScreen.Hide();
        }
    }
}
