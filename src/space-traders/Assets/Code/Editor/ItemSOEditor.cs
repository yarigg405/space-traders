#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Assets.Code.Common.Inventory
{
    [CustomEditor(typeof(ItemSO))]
    [CanEditMultipleObjects]
    internal class ItemSOEditor : Editor
    {
        private int _index = 0;
        private Type[] _allComponentsTypes;
        private string[] _options;

        private void OnEnable()
        {
            var baseType = typeof(InventoryComponent);
            var assembly = baseType.Assembly;
            _allComponentsTypes = assembly.GetTypes()
                .Where(t => t != baseType && baseType.IsAssignableFrom(t)).ToArray();

            _options = _allComponentsTypes.Select(x => x.Name).ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var item = (ItemSO)target;
            _index = EditorGUILayout.Popup("Select Component", _index, _options);

            if (GUILayout.Button("Add Component"))
            {
                var type = _allComponentsTypes[_index];
                var component = Convert.ChangeType(Activator.CreateInstance(type), type);

                item.AddComponent(component);
                EditorUtility.SetDirty(item);
            }
        }
    }
}
#endif