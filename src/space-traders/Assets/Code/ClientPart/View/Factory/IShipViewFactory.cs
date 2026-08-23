using UnityEngine;


namespace Assets.Code.ClientPart.View.Factory
{
    public interface IShipViewFactory
    {
        GameObject CreateShipModel(string shipModelId, Transform parent);
    }
}
