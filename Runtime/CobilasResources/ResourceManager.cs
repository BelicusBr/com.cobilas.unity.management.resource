﻿using System.IO;
using UnityEngine;
using UnityEngine.Video;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;
using UEResources = UnityEngine.Resources;
#if UNITY_EDITOR
using UnityEditor;
using Cobilas.Unity.Management.Build;
using Cobilas.Unity.Management.Runtime;
#endif

namespace Cobilas.Unity.Management.Resources {
    [CreateAssetMenu(fileName = "Resource Container", menuName = "Resource manager/Container")]
    public class ResourceManager : ScriptableObject {
        [SerializeField] private ResourceItem[] itens;

#if UNITY_EDITOR
        [StartBeforeSceneLoad("#ResourceManager")]
        [MenuItem("Tools/Resource manager/Refresh ResourceManager")]
        private static void Refresh() {
            Debug.Log($"[Resources]Refresh resources paths[{System.DateTime.Now}]");
            ResourceItem[] resources = CreateResourceItemList(GetResourceObjects());
            foreach (var item in GetContainers()) {
                item.UnloadResourceList();
                item.itens = resources;
                EditorUtility.SetDirty(item);
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Resource manager/Create Resources Folder")]
        private static void CreateTranslationFolder() {
            string path = Path.Combine(Application.dataPath, "Resources");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }

        [InitializeOnLoadMethod]
        private static void Editor_Refresh() {
            CobilasBuildProcessor.EventOnPreprocessBuild += (pp, br) => {
                if (pp == CobilasEditorProcessor.PriorityProcessor.Middle)
                    Refresh();
            };
        }

        private static IEnumerator<Object> GetResourceObjects() {
            Object[] ass = UEResources.LoadAll("");
            for (int I = 0; I < ArrayManipulation.ArrayLength(ass); I++)
                if (ass[I].GetType() != typeof(ResourceManager))
                    yield return ass[I];
        }

        private static ResourceItem[] CreateResourceItemList(IEnumerator<Object> enumerator) {
            ResourceItem[] res = null;
            while (enumerator.MoveNext()) {
                string path = AssetDatabase.GetAssetPath(enumerator.Current);
                path = path.Remove(0, path.IndexOf("/Resources") + 1);
                ArrayManipulation.Add(new ResourceItem(enumerator.Current, Path.GetDirectoryName(path).Replace('\\', '/')), ref res);
            }
            return res;
        }

        private void UnloadResourceList() {
            for (int I = 0; I < ArrayManipulation.ArrayLength(itens); I++)
                itens[I].Dispose();
            ArrayManipulation.ClearArraySafe(ref itens);
        }
#endif
        public static void LoadAssetBundle(string folderPath) {
            AssetBundle bundle = AssetBundle.LoadFromFile(UnityPath.Combine(folderPath, UnityPath.GetFileName(folderPath)));
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] subdirs = manifest.GetAllAssetBundles();
            CRC crc = GetContainers();
            for (int J = 0; J < ArrayManipulation.ArrayLength(subdirs); J++) {
                Object[] objs = AssetBundle.LoadFromFile(UnityPath.Combine(folderPath, subdirs[J])).LoadAllAssets();
                for (int I = 0; I < ArrayManipulation.ArrayLength(objs); I++)
                    foreach (var containers in crc)
                        ArrayManipulation.Add(new ResourceItem(objs[I], UnityPath.GetDirectoryName(subdirs[J])), 
                            ref containers.itens);
            }
        }

        public static void LoadAssetBundle(params string[] folderPaths) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(folderPaths); I++)
                LoadAssetBundle(folderPaths[I]);
        }

        public static bool ContainsResourceItem(string name) {
            foreach (var item in GetContainers()) {
                for (int I = 0; I < ArrayManipulation.ArrayLength(item.itens); I++)
                    if (item.itens[I].Name == name)
                        return true;
                break;
            }
            return false;
        }

        public static Object[] GetAllObject() {
            Object[] res = null;
            foreach (var item in GetContainers()) {
                res = new Object[ArrayManipulation.ArrayLength(item.itens)];
                for (int I = 0; I < res.Length; I++)
                    res[I] = item.itens[I].Item;
                break;
            }
            return res;
        }

        public static T[] GetAllSpecificObject<T>() where T : Object {
            T[] res = null;
            foreach (var item in GetContainers()) {
                for (int I = 0; I < ArrayManipulation.ArrayLength(item.itens); I++)
                    if (item.itens[I] == typeof(T))
                        ArrayManipulation.Add((T)item.itens[I].Item, ref res);
                break;
            }
            return res;
        }

        public static Object[] GetAllObjectInFolder(string relativeFolder) {
            Object[] res = null;
            foreach (var item in GetContainers()) {
                for (int I = 0; I < ArrayManipulation.ArrayLength(item.itens); I++)
                    if (item.itens[I] == relativeFolder)
                        ArrayManipulation.Add(item.itens[I].Item, ref res);
                break;
            }
            return res;
        }

        public static T[] GetAllSpecificObjectInFolder<T>(string relativeFolder) where T : Object {
            T[] res = null;
            foreach (var item in GetContainers()) {
                for (int I = 0; I < ArrayManipulation.ArrayLength(item.itens); I++)
                    if (item.itens[I] == relativeFolder && item.itens[I] == typeof(T))
                        ArrayManipulation.Add((T)item.itens[I].Item, ref res);
                break;
            }
            return res;
        }

        public static Object GetObject(string name) {
            foreach (var item in GetContainers()) {
                for (int I = 0; I < ArrayManipulation.ArrayLength(item.itens); I++)
                    if (item.itens[I] == name)
                        return item.itens[I].Item;
                break;
            }
            return null;
        }

        public static T GetObject<T>(string name) where T : Object {
            foreach (var item in GetContainers()) {
                for (int I = 0; I < ArrayManipulation.ArrayLength(item.itens); I++)
                    if (item.itens[I] == name && item.itens[I] == typeof(T))
                        return (T)item.itens[I].Item;
                break;
            }
            return (T)null;
        }

        public static Cubemap GetCubemap(string name)
            => GetTexture<Cubemap>(name);

        public static TextAsset GetTextAsset(string name)
            => GetObject<TextAsset>(name);

        public static Texture GetTexture(string name)
            => GetObject<Texture>(name);

        public static T GetTexture<T>(string name) where T : Texture
            => GetObject<T>(name);

        public static ScriptableObject GetScriptableObject(string name)
            => GetObject<ScriptableObject>(name);

        public static T GetScriptableObject<T>(string name) where T : ScriptableObject
            => GetObject<T>(name);

        public static Sprite GetSprite(string name)
            => GetObject<Sprite>(name);

        public static AudioClip GetAudioClip(string name)
            => GetObject<AudioClip>(name);

        public static VideoClip GetVideoClip(string name)
            => GetObject<VideoClip>(name);

        public static GameObject GetGameObject(string name)
            => GetObject<GameObject>(name);

        public static T GetComponentInGameObject<T>(string name) where T : Component
            => GetGameObject(name).GetComponent<T>();

        private static CRC GetContainers() => new CRC(UEResources.LoadAll<ResourceManager>(""));
    }
}
