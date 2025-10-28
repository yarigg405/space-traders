#if UNITY_EDITOR
using Entitas.VisualDebugging.Unity.Editor;
using System;
using Unity.Mathematics;
using UnityEditor;


namespace Assets.Code.Editor
{
    internal class Int2TypeDrawer : ITypeDrawer
    {
        bool ITypeDrawer.HandlesType(Type type) => type == typeof(int2);

        object ITypeDrawer.DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            var int2Value = (int2)value;

            EditorGUILayout.LabelField(memberName, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            int x = EditorGUILayout.IntField("X", int2Value.x);
            int y = EditorGUILayout.IntField("Y", int2Value.y);

            EditorGUI.indentLevel--;

            return new int2(x, y);
        }
    }
}
#endif