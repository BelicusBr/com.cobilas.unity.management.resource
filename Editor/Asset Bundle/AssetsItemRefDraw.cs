using UnityEditor;
using UnityEngine;
using Cobilas.Unity.Utility;
using UEEditor = UnityEditor.Editor;

namespace Cobilas.Unity.Editor.Management.Resources.Asset {
    [CustomEditor(typeof(AssetsItemRef))]
    public class AssetsItemRefDraw : UEEditor {

        private AssetsItemRef temp;
        private string[] s_types;
        private string[] s_types_display;
        private int index;

        private void OnEnable() {
            index = 0;
            temp = (AssetsItemRef)target;
            s_types = new string[] {
                typeof(Object).FullName,
                typeof(Texture).FullName,
                typeof(GameObject).FullName,
                typeof(TextAsset).FullName,
                typeof(Sprite).FullName,
                typeof(ScriptableObject).FullName,
                typeof(Material).FullName,
                typeof(SceneAsset).FullName,
                typeof(Shader).FullName
            };
            s_types_display = new string[] {
                nameof(Object),
                nameof(Texture),
                nameof(GameObject),
                nameof(TextAsset),
                nameof(Sprite),
                nameof(ScriptableObject),
                nameof(Material),
                nameof(SceneAsset),
                nameof(Shader)
            };
            for (int I = 0; I < s_types.Length; I++)
                if (s_types[I] == temp.S_Type) {
                    index = I;
                    break;
                }
        }

        private void OnDisable()
        {
            temp = null;
            System.Array.Clear(s_types, 0, s_types.Length);
            System.Array.Clear(s_types_display, 0, s_types_display.Length);
        }

        public override void OnInspectorGUI() {
            temp.UseTargetObjectName = EditorGUILayout.Toggle("Use target object name", temp.UseTargetObjectName);
            EditorGUI.BeginChangeCheck();
            index = EditorGUILayout.Popup("Type", index, s_types_display);
            if (EditorGUI.EndChangeCheck()) {
                temp.S_Type = s_types[index];
                temp.MyObject = null;
            }
            EditorGUI.BeginChangeCheck();
            temp.MyObject = EditorGUILayout.ObjectField("Object ref", temp.MyObject, temp.MyType, true);
            if (EditorGUI.EndChangeCheck())
                if (temp.MyObject != null && temp.UseTargetObjectName) {
                    AssetDatabase.RenameAsset(
                        AssetDatabase.GetAssetPath(temp),
                        CobilasPaths.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(temp.MyObject))
                    );
                    AssetDatabase.Refresh();
                }
        }
    }
}