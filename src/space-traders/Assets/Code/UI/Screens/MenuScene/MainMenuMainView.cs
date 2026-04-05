using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.MainMenu
{
    public sealed class MainMenuMainView : UIScreenView
    {
        [field: SerializeField] public Button StartGameBtn { get; private set; }
        [field: SerializeField] public Button JoinGameBtn { get; private set; }
        [field: SerializeField] public TMP_InputField ServerPortIF { get; private set; }
        [field: SerializeField] public TMP_InputField ServerPasswordIF { get; private set; }

        public override void Show()
        {
            ServerPortIF.text = "40501";
            ServerPasswordIF.text = string.Empty;
            base.Show();
        }
    }
}
