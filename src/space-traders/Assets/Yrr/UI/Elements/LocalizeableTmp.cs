using TMPro;
using UnityEngine;
using UnityEngine.Localization;


namespace Yrr.UI.Elements
{
    public sealed class LocalizeableTmp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageTmp;
        private LocalizedString _message;

        private const string _table = "LocalizationTable";

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!_messageTmp)
                _messageTmp = GetComponent<TextMeshProUGUI>();
        }
#endif

        public void BindText(string entry, params object[] args)
        {
            Unbind();

            _message = new LocalizedString
            {
                TableReference = _table,
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