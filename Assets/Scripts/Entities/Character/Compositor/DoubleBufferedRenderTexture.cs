using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Render textures can't natively be used for read and write at the same time
/// The solution is to use two render textures and swap between them
/// Unity provides something for this in the form of CustomRenderTexture.doubleBuffered,
/// but it doesn't give me enough control:
/// I want to be able to clean up just the remaining render texture at the end
/// </summary>
public sealed class DoubleBufferedRenderTexture
{
	RenderTexture _upToDate;
	RenderTexture _backup;

	public DoubleBufferedRenderTexture(int textureSize)
	{
		_upToDate = CreateRT();
		_backup = CreateRT();

		RenderTexture CreateRT()
		{
			var rt = new RenderTexture(textureSize, textureSize, 0);
			rt.Create();

			return rt;
		}
	}

	public void Blit(Material mat)
	{
		Assert.IsNotNull(_backup);

		Graphics.Blit(_upToDate, _backup, mat);
		Swap();
	}

	public RenderTexture Finalize()
	{
		if (_backup != null)
		{
			_backup.Release();
			Object.Destroy(_backup);
			_backup = null;
		}

		return _upToDate;
	}

	void Swap()
	{
		RenderTexture temp = _upToDate;
		_upToDate = _backup;
		_backup = temp;
	}
}
