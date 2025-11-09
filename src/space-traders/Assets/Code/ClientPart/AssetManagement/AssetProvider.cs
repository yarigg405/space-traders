using UnityEngine;


namespace Assets.Code.ClientPart.AssetManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        T IAssetProvider.LoadAsset<T>(string path)
        {
            return Resources.Load<T>(path);
        }
    }
}
