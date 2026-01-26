using UnityEngine;


namespace Assets.Code.Common.StaticData
{
    [CreateAssetMenu(fileName = "ConfigsStorage", menuName = "ScriptableObjects/ConfigsStorage", order = 51)]
    public sealed class ConfigsStorage : ScriptableObject
    {
        [field: SerializeField] public AnimationCurve WarpAccelerationCurve { get; private set; }
    }
}
