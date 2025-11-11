using System;
using System.IO;

public sealed class TemporaryFolder : IDisposable
{
	public string Path { get; }

	public TemporaryFolder()
	{
		Path = System.IO.Path.Combine(
			System.IO.Path.GetTempPath(),
			"TempYingFolder_" + Guid.NewGuid().ToString("N"));

		Directory.CreateDirectory(Path);
	}

	public void Dispose()
	{
		if (Directory.Exists(Path))
		{
			Directory.Delete(Path, true);
		}
	}
}