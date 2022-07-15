using System;
using UnityEditor;
using UnityEngine;

namespace Cobilas.Unity.Editor.Management.Resources.Asset {
    using UEObject = UnityEngine.Object;
    [CreateAssetMenu(fileName = "Assets item ref", menuName = "Cobilas/Asset management/Assets item ref")]
    public class AssetsItemRef : ScriptableObject {

        public bool UseTargetObjectName;
        public string S_Type;
        public UEObject MyObject;

        public Type MyType {
            get {
                if (typeof(Shader).FullName == S_Type) return typeof(Shader);
                if (typeof(Sprite).FullName == S_Type) return typeof(Sprite);
                if (typeof(Texture).FullName == S_Type) return typeof(Texture);
                if (typeof(UEObject).FullName == S_Type) return typeof(UEObject);
                if (typeof(Material).FullName == S_Type) return typeof(Material);
                if (typeof(TextAsset).FullName == S_Type) return typeof(TextAsset);
                if (typeof(GameObject).FullName == S_Type) return typeof(GameObject);
                if (typeof(SceneAsset).FullName == S_Type) return typeof(SceneAsset);
                if (typeof(ScriptableObject).FullName == S_Type) return typeof(ScriptableObject);
                return null;
            }
        }

        private void Awake() {
            UseTargetObjectName = true;
            S_Type = typeof(UEObject).FullName;
        }

        public static AssetsItemRef[] GetAllAssetsItemRefs() {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(AssetsItemRef).Name}");
            AssetsItemRef[] res = new AssetsItemRef[guids.Length];

            for (int I = 0; I < res.Length; I++)
                res[I] = AssetDatabase.LoadAssetAtPath<AssetsItemRef>(AssetDatabase.GUIDToAssetPath(guids[I]));
            
            return res;
        }
    }
}