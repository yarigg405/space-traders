#if UNITY_EDITOR
using Entitas.VisualDebugging.Unity.Editor;
using System;
using Unity.Mathematics;
using UnityEditor;


namespace Assets.Code
{
    internal class Double2TypeDrawer : ITypeDrawer
    {
        bool ITypeDrawer.HandlesType(Type type) => type == typeof(double2);


        object ITypeDrawer.DrawAndGetNewValue(Type memberType, string memberName, object value, object target)
        {
            var double2Value = (double2)value;

            EditorGUILayout.LabelField(memberName, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            double x = EditorGUILayout.DoubleField("X", double2Value.x);
            double y = EditorGUILayout.DoubleField("Y", double2Value.y);

            EditorGUI.indentLevel--;

            return new double2(x, y);
        }
    }
}
#endif