using UnityEngine;

namespace Assets.Code.Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        T LoadAsset<T>(string path) where T : Component;
    }
}