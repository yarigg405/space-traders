using UnityEngine;
using UnityEngine.UI;


namespace Yrr.UI.Elements
{
    internal sealed class CustomButtonImageColorChanger : CustomButtonStateListener
    {
        [SerializeField] private Image[] _imagesForRecolor;

        [Space]
        [SerializeField] private Color _normal;
        [SerializeField] private Color _highlighted;
        [SerializeField] private Color _pressed;
        [SerializeField] private Color _selected;
        [SerializeField] private Color _disabled;

        protected override void SetNormal()
        {
            for (int i = 0; i < _imagesForRecolor.Length; i++)
            {
                _imagesForRecolor[i].color = _normal;
            }
        }

        protected override void SetDisabled()
        {
            for (int i = 0; i < _imagesForRecolor.Length; i++)
            {
                _imagesForRecolor[i].color = _disabled;
            }
        }

        protected override void SetPressed()
        {
            for (int i = 0; i < _imagesForRecolor.Length; i++)
            {
                _imagesForRecolor[i].color = _pressed;
            }
        }

        protected override void SetHighlighted()
        {
            for (int i = 0; i < _imagesForRecolor.Length; i++)
            {
                _imagesForRecolor[i].color = _highlighted;
            }
        }
    }
}
