using TMPro;
using UnityEngine;


namespace Assets.Code.UI.Screens
{
    public sealed class MessageBoxView : UIScreenView
    {
        [SerializeField] private TextMeshProUGUI _messageTmp;

        internal void SetMessageText(string text)
        {
            _messageTmp.text = text;
        }
    }
}
