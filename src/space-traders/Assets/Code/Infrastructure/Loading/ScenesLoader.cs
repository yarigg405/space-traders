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

        public ScenesLoader(SceneLoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        public void LoadScene(string name, Action onLoaded = null)
        {
            CurrentScene = name;
            LoadAsync(name, onLoaded).Forget();
        }

        private async UniTaskVoid LoadAsync(string sceneName, Action onLoaded, CancellationToken cancellationToken = default)
        {
            _loadingScreen.Show();
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            onLoaded?.Invoke();
            _loadingScreen.Hide();
        }
    }
}
