using System;
using System.IO;
using UnityEngine;

namespace Character.Creator
{
    public interface ICustomizationSaveFolderProvider
    {
        string FolderRoot { get; }
    }

    public class CustomizationSaveFolderProvider : MonoBehaviour, ICustomizationSaveFolderProvider
    {
        string _folderRoot;
        public string FolderRoot
        {
            get
            {
                if (_folderRoot == null)
                {
                    _folderRoot = GetFolderRoot();
                }
                return _folderRoot;
            }
        }

        static string GetFolderRoot()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = Path.Combine(documentsPath, "My Games", Application.productName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }
    }
}