using UnityEngine;
using UnityEditor;
using manager.ioc;

namespace players.rts
{
    [CustomPropertyDrawer(typeof(CustomDict<string, Player>))]
    [CustomPropertyDrawer(typeof(PlayerDictionary), true)]
    public class PlayerDictionaryDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty _Property, GUIContent _Label)
        {
            return EditorGUI.GetPropertyHeight(_Property, _Label, true);
        }

        public override void OnGUI(Rect _Position, SerializedProperty _Property, GUIContent _Label)
        {
            EditorGUI.BeginProperty(_Position, _Label, _Property);
            EditorGUI.PropertyField(_Position, _Property, true);
            EditorGUI.EndProperty();
        }
    }
}
