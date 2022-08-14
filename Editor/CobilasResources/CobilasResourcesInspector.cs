using UnityEngine;
using UnityEditor;
using Cobilas.Unity.Management.Resources;
using UEEditor = UnityEditor.Editor;

namespace Cobilas.Unity.Editor.Management.Resources {
    [CustomEditor(typeof(CobilasResources))]
    public class CobilasResourceContainerInspector : UEEditor {

        private MSerializedProperty[] properties;
        private Vector2 scrollView;

        private void OnEnable() {
            SerializedProperty resourceList = serializedObject.FindProperty("itens");
            properties = new MSerializedProperty[resourceList.arraySize];
            for (int I = 0; I < properties.Length; I++)
                properties[I] = new MSerializedProperty() {
                    relativePath = resourceList.GetArrayElementAtIndex(I).FindPropertyRelative("relativePath").stringValue,
                    p_item = resourceList.GetArrayElementAtIndex(I).FindPropertyRelative("item"),
                    item = resourceList.GetArrayElementAtIndex(I).FindPropertyRelative("item").objectReferenceValue
                };
        }

        public override void OnInspectorGUI() {
            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Resources", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            for (int I = 0; I < properties.Length; I++) {
                string relativePath = properties[I].relativePath;
                SerializedProperty p_item = properties[I].p_item;
                Object item = properties[I].item;
                EditorGUILayout.LabelField($"Path:{relativePath}");
                if (item == null) {
                    EditorGUILayout.LabelField("Item : none");
                } else {
                    EditorGUILayout.LabelField($"Item name:{item.name}");
                    EditorGUILayout.LabelField($"Type:{item.GetType().Name}");
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(p_item);
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.Space();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private struct MSerializedProperty {
            public string relativePath;
            public SerializedProperty p_item;
            public Object item;
        }
    }
}
