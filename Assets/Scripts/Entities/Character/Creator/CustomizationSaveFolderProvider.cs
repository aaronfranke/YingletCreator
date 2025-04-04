using System;
using System.IO;
using UnityEngine;

namespace Character.Creator
{
    public interface ICustomizationSaveFolderProvider
    {
        string CustomFolderRoot { get; }
    }

    public class CustomizationSaveFolderProvider : MonoBehaviour, ICustomizationSaveFolderProvider
    {
        string _customFolderRoot;
        public string CustomFolderRoot
        {
            get
            {
                if (_customFolderRoot == null)
                {
                    _customFolderRoot = GetCustomFolderRoot();
                }
                return _customFolderRoot;
            }
        }

        static string GetCustomFolderRoot()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = Path.Combine(documentsPath, "My Games", Application.productName, "CustomYings");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }
    }
}