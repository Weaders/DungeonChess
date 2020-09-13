using System;
using System.CodeDom;
using Assets.Scripts.Observable;
using Assets.Scripts.StarsData;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor {

    [CustomPropertyDrawer(typeof(StatField))]
    public class StatTypeEditor : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            property.serializedObject.Update();

            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var statTypeProp = property.FindPropertyRelative("statType");

            var vals = Enum.GetValues(typeof(Stat));

            var val = vals.GetValue(statTypeProp.enumValueIndex) as Stat?;

            var observeVal = val.Value.GetObservableVal();

            // Fuck you unity!
            if ($"managedReference<{observeVal.GetType().Name}>" != property.FindPropertyRelative("observableVal").type) {
                property.FindPropertyRelative("observableVal").managedReferenceValue = observeVal;
            }
            

            EditorGUI.PropertyField(new Rect(position.position, position.size - new Vector2(0, 1)), property.FindPropertyRelative("statType"));

            EditorGUI.PropertyField(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - 15), property.FindPropertyRelative("observableVal").FindPropertyRelative("_val"));

            EditorGUI.EndProperty();

            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

            var totalHeight = (EditorGUI.GetPropertyHeight(property, label, true) * 2);

            return totalHeight;
        }

    }

}
