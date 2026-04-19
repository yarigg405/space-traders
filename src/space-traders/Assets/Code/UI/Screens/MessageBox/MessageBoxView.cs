using TMPro;
using UnityEngine;
using UnityEngine.Localization;


namespace Assets.Code.UI.Screens
{
    public sealed class MessageBoxView : UIScreenView
    {
        [SerializeField] private TextMeshProUGUI _messageTmp;
        private LocalizedString _message;

        internal void SetMessage(string table, string entry, params object[] args)
        {
            Unbind();

            _message = new LocalizedString
            {
                TableReference = table,
                TableEntryReference = entry,
                Arguments = args
            };

            _message.StringChanged += OnMessageChanged;
            _message.RefreshString();
        }

        private void OnMessageChanged(string value)
        {
            _messageTmp.text = value;
        }

        private void OnDisable()
        {
            Unbind();
        }

        private void Unbind()
        {
            if (_message == null)
                return;

            _message.StringChanged -= OnMessageChanged;
            _message = null;
        }
    }
}
