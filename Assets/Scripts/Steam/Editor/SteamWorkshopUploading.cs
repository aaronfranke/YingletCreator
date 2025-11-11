using System.Threading.Tasks;
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

		var publishResult = await Steamworks.Ugc.Editor.NewCommunityFile
			.WithTitle(modDefinition.Title)
			.WithDescription(modDefinition.ShortDescription)
			.WithContent(contentFolder)
			.WithTag("Presets")
			.WithPreviewFile(iconFullPath)
			//.WithPrivateVisibility()
			.SubmitAsync();

		var id = publishResult.FileId.Value;
		if (id != 0)
		{
			Application.OpenURL($"https://steamcommunity.com/sharedfiles/filedetails/?id={id}");
		}
		if (!publishResult.Success)
		{
			// If you're getting InvalidParam like I was, you might need to delete the vdf files out of C:\Program Files (x86)\Steam\appcache and restart steam
			throw new System.Exception($"Failed to publish to SteamWorks: {publishResult.Result.ToString()}");
		}
	}
}
