﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptTemplates.Editor
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptTemplates/Create Settings", fileName = "ScriptTemplateSettings")]
    public class ScriptTemplateSettings : ScriptableObject
    {
        public string Organization = "Organization.";
        public string License = "MIT License";
        public List<string> IgnoredFolderNamesToGenerateNamespace = new List<string>()
        {
            "Assets",
            "Scripts",
        };
        public List<TextReplaceSetting> FolderNameTextReplaceSettings = new List<TextReplaceSetting>();
    }

    [Serializable]
    public class TextReplaceSetting
    {
        public string OldText;
        public string NewText;
    }
}