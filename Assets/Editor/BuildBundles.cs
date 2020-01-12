﻿#if UNITY_EDITOR
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace IllusionMods.KoikatuModdingTools
{
    /// <summary>
    /// Adds a menu item to build asset bundles
    /// </summary>
    public class MenuItems
    {
        [MenuItem("Assets/Build Mod")]
        internal static void BuildMod()
        {
            string projectPath = GetProjectPath();
            BuildSingleMod(projectPath);
        }

        [MenuItem("Assets/Build All Mods")]
        [MenuItem("Build/Build All Mods")]
        internal static void BuildMods()
        {
            var di = new DirectoryInfo(Constants.ModsPath);
            foreach (var file in di.GetFiles("manifest.xml", SearchOption.AllDirectories))
            {
                string projectPath = file.Directory.FullName;
                projectPath = projectPath.Substring(projectPath.IndexOf(Constants.ModsPath));
                BuildSingleMod(projectPath);
            }
        }

        /// <summary>
        /// Packs up a mod including its manifest.xml, list files, and asset bundles. Copies the mod to the user's install folder.
        /// </summary>
        /// <param name="projectPath">Path of the project containing the mod, manifest.xml should be in the root.</param>
        private static void BuildSingleMod(string projectPath)
        {
            string manifestPath = Path.Combine(projectPath, "manifest.xml");
            string makerListPath = Path.Combine(projectPath, @"List\Maker");
            string studioListPath = Path.Combine(projectPath, @"List\Studio");

            HashSet<string> modABs = new HashSet<string>();
            HashSet<string> makerListFiles = new HashSet<string>();
            HashSet<string> studioListFiles = new HashSet<string>();

            Debug.Log("Building zipmod...");
            if (!File.Exists(manifestPath))
            {
                Debug.Log("manifest.xml does not exist in the directory, mod creation aborted.");
                return;
            }

            //Read the manifest.xml
            XDocument manifestDocument = XDocument.Load(manifestPath);
            string modGUID = manifestDocument.Root.Element("guid").Value;
            string modName = "";
            string modVersion = "";
            string modAuthor = "";
            string modGame = "";
            if (manifestDocument.Root.Element("name") != null)
                modName = manifestDocument.Root.Element("name").Value;
            if (manifestDocument.Root.Element("version") != null)
                modVersion = manifestDocument.Root.Element("version").Value;
            if (manifestDocument.Root.Element("author") != null)
                modAuthor = manifestDocument.Root.Element("author").Value;
            if (manifestDocument.Root.Element("game") != null)
                modGame = manifestDocument.Root.Element("game").Value;

            if (modGame != "")
            {
                if (!Constants.GameNameList.Contains(modGame.ToLower().Replace("!", "")))
                {
                    Debug.Log("The manifest.xml lists a game other than Koikatsu, this mod will not be built.");
                    return;
                }
                else
                    modGame = "KK";
            }

            //Create a name for the .zipmod based on manifest.xml
            string zipFileName = "";
            if (modAuthor != "" && modName != "")
            {
                if (modAuthor != "")
                    zipFileName += "[" + modAuthor + "]";
                if (modGame != "")
                    zipFileName += "[" + modGame + "]";
                if (modName != "")
                    zipFileName += modName;
            }
            else
                zipFileName = modGUID;
            string zipFileNamePrexix = zipFileName;
            if (modVersion != "")
                zipFileName += " v" + modVersion;
            zipFileName += ".zipmod";
            string zipPath = Path.Combine("Build", zipFileName);

            //Find all the asset bundles for this mod
            foreach (var assetguid in AssetDatabase.FindAssets("", new string[] { projectPath }))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetguid);
                string modAB = AssetDatabase.GetImplicitAssetBundleName(assetPath);
                if (modAB != string.Empty)
                    modABs.Add(Path.Combine(Constants.BuildPath, modAB));
            }

            var di = new DirectoryInfo(makerListPath);
            if (di.Exists)
                foreach (var file in di.GetFiles("*.csv", SearchOption.AllDirectories))
                    makerListFiles.Add(file.FullName);

            di = new DirectoryInfo(studioListPath);
            if (di.Exists)
                foreach (var file in di.GetFiles("*.csv", SearchOption.AllDirectories))
                    studioListFiles.Add(file.FullName);

            if (makerListFiles.Count == 0 && studioListFiles.Count == 0)
                Debug.Log("No list files were found for this mod. If this mod is overriding vanilla assets, no list files are required. Any mod adding new content to maker or studio requires list files");

            //Build the zip file
            File.Delete(zipPath);
            ZipFile zipFile = new ZipFile(zipPath, Encoding.UTF8);
            zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.None;

            //Add the manifest
            zipFile.AddFile(manifestPath, "");

            //Add asset bundles
            foreach (var modAB in modABs)
            {
                string folderAB = modAB.Replace("/", @"\").Replace(@"Build\", "");
                folderAB = folderAB.Remove(folderAB.LastIndexOf(@"\")); //Remove the .unity3d filename
                zipFile.AddFile(modAB, folderAB);
            }

            //Add list files
            foreach (var listFile in makerListFiles)
                zipFile.AddFile(listFile, @"abdata\list\characustom\00\");

            zipFile.Save();
            zipFile.Dispose();

            if (Constants.CopyModToGameFolder)
            {
                var modsFolder = Path.Combine(Constants.KoikatsuInstallPath, "mods");
                var copyPath = Path.Combine(modsFolder, zipFileName);
                di = new DirectoryInfo(modsFolder);
                if (di.Exists)
                {
                    Debug.Log(zipFileNamePrexix);
                    foreach (var file in di.GetFiles("*.zipmod"))
                    {
                        Debug.Log(file.Name);
                        if (file.Name.StartsWith(zipFileNamePrexix))
                            file.Delete();
                    }
                    File.Copy(zipPath, copyPath);
                }
                else
                    Debug.Log("Mods folder not found, could not copy .zipmod files to game install.");
            }

            Debug.Log("Mod built sucessfully.");
        }

        /// <summary>
        /// Get the path of the currently selected folder.
        /// </summary>
        public static string GetProjectPath()
        {
            try
            {
                var projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
                var projectBrowser = projectBrowserType.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                var invokeMethod = projectBrowserType.GetMethod("GetActiveFolderPath", BindingFlags.NonPublic | BindingFlags.Instance);
                return (string)invokeMethod.Invoke(projectBrowser, new object[] { });
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Error while trying to get current project path.");
                Debug.LogWarning(exception.Message);
                return string.Empty;
            }
        }
    }
}
#endif