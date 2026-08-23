using UnityEngine;
using Yrr.UI.Elements;


namespace Assets.Code.UI.Screens
{
    public sealed class MessageBoxView : UIScreenView
    {
        [SerializeField] private LocalizeableTmp _localizedTmp;

        internal void SetMessage(string entry, string table, params object[] args)
        {
            _localizedTmp.BindText(entry, table, args);
        }
    }
}
