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
	private Camera _mainCamera;

	float _lastPicTime = 0;

	public event Action PictureTaken;

	private void Start()
	{
		_locationProvider = this.GetComponentInParent<ICustomizationSaveFolderProvider>();
		_visibilityControl = this.GetComponentInParent<ICharacterCreatorVisibilityControl>();
		_mainCamera = this.GetComponentInChildren<Camera>();
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
		RenderTexture rt = new RenderTexture(resolution.x, resolution.y, 24, RenderTextureFormat.ARGB32);
		rt.Create();

		// Create a new camera since the main one has a bunch of post-processing junk which messes up the background transparency(?)
		// Not sure that was actually what was happening, but this seems to work
		using (var camera = new TemporaryCamera(_mainCamera, rt))
		{
			camera.Render();
		}

		RenderTexture.active = rt;
		Texture2D image = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, false);
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

	sealed class TemporaryCamera : IDisposable
	{
		private readonly GameObject _cameraGo;
		private readonly Camera _camera;

		public TemporaryCamera(Camera mainCamera, RenderTexture rt)
		{
			_cameraGo = new GameObject("SnapshotterCamera");
			_camera = _cameraGo.AddComponent<Camera>();
			_camera.transform.position = mainCamera.transform.position;
			_camera.transform.rotation = mainCamera.transform.rotation;
			_camera.cullingMask = mainCamera.cullingMask;
			_camera.nearClipPlane = mainCamera.nearClipPlane;
			_camera.fieldOfView = mainCamera.fieldOfView;
			_camera.clearFlags = CameraClearFlags.Color;
			_camera.backgroundColor = Color.clear; // Fully transparent
			_camera.targetTexture = rt;
		}
		public void Render()
		{
			_camera.Render();
		}
		public void Dispose()
		{
			Destroy(_cameraGo);
		}

	}

}
