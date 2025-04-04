using System;
using System.IO;
using UnityEngine;

namespace Character.Creator
{
    public interface ICustomizationSaveFolderProvider
    {
        string PresetFolderRoot { get; }
        string CustomFolderRoot { get; }
    }

    public class CustomizationSaveFolderProvider : MonoBehaviour, ICustomizationSaveFolderProvider
    {

        string _presetFolderRoot;
        public string PresetFolderRoot
        {
            get
            {
                if (_presetFolderRoot == null)
                {
                    string streamingAssetsPath = Application.streamingAssetsPath;
                    string folder = Path.Combine(streamingAssetsPath, "PresetYings");
                    _presetFolderRoot = folder;
                }
                return _presetFolderRoot;
            }
        }

        string _customFolderRoot;
        public string CustomFolderRoot
        {
            get
            {
                if (_customFolderRoot == null)
                {
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string folder = Path.Combine(documentsPath, "My Games", Application.productName, "CustomYings");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    _customFolderRoot = folder;
                }
                return _customFolderRoot;
            }
        }
    }
}