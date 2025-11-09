using UnityEngine;

namespace Assets.Code.ClientPart.AssetManagement
{
    public interface IAssetProvider
    {
        T LoadAsset<T>(string path) where T : Component;
    }
}