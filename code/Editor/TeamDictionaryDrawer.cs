using UnityEngine;
using UnityEditor;

namespace players.rts
{
    [CustomPropertyDrawer(typeof(TeamDictionary), true)]
    public class TeamDictionaryDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty _Property, GUIContent _Label)
        {
            return EditorGUI.GetPropertyHeight(_Property, _Label, true);
        }

        public override void OnGUI(Rect _Position, SerializedProperty _Property, GUIContent _Label)
        {
            EditorGUI.BeginProperty(_Position, _Label, _Property);
            EditorGUI.DropdownButton(_Position, new GUIContent("true"), FocusType.Passive);
            EditorGUI.PropertyField(_Position, _Property, true);
            EditorGUI.EndProperty();
        }
    }
}