using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Code.UI.Screens.MainMenu
{
    internal sealed class SelectCharacterCardView : MonoBehaviour
    {
        [SerializeField] private Image _characterIcon;
        [SerializeField] private TextMeshProUGUI _characterNameTmp;
        [field: SerializeField] public Button SelectCharacterButton { get; private set; }

        internal void SetupName(string name)
        {
            _characterNameTmp.text = name;
        }
    }
}
