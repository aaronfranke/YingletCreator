using Character.Creator;
using Character.Creator.UI;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public interface ITakePictureEvents
{
	event Action PictureTaken;
}

public class TakePictureOnLeftClick : MonoBehaviour, ITakePictureEvents
{
	private ICustomizationSaveFolderProvider _locationProvider;
	private ICharacterCreatorVisibilityControl _visibilityControl;
	private Camera _camera;

	float _lastPicTime = 0;

	public event Action PictureTaken;

	private void Start()
	{
		_locationProvider = this.GetComponentInParent<ICustomizationSaveFolderProvider>();
		_visibilityControl = this.GetComponentInParent<ICharacterCreatorVisibilityControl>();
		_camera = this.GetComponentInChildren<Camera>();
	}

	private void Update()
	{
		// Early return if we're showing UI (aka not in photo mode)
		if (_visibilityControl.IsVisible.Val) return;

		// Don't let the user take too many pics
		if (Time.time < _lastPicTime + 1.5f) return;

		if (Input.GetMouseButtonDown(0))
		{
			StartCoroutine(TakePicture());
		}
	}

	IEnumerator TakePicture()
	{
		// Play the sound effects and stuff immediately
		PictureTaken?.Invoke();
		_lastPicTime = Time.time;
		yield return null;

		yield return new WaitForEndOfFrame();
		var screenResolution = Screen.currentResolution;
		var resolution = new Vector2Int(screenResolution.width, screenResolution.height);
		resolution *= 2; // Experimental resolution doubling
		RenderTexture rt = new RenderTexture(resolution.x, resolution.y, 24);
		rt.Create();

		var previousRt = _camera.targetTexture; ;
		using (var swapper = new CameraTargetSwapper(_camera, rt))
		{
			_camera.Render();
		}

		RenderTexture.active = rt;
		Texture2D image = new Texture2D(resolution.x, resolution.y, TextureFormat.RGB24, false);
		image.ReadPixels(new Rect(0, 0, resolution.x, resolution.y), 0, 0);
		image.Apply();
		RenderTexture.active = null;

		rt.Release();
		Destroy(rt);

		SavePicture(image);
	}

	void SavePicture(Texture2D image)
	{
		// Save to desktop
		byte[] bytes = image.EncodeToPNG();
		Destroy(image);

		string fileName = $"Picture_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
		string fullPath = Path.Combine(_locationProvider.PhotoRoot, fileName);
		File.WriteAllBytes(fullPath, bytes);
	}

	sealed class CameraTargetSwapper : IDisposable
	{
		private Camera _camera;
		private RenderTexture _previousTarget;

		public CameraTargetSwapper(Camera camera, RenderTexture newTarget)
		{
			_camera = camera;
			_previousTarget = camera.targetTexture;
			_camera.targetTexture = newTarget;
		}
		public void Dispose()
		{
			_camera.targetTexture = _previousTarget;
		}
	}
}