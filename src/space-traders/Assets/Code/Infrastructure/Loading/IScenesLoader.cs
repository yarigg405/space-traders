using System;


namespace Assets.Code.Infrastructure.Loading
{
    public interface IScenesLoader
    {
        void LoadScene(string name, Action onLoaded = null);
        string CurrentScene { get; }
    }
}