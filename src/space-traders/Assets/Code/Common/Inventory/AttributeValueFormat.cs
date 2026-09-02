using UnityEngine.Localization;


namespace Assets.Code.Common.Inventory
{
    public static class AttributeValueFormat
    {
        private const string LocalizationTable = "LocalizationTable";

        public static string Format(string formatKey, float value)
        {
            var localized = new LocalizedString
            {
                TableReference = LocalizationTable,
                TableEntryReference = formatKey,
                Arguments = new object[] { value.ToString("N0") }
            };

            return localized.GetLocalizedString();
        }
    }
}
