#if UNITY_EDITOR
using Assets.Code.Common.StaticData;
using UnityEditor;
using UnityEngine;


namespace Assets.Code.Common.TradingSystem
{
    [CustomEditor(typeof(TradeItemsCategoryConfig))]
    public sealed class ItemsCategoryStorageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (GUILayout.Button("UpdateCategories"))
            {
                var config = (TradeItemsCategoryConfig)target;

                config.UpdateCategoryIds();

                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssetIfDirty(config);
            }
        }
    }
}

#endif