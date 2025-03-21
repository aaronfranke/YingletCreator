using System.IO;
using UnityEditor;
using UnityEngine;

public class UpdateEyeAssets
{
	const string EYE_FOLDER_PATH = "Assets/Art/Materials/Entities/Yinglet/Eyes/RawTextures";
	[MenuItem("Custom/Update Eye Assets")]
	static void Apply()
	{
		var eyeFolders = Directory.GetDirectories(EYE_FOLDER_PATH);
		foreach (var eyeFolder in eyeFolders)
		{
			Debug.Log(eyeFolder);
		}
	}
}
