using System;


namespace Assets.Code.UI.Elements
{
    public readonly struct ContextMenuEntry
    {
        public readonly string LabelKey;
        public readonly Action Action;

        public ContextMenuEntry(string labelKey, Action action)
        {
            LabelKey = labelKey;
            Action = action;
        }
    }
}
