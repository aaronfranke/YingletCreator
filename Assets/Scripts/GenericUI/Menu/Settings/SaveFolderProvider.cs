using System;
using System.IO;
using UnityEngine;

public interface ISaveFolderProvider
{
	string GameRootFolderPath { get; }
}
public class SaveFolderProvider : MonoBehaviour, ISaveFolderProvider
{
	string _gameRootFolderPath;
	public string GameRootFolderPath
	{
		get
		{
			if (_gameRootFolderPath == null)
			{
				string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				_gameRootFolderPath = Path.Combine(documentsPath, "My Games", Application.productName);
				if (!Directory.Exists(_gameRootFolderPath))
				{
					Directory.CreateDirectory(_gameRootFolderPath);
				}
			}
			return _gameRootFolderPath;
		}
	}
}
