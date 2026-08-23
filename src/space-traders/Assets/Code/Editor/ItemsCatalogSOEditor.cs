#if UNITY_EDITOR
using Assets.Code.Common.Inventory;
using Assets.Code.Common.StaticData;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Assets.Code
{
    [CustomEditor(typeof(ItemsCatalogSO))]
    internal class ItemsCatalogSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (GUILayout.Button("RefreshItems"))
            {
                RefreshItems((ItemsCatalogSO)target);
            }
        }

        private static void RefreshItems(ItemsCatalogSO catalog)
        {
            var items = AssetDatabase.FindAssets($"t:{nameof(ItemSO)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ItemSO>)
                .Where(item => item != null)
                .ToArray();

            catalog.SetItems(items);

            EditorUtility.SetDirty(catalog);
            AssetDatabase.SaveAssetIfDirty(catalog);
        }
    }
}
#endif