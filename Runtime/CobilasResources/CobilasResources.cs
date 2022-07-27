using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Video;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;
using Cobilas.Unity.Management.RuntimeInitialize;
#if UNITY_EDITOR
using UnityEditor;
using Cobilas.Unity.Management.Build;
#endif
using UEObject = UnityEngine.Object;
using UEResources = UnityEngine.Resources;

namespace Cobilas.Unity.Management.Resources {
    public static class CobilasResources {
        private static CRItem[] Prefabs;

        [CRIOLM_BeforeSceneLoad(CRIOLMPriority.High)]
        private static void Init() {
            TextAsset text = UEResources.Load<TextAsset>("ResourcesPaths");
            if (text != (TextAsset)null)
                using (XmlReader reader = XmlReader.Create(new StringReader(text.text))) {
                    using (ElementTag elemento = reader.GetElementTag()) {
                        ElementTag[] tags = elemento.GetElementTags();
                        for (int I = 0; I < ArrayManipulation.ArrayLength(tags); I++) {
                            string RelativePath = GetRelativeAssetPath(tags[I].GetElementTag("RelativePath").Value.ValueToString);
                            string VType = tags[I].GetElementTag("Type").Value.ValueToString;

                            ArrayManipulation.Add(new CRItem(RelativePath, VType), ref Prefabs);
                        }
                    }
                }

            Application.quitting += () => {
#if !UNITY_EDITOR
                DescarregarAtivo(Prefabs);
#endif
                ArrayManipulation.ClearArraySafe(ref Prefabs);
            };
        }

        private static string GetRelativeAssetPath(string path) {
            path = path.Remove(0, path.IndexOf("/Resources") + 1);
            return path.Contains('.') ? path.Remove(path.IndexOf('.')) : path;
        }

#if UNITY_EDITOR
        //private static string folderPathResources => Path.Combine(Application.dataPath, "Resources").Replace('\\', '/');
        [InitializeOnLoadMethod]
        private static void InitEditor() {
            CobilasBuildProcessor.EventOnPreprocessBuild += (p, b)=> {
                if (p == CobilasEditorProcessor.PriorityProcessor.Low)
                    RefreshResourcesPaths();
            };
            CobilasEditorProcessor.playModeStateChanged += (p, pm) => {
                if (p == CobilasEditorProcessor.PriorityProcessor.Low)
                    if (pm == PlayModeStateChange.EnteredPlayMode)
                        RefreshResourcesPaths();
            };
            DescarregarAtivo(Prefabs);
            Init();
        }

        [MenuItem("Tools/Cobilas/Refresh Resources paths")]
        private static void RefreshResourcesPaths() {
            MonoBehaviour.print($"Refresh resources paths[{DateTime.Now}]");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\n";
            using (FileStream stream = new FileStream($"{CobilasPaths.ResourcesPath}/ResourcesPaths.xml", FileMode.Create, FileAccess.Write, FileShare.Write)) {
                using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
                    ElementTag Raiz = new ElementTag("ResourcesPaths");
                    foreach (var item in GetRelativeFilesPtah()) {
                        Raiz.Add(new ElementTag("Item",
                            new ElementTag("RelativePath", item.Key),
                            new ElementTag("Type", item.Value.AssemblyQualifiedName)
                        ));
                    }
                    writer.WriteElementTag(Raiz);
                }
            }
            AssetDatabase.Refresh();
        }

        private static KeyValuePair<string, Type>[] GetRelativeFilesPtah() {
            KeyValuePair<string, Type>[] files = null;
            UEObject[] objtemp = UEResources.LoadAll("");
            for (int I = 0; I < ArrayManipulation.ArrayLength(objtemp); I++) {
                if (objtemp[I].name != "ResourcesPaths")
                    ArrayManipulation.Add(new KeyValuePair<string, Type>(
                        AssetDatabase.GetAssetPath(objtemp[I]),
                        objtemp[I].GetType()
                        ), ref files);

            }
            return files;
        }
#endif
        public static string[] GetAllResourcesPaths() {
            string[] res = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Prefabs); I++)
                ArrayManipulation.Add(Prefabs[I].RelativePath, ref res);
            return res;
        }

        public static UEObject[] GetAllObject() {
            UEObject[] Res = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Prefabs); I++)
                ArrayManipulation.Add(LoadObject(Prefabs[I]), ref Res);
            return Res;
        }

        public static T[] GetAllSpecificObject<T>() where T : UEObject {
            T[] Res = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Prefabs); I++)
                if (Prefabs[I] == typeof(T))
                    ArrayManipulation.Add(LoadObject<T>(Prefabs[I]), ref Res);
            return Res;
        }

        public static UEObject[] GetAllObjectInFolder(string relativeFolder) {
            UEObject[] Res = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Prefabs); I++)
                if (Prefabs[I] == relativeFolder)
                    ArrayManipulation.Add(LoadObject(Prefabs[I]), ref Res);
            return Res;
        }

        public static T[] GetAllSpecificObjectInFolder<T>(string relativeFolder) where T : UEObject {
            T[] Res = null;
            for (int I = 0; I < ArrayManipulation.ArrayLength(Prefabs); I++)
                if (Prefabs[I] == typeof(T) && Prefabs[I] == relativeFolder)
                    ArrayManipulation.Add(LoadObject<T>(Prefabs[I]), ref Res);
            return Res;
        }

        public static T GetObject<T>(string name) where T : UEObject {
            for (int I = 0; I < ArrayManipulation.ArrayLength(Prefabs); I++)
                if (Prefabs[I] == name && Prefabs[I] == typeof(T))
                    return LoadObject<T>(Prefabs[I]);
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

        public static Sprite GetSprite(string name)
            => GetObject<Sprite>(name);

        public static AudioClip GetAudioClip(string name)
            => GetObject<AudioClip>(name);

        public static VideoClip GetVideoClip(string name)
            => GetObject<VideoClip>(name);

        public static GameObject GetGameObject(string name)
            => GetObject<GameObject>(name);

        public static T GetComponentInGameObject<T>(string name)
            => GetGameObject(name).GetComponent<T>();

        private static void DescarregarAtivo(CRItem[] list) {
            for (int I = 0; I < ArrayManipulation.ArrayLength(list); I++)
                list[I].Dispose();
        }

        private static UEObject LoadObject(CRItem item) {
            string path = item.RelativePath.Remove(0, item.RelativePath.IndexOf('/') + 1);
            return UEResources.Load(path);
        }

        private static T LoadObject<T>(CRItem item) where T : UEObject {
            string path = item.RelativePath.Remove(0, item.RelativePath.IndexOf('/') + 1);
            return UEResources.Load<T>(path);
        }
    }
}
