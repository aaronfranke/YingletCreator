using System;
using System.IO;
using UnityEngine;

public interface ISaveFolderProvider
{
	string GameRootFolderPath { get; }
	string ModsFolderPath { get; }
}
public class SaveFolderProvider : MonoBehaviour, ISaveFolderProvider
{
	private void Awake()
	{
		string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		var fullPath = Path.Combine(documentsPath, "My Games", Application.productName);
		GameRootFolderPath = PathUtils.EnsureDirectoryExists(fullPath);
		ModsFolderPath = PathUtils.EnsureDirectoryExists(Path.Combine(fullPath, "Mods"));
	}
	public string GameRootFolderPath { get; private set; }
	public string ModsFolderPath { get; private set; }
}
