using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Cobilas.Collections;
using Cobilas.Unity.Utility;

namespace Cobilas.Unity.Editor.Management.Resources.Asset {
    public static class CobilasAssetManagementWin {

        private static string camFolderPath => CobilasPaths.Combine(
            CobilasPaths.GetDirectoryName(CobilasPaths.AssetsPath),
            "AssetBundle");
        private static string camBilder => CobilasPaths.Combine(CobilasPaths.AssetsPath, "AssetBundleBild");

        [InitializeOnLoadMethod]
        private static void Init() {
            if (!Directory.Exists(camBilder))
                Directory.CreateDirectory(camBilder);
        }

        [MenuItem("Tools/Cobilas/Asset management/Create AssetBundleBild Folder")]
        private static void CreateAssetBundleBildFolder() {
            if (!Directory.Exists(camBilder)) {
                Directory.CreateDirectory(camBilder);
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("Tools/Cobilas/Asset management/Assembly AssetBundle")]
        private static void AssemblyAssetBundle() {
            AssetsItemRef[] itemRefs = AssetsItemRef.GetAllAssetsItemRefs();
            DLCItemPath[] dLCs = null;

            for (int I = 0; I < ArrayManipulation.ArrayLength(itemRefs); I++) {
                string relativePath = AssetDatabase.GetAssetPath(itemRefs[I]);
                if (!relativePath.Contains("Assets/AssetBundleBild")) continue;
                string relativePath2 = CobilasPaths.GetDirectoryName(relativePath);

                DLCItemPath dLCtemp = DLCItemPath.GetDLCItemPath(
                    DLCItemPath.GetDLCName(AssetsItemPaths.RemoveAssetBundleBildPath(relativePath2)), dLCs);


                if (dLCtemp == null) {
                    dLCtemp = new DLCItemPath();
                    dLCtemp.name = DLCItemPath.GetDLCName(AssetsItemPaths.RemoveAssetBundleBildPath(relativePath2));
                    ArrayManipulation.Add(dLCtemp, ref dLCs);
                }

                AssetsItemPaths assetsItemtemp = AssetsItemPaths.GetAssetsItemPaths(AssetsItemPaths.RemoveAssetBundleBildPath(relativePath2), dLCtemp.paths);
                if (assetsItemtemp == null) {
                    assetsItemtemp = new AssetsItemPaths();
                    assetsItemtemp.assetBundleName = AssetsItemPaths.RemoveAssetBundleBildPath(relativePath2);
                    ArrayManipulation.Add(assetsItemtemp, ref dLCtemp.paths);
                }
                ArrayManipulation.Add(AssetDatabase.GetAssetPath(itemRefs[I].MyObject), ref assetsItemtemp.assetNames);
            }

            for (int I = 0; I < ArrayManipulation.ArrayLength(dLCs); I++) {
                AssetBundleBuild[] builds = null;
                string camdlcpath = null;
                for (int J = 0; J < ArrayManipulation.ArrayLength(dLCs[I].paths); J++) {
                    AssetBundleBuild build = new AssetBundleBuild();
                    build.assetBundleName = dLCs[I].paths[J].assetBundleName;
                    build.assetNames = dLCs[I].paths[J].assetNames;
                    ArrayManipulation.Add(build, ref builds);

                }
                camdlcpath = CobilasPaths.Combine(camFolderPath, $"CTOR_{dLCs[I].name}");

                ClearAssetBundle(camdlcpath);
                _ = BuildPipeline.BuildAssetBundles($"AssetBundle/CTOR_{dLCs[I].name}", builds, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            }
            MonoBehaviour.print($"Build AssetBundle[{DateTime.Now}]");
            AssetDatabase.Refresh();
        }

        private static void ClearAssetBundle(string path) {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private sealed class DLCItemPath {
            public string name;
            public AssetsItemPaths[] paths;

            public static string GetDLCName(string path) {
                int index = path.IndexOf('/');
                if (index == -1) return path;
                return path.Remove(index);
            }

            public static DLCItemPath GetDLCItemPath(string name, DLCItemPath[] paths) {
                for (int I = 0; I < ArrayManipulation.ArrayLength(paths); I++)
                    if (paths[I].name == name)
                        return paths[I];
                return null;
            }
        }

        private sealed class AssetsItemPaths {
            public string assetBundleName;
            public string[] assetNames;

            public static string RemoveAssetBundleBildPath(string path) {
                int index = path.IndexOf("/AssetBundleBild");
                path = path.Remove(0, index + 1);
                path = path.Remove(0, path.IndexOf('/') + 1);
                return path;
            }

            public static AssetsItemPaths GetAssetsItemPaths(string assetBundleName, AssetsItemPaths[] paths) {
                for (int I = 0; I < ArrayManipulation.ArrayLength(paths); I++)
                    if (paths[I].assetBundleName == assetBundleName)
                        return paths[I];
                return null;
            }
        }
    }
}