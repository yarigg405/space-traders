using Assets.Code.Serialization;
using Assets.Code.View;
using Entitas;


namespace Assets.Code.Common
{
    [Game] public class View : IComponent { public EntityBehaviour Value; }
    [Game] public class ViewPath : ISerializeComponent { public string Value; }
    [Game] public class ViewPrefab : IComponent { public EntityBehaviour Value; }
    [Game] public class Destructed : ISerializeComponent { }
    [Game] public class SelfDestructTimer : ISerializeComponent { public float Value; }
}
