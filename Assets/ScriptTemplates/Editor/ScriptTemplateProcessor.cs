using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ScriptTemplates.Editor
{
    public class ScriptTemplateProcessor : UnityEditor.AssetModificationProcessor
    {
        private static string _SettingFilePath = "Assets/ScriptTemplates/Editor/ScriptTemplateSettings.asset";

        public static void OnWillCreateAsset(string assetMetaFilePath)
        {
            string assetFilePath = assetMetaFilePath.Replace(".meta", "");

            string extension = GetExtension(assetFilePath);
            if (!extension.Equals(".cs"))
            {
                return;
            }

            ScriptTemplateSettings settings = GetScriptTemplateSettings();

            string fullPath = GetProjectPath() + assetFilePath;
            string content = File.ReadAllText(fullPath);

            content = content.Replace("#ORGANIZATION#", settings.Organization);
            content = content.Replace("#LICENSE#", settings.License);

            List<string> folderNames = assetFilePath.Split('/').ToList();
            foreach (string ignoreFolderName in settings.IgnoredFolderNamesToGenerateNamespace)
            {
                folderNames.Remove(ignoreFolderName);
            }
            folderNames.RemoveAt(folderNames.Count - 1);

            string joinedFolderName = string.Join(".", folderNames.ToArray());
            content = content.Replace("#NAMESPACE#", joinedFolderName);

            File.WriteAllText(fullPath, content);
            AssetDatabase.Refresh();
        }

        public static string GetExtension(string filePath)
        {
            int lastIndex = filePath.LastIndexOf(".");
            return (lastIndex < 0) ? "" : filePath.Substring(lastIndex);
        }

        public static string GetProjectPath()
        {
            int lastIndex = Application.dataPath.LastIndexOf("Assets");
            return Application.dataPath.Substring(0, lastIndex);
        }

        public static ScriptTemplateSettings GetScriptTemplateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<ScriptTemplateSettings>(_SettingFilePath);

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ScriptTemplateSettings>();
                AssetDatabase.CreateAsset(settings, _SettingFilePath);
            }

            return settings;
        }
    }
}
