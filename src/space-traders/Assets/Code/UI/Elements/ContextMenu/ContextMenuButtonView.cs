using System;
using UnityEngine;
using UnityEngine.UI;
using Yrr.UI.Elements;


namespace Assets.Code.UI.Elements
{
    public sealed class ContextMenuButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private LocalizeableTmp _label;

        private Action _onClick;

        public void Bind(ContextMenuEntry entry, Action onChosen)
        {
            _onClick = () =>
            {
                entry.Action?.Invoke();
                onChosen?.Invoke();
            };

            _button.onClick.RemoveListener(OnClick);
            _button.onClick.AddListener(OnClick);

            BindLabel(entry.LabelKey);
        }

        private void OnClick()
        {
            _onClick?.Invoke();
        }

        private void BindLabel(string entryKey)
        {
            _label.BindText(entryKey);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
