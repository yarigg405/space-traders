using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.MainMenu
{
    public sealed class CreateCharacterView : UIScreenView
    {
        [field: SerializeField] public TMP_InputField CharacterNameIF { get; private set; }
        [field: SerializeField] public Button CreateCharacterButton { get; private set; }
    }
}
