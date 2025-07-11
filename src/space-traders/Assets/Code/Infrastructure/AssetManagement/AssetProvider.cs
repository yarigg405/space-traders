using UnityEngine;


namespace Assets.Code.Infrastructure.AssetManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        T IAssetProvider.LoadAsset<T>(string path)
        {
            return Resources.Load<T>(path);
        }
    }
}
