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

		var iconFullPath = modDefinition.Icon.GetFullAssetPath();

		var existingSteamId = modDefinition.SteamWorkshopId;

		var publishJob = existingSteamId == 0 ? Steamworks.Ugc.Editor.NewCommunityFile : new Steamworks.Ugc.Editor(existingSteamId);

		publishJob = publishJob
			.WithTitle(modDefinition.Title)
			.WithDescription(modDefinition.ShortDescription)
			.WithContent(contentFolder)
			.WithTag("Presets")
			.WithPreviewFile(iconFullPath);

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
}
