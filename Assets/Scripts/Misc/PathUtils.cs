using System.IO;

public static class PathUtils
{
	public static string EnsureDirectoryExists(string folderPath)
	{
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		return folderPath;
	}
}
