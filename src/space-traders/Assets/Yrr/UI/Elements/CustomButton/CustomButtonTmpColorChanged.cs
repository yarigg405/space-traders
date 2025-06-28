using TMPro;
using UnityEngine;


namespace Yrr.UI.Elements
{
    internal sealed class CustomButtonTmpColorChanged : CustomButtonStateListener
    {
        [SerializeField] private TextMeshProUGUI[] _tmpsForRecolor;

        [Space]
        [SerializeField] private Color _normal;
        [SerializeField] private Color _highlighted;
        [SerializeField] private Color _pressed;
        [SerializeField] private Color _selected;
        [SerializeField] private Color _disabled;

        protected override void SetNormal()
        {
            for (int i = 0; i < _tmpsForRecolor.Length; i++)
            {
                _tmpsForRecolor[i].color = _normal;
            }
        }

        protected override void SetDisabled()
        {
            for (int i = 0; i < _tmpsForRecolor.Length; i++)
            {
                _tmpsForRecolor[i].color = _disabled;
            }
        }

        protected override void SetPressed()
        {
            for (int i = 0; i < _tmpsForRecolor.Length; i++)
            {
                _tmpsForRecolor[i].color = _pressed;
            }
        }

        protected override void SetHighlighted()
        {
            for (int i = 0; i < _tmpsForRecolor.Length; i++)
            {
                _tmpsForRecolor[i].color = _highlighted;
            }
        }
    }
}
