#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Yrr.Utils;

namespace Assets.Yrr.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        private static GUIStyle _readonlyStringStyle;
        private static Texture2D _readonlyBackground;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                DrawSelectableReadonlyString(position, property, label);
            }
            else
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }

            EditorGUI.EndProperty();
        }

        private static void DrawSelectableReadonlyString(
            Rect position,
            SerializedProperty property,
            GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            Rect valueRect = EditorGUI.PrefixLabel(position, label);

            GUIStyle style = GetReadonlyStringStyle();

            EditorGUI.SelectableLabel(
                valueRect,
                property.stringValue,
                style);
        }

        private static GUIStyle GetReadonlyStringStyle()
        {
            if (_readonlyStringStyle != null)
                return _readonlyStringStyle;

            Color backgroundColor;
            Color textColor;

            if (EditorGUIUtility.isProSkin)
            {
                backgroundColor = new Color(0.13f, 0.13f, 0.13f, 1f);
                textColor = new Color(0.55f, 0.55f, 0.55f, 1f);
            }
            else
            {
                backgroundColor = new Color(0.82f, 0.82f, 0.82f, 1f);
                textColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            }

            _readonlyBackground = MakeTexture(backgroundColor);

            _readonlyStringStyle = new GUIStyle(EditorStyles.textField)
            {
                normal =
                {
                    background = _readonlyBackground,
                    textColor = textColor
                },
                focused =
                {
                    background = _readonlyBackground,
                    textColor = textColor
                },
                hover =
                {
                    background = _readonlyBackground,
                    textColor = textColor
                },
                active =
                {
                    background = _readonlyBackground,
                    textColor = textColor
                }
            };

            return _readonlyStringStyle;
        }

        private static Texture2D MakeTexture(Color color)
        {
            var texture = new Texture2D(1, 1)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            texture.SetPixel(0, 0, color);
            texture.Apply();

            return texture;
        }
    }
}
#endif