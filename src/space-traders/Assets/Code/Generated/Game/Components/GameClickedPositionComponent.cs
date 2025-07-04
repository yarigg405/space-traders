//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherClickedPosition;

    public static Entitas.IMatcher<GameEntity> ClickedPosition {
        get {
            if (_matcherClickedPosition == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ClickedPosition);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherClickedPosition = matcher;
            }

            return _matcherClickedPosition;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Assets.Code.Gameplay.InputInteraction.ClickedPosition clickedPosition { get { return (Assets.Code.Gameplay.InputInteraction.ClickedPosition)GetComponent(GameComponentsLookup.ClickedPosition); } }
    public UnityEngine.Vector3 ClickedPosition { get { return clickedPosition.Value; } }
    public bool hasClickedPosition { get { return HasComponent(GameComponentsLookup.ClickedPosition); } }

    public GameEntity AddClickedPosition(UnityEngine.Vector3 newValue) {
        var index = GameComponentsLookup.ClickedPosition;
        var component = (Assets.Code.Gameplay.InputInteraction.ClickedPosition)CreateComponent(index, typeof(Assets.Code.Gameplay.InputInteraction.ClickedPosition));
        component.Value = newValue;
        AddComponent(index, component);
        return this;
    }

    public GameEntity ReplaceClickedPosition(UnityEngine.Vector3 newValue) {
        var index = GameComponentsLookup.ClickedPosition;
        var component = (Assets.Code.Gameplay.InputInteraction.ClickedPosition)CreateComponent(index, typeof(Assets.Code.Gameplay.InputInteraction.ClickedPosition));
        component.Value = newValue;
        ReplaceComponent(index, component);
        return this;
    }

    public GameEntity RemoveClickedPosition() {
        RemoveComponent(GameComponentsLookup.ClickedPosition);
        return this;
    }
}
