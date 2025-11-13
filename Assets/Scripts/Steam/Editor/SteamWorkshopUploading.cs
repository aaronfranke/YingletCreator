using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class SteamWorkshopUploading
{
	public static async Task UploadModAsync(ModDefinition modDefinition, string contentFolder)
	{
		if (!Steamworks.SteamClient.IsValid)
		{
			try
			{
				Steamworks.SteamClient.Init(SteamManager.SteamAppId, true);
			}
			catch (System.Exception e)
			{
				Debug.LogError("Steamworks initialization failed - " + e);
				throw e;
			}
		}


		var existingSteamId = modDefinition.SteamWorkshopId;

		var publishJob = existingSteamId == 0 ? Steamworks.Ugc.Editor.NewCommunityFile : new Steamworks.Ugc.Editor(existingSteamId);

		publishJob = publishJob
			.WithTitle(modDefinition.Title)
			.WithDescription(modDefinition.ShortDescription)
			.WithContent(contentFolder);

		var counts = modDefinition.CountAssetTypes();
		if (counts.Presets > 0)
		{
			publishJob = publishJob.WithTag("Presets");
		}
		if (counts.Toggles > 0)
		{
			publishJob = publishJob.WithTag("Toggles");
		}
		if (counts.Poses > 0)
		{
			publishJob = publishJob.WithTag("Poses");
		}

		using var tempIconFolder = new TemporaryFolder();
		if (modDefinition.Icon != null)
		{
			var iconFullPath = modDefinition.Icon.GetFullAssetPath();
			var soloSprite = ExportSpriteToPNG(modDefinition.Icon, tempIconFolder.Path);
			publishJob = publishJob.WithPreviewFile(iconFullPath);
		}

		var publishResult = await publishJob.SubmitAsync();

		var returnedSteamId = publishResult.FileId.Value;
		if (returnedSteamId != 0)
		{
			modDefinition.SteamWorkshopId = returnedSteamId; ;
			EditorUtility.SetDirty(modDefinition);
			AssetDatabase.SaveAssets();

			Application.OpenURL($"https://steamcommunity.com/sharedfiles/filedetails/?id={returnedSteamId}");
		}
		if (!publishResult.Success)
		{
			// If you're getting InvalidParam like I was, you might need to delete the vdf files out of C:\Program Files (x86)\Steam\appcache and restart steam
			throw new System.Exception($"Failed to publish to SteamWorks: {publishResult.Result.ToString()}");
		}
	}


	// We can't provide the sprite alone since there might be other stuff; put it in a temp folder
	public static string ExportSpriteToPNG(Sprite sprite, string outputFolder)
	{
		string assetPath = AssetDatabase.GetAssetPath(sprite.texture);
		var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
		bool wasReadable = false;
		if (importer != null)
		{
			wasReadable = importer.isReadable;
			if (!importer.isReadable)
			{
				importer.isReadable = true;
				importer.SaveAndReimport();
			}
		}
		var rect = sprite.rect;
		var texture = sprite.texture;

		// Crop texture to sprite rect
		var pixels = texture.GetPixels(
			(int)rect.x,
			(int)rect.y,
			(int)rect.width,
			(int)rect.height
		);

		var newTex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
		newTex.SetPixels(pixels);
		newTex.Apply();

		string filePath = Path.Combine(outputFolder, sprite.name + ".png");
		byte[] pngData = newTex.EncodeToPNG();

		File.WriteAllBytes(filePath, pngData);
		Object.DestroyImmediate(newTex);

		if (wasReadable != importer.isReadable)
		{
			importer.isReadable = wasReadable;
			importer.SaveAndReimport();
		}

		return filePath;
	}
}
