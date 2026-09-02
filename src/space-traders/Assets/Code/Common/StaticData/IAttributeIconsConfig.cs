using UnityEngine;


namespace Assets.Code.Common.StaticData
{
    public interface IAttributeIconsConfig
    {
        Sprite Get(string attributeKey);
    }
}
