using System;
using System.Linq;
using Assets.Scripts.Observable;
using Assets.Scripts.StatsData;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor {

    [CustomPropertyDrawer(typeof(StatField))]
    public class StatTypeEditor : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            EditorGUI.BeginProperty(position, label, property);

            var statTypeProp = property.FindPropertyRelative("changeStatType");

            var vals = Enum.GetValues(typeof(ChangeStatType)).Cast<ChangeStatType?>().ToArray();

            ChangeStatType? val;

            if (vals.Length < statTypeProp.enumValueIndex) {
                val = ChangeStatType.None;
            } else {
                val = vals[statTypeProp.enumValueIndex];
            }

            var observeVal = val.Value.GetObservableVal(null);

            // Fuck you unity!
            //if ($"managedReference<{observeVal.GetType().Name}>" != property.FindPropertyRelative("observableVal").type) {
            //    property.FindPropertyRelative("observableVal").managedReferenceValue = observeVal;
            //}

            EditorGUI.PropertyField(new Rect(position.position, new Vector2((EditorGUIUtility.currentViewWidth / 2f) - 20, position.size.y)), property.FindPropertyRelative("changeStatType"), GUIContent.none);
            //EditorGUI.PropertyField(new Rect((EditorGUIUtility.currentViewWidth / 2f), position.y, (EditorGUIUtility.currentViewWidth / 2f), position.height), property.FindPropertyRelative("observableVal").FindPropertyRelative("_val"), GUIContent.none);

            //object oldVal = null;

            // TODO: Rewrite this shit
            if (observeVal is IntObservable) {
                EditorGUI.PropertyField(new Rect((EditorGUIUtility.currentViewWidth / 2f), position.y, (EditorGUIUtility.currentViewWidth / 2f), position.height), property.FindPropertyRelative("intVal"), GUIContent.none);
            } else if (observeVal is FloatObsrevable) {
                EditorGUI.PropertyField(new Rect((EditorGUIUtility.currentViewWidth / 2f), position.y, (EditorGUIUtility.currentViewWidth / 2f), position.height), property.FindPropertyRelative("floatVal"), GUIContent.none);
            } else if (observeVal is BoolObservable) {
                EditorGUI.PropertyField(new Rect((EditorGUIUtility.currentViewWidth / 2f), position.y, (EditorGUIUtility.currentViewWidth / 2f), position.height), property.FindPropertyRelative("boolVal"), GUIContent.none);
            } else {

                //var oldVal = property.FindPropertyRelative("observableVal").FindPropertyRelative("_val").;
                //property.FindPropertyRelative("observableVal").managedReferenceValue = copyVal;
                //property.FindPropertyRelative("observableVal").FindPropertyRelative("_val").boolValue = oldVal;

            }

            //var copyVal = val.Value.GetObservableVal(oldVal);
            //property.FindPropertyRelative("observableVal").managedReferenceValue = copyVal;

        }

    }

}
