using Assets.Code.UI.Infrastructure;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.MainMenu
{
    internal sealed class MainMenuMainView : UIScreenView
    {
        [field: SerializeField] public Button StartGameBtn { get; private set; }
        [field: SerializeField] public Button JoinGameBtn { get; private set; }
    }
}
