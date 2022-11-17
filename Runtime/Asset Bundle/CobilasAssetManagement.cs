using System.IO;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Utility;
using System.Collections.Generic;
using Cobilas.Unity.Management.Runtime;
//using UEResources = UnityEngine.Resources;

namespace Cobilas.Unity.Management.Resources.Asset {
    //Assets/Resources/Translation/PT-BR-TDS.alfbt
    public static class CobilasAssetManagement {
        //public const string camVersion = "1.7";
        //public const string camDefaultPathRes = "camDefaultPath";
        private static List<AssetBundle> bundles = null;

#if UNITY_EDITOR
        public static string camFolder => UnityPath.Combine(UnityPath.ResourcesPath, "camFolder");
        public static string camFile => UnityPath.Combine(camFolder, "camDefaultPath.txt");
#endif
        public static int AssetsCount => bundles == null ? 0 : bundles.Count;

        [StartBeforeSceneLoad]
        private static void Init()
            => Application.quitting += () => AssetBundle.UnloadAllAssetBundles(true);

        public static void LoadAssetBundle(string relativePath) {
            if (bundles == null) bundles = new List<AssetBundle>();
            string[] dirs = Directory.GetDirectories(Path.Combine(Application.streamingAssetsPath, relativePath));
            for (int I = 0; I < ArrayManipulation.ArrayLength(dirs); I++) {
                AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(dirs[I], Path.GetFileName(dirs[I])));
                AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                string[] man = manifest.GetAllAssetBundles();
                for (int J = 0; J < ArrayManipulation.ArrayLength(man); J++)
                    bundles.Add(AssetBundle.LoadFromFile(Path.Combine(dirs[I], man[J])));
            }
        }

        public static T[] LoadAllAssets<T>() where T : UnityEngine.Object {
            T[] Res = null;
            for (int I = 0; I < AssetsCount; I++)
                ArrayManipulation.Add(bundles[I].LoadAllAssets<T>(), ref Res);
            return Res;
        }

        public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object {
            for (int I = 0; I < AssetsCount; I++)
                if (bundles[I].Contains(assetName))
                    return bundles[I].LoadAsset<T>(assetName);
            return null;
        }
    }
}
