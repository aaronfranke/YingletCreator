using Character.Creator;
using Character.Data;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Snapshotter
{
	public sealed class SnapshotterGatherToggleIcons
	{
		const string ReferencesRelativePath = "Assets/Scripts/Entities/Snapshotter/SnapshotterReferences_ToggleIcons.asset";
		const string CameraPosRelativePath = "Assets/Scripts/Entities/Snapshotter/CameraPositions/_Default.asset";
		public const string OutputPath = "Assets/Scripts/Entities/Snapshotter/Generated/ToggleIcons.png";
		const string PresetPath = "Assets/Scripts/Entities/Snapshotter/SnapshotYing.yingsave";

		[MenuItem("Custom/Snapshotter/Generate Toggle Icons")]
		public static void TestSnapshotter()
		{
			if (!EditorApplication.isPlaying)
			{
				Debug.LogError("Snapshotting logic can only be ran while playing");
				return;
			}

			var toggles = GetAllToggles();
			Debug.Log($"Generating icons for {toggles.Length} toggles");
			var references = AssetDatabase.LoadAssetAtPath<SnapshotterReferences>(ReferencesRelativePath);
			int totalTexSize = CalculateTotalTextureSize(references.SizeInPixels, toggles.Length);

			SnapshotsToTexture(references, toggles, totalTexSize);
			SpliceSpriteSheet(references, toggles, totalTexSize);
			ApplySpriteSheetToToggles(toggles);
		}

		static void SnapshotsToTexture(SnapshotterReferences references, CharacterToggleId[] toggles, int totalTexSize)
		{
			var camPos = AssetDatabase.LoadAssetAtPath<SnapshotterCameraPosition>(CameraPosRelativePath);

			string text = File.ReadAllText(PresetPath);
			var serializedData = JsonUtility.FromJson<SerializableCustomizationData>(text);

			Debug.Log($"Texture size required: {totalTexSize}x{totalTexSize}");


			Texture2D tex = new Texture2D(totalTexSize, totalTexSize, TextureFormat.RGBA32, false);
			MakeTransparent(tex);

			for (int i = 0; i < toggles.Length; i++)
			{
				var toggle = toggles[i];
				EditorUtility.DisplayProgressBar("Snapshot Toggle Icon Previews", $"Generating icon ({i}/{toggles.Length}) - ${toggle.DisplayName}", i / (float)toggles.Length);

				var observableData = new ObservableCustomizationData(serializedData);
				if (!observableData.ToggleData.GetToggle(toggle)) // Accounts for the defaults
				{
					observableData.ToggleData.FlipToggle(toggle);
				}
				var sParams = new SnapshotterParams(camPos, observableData);
				var rt = SnapshotterUtils.Snapshot(references, sParams);

				// Create Texture2D and read pixels
				RenderTexture.active = rt;
				var offset = GetOffsetFromIndex(i, references.SizeInPixels, totalTexSize);
				tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), offset.x, offset.y);
				tex.Apply();
				RenderTexture.active = null;
				GameObject.DestroyImmediate(rt);
			}
			EditorUtility.ClearProgressBar();

			// Encode to PNG
			byte[] pngData = tex.EncodeToPNG();
			File.WriteAllBytes(OutputPath, pngData);
			Debug.Log("Saved PNG to: " + OutputPath);

			// Refresh AssetDatabase to show the new file
			AssetDatabase.Refresh();

			// Cleanup
			GameObject.DestroyImmediate(tex);
		}

		static void SpliceSpriteSheet(SnapshotterReferences references, CharacterToggleId[] toggles, int totalTexSize)
		{
			SpriteRect[] spriteRects = new SpriteRect[toggles.Length];
			int index = 0;

			for (int i = 0; i < toggles.Length; i++)
			{
				var toggle = toggles[i];
				var offset = GetOffsetFromIndex(i, references.SizeInPixels, totalTexSize);

				SpriteRect meta = new SpriteRect();
				meta.rect = new Rect(offset.x, offset.y, references.SizeInPixels, references.SizeInPixels); // note: origin is bottom-left
				meta.name = toggle.name;
				meta.pivot = new Vector2(0.5f, 0.5f); // center pivot
				meta.spriteID = GenerateDeterministicSpriteId(toggle.UniqueAssetID);
				spriteRects[index++] = meta;
			}

			TextureImporter importer = AssetImporter.GetAtPath(OutputPath) as TextureImporter;

			var factory = new SpriteDataProviderFactories();
			factory.Init();
			var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
			dataProvider.InitSpriteEditorDataProvider();
			dataProvider.SetSpriteRects(spriteRects);
			dataProvider.Apply();
			AssetDatabase.ImportAsset(OutputPath, ImportAssetOptions.ForceUpdate);
		}

		private static void ApplySpriteSheetToToggles(CharacterToggleId[] toggles)
		{
			string spriteSheetPath = OutputPath;
			var sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath)
				.Select(s => s as Sprite)
				.Where(s => s != null)
				.ToArray();
			Debug.Assert(toggles.Length == sprites.Length);
			for (int i = 0; i < toggles.Length; i++)
			{
				var toggle = toggles[i];
				var sprite = sprites[i];
				toggle.Preview.SetSprite(sprite);
				EditorUtility.SetDirty(toggle);
			}
			AssetDatabase.SaveAssets();
		}

		static void MakeTransparent(Texture2D tex)
		{
			Color32[] pixels = new Color32[tex.width * tex.width];
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = new Color32(0, 0, 0, 0);
			}

			tex.SetPixels32(pixels);
			tex.Apply();
		}

		static CharacterToggleId[] GetAllToggles()
		{
			return Resources.LoadAll<CharacterToggleId>(nameof(CharacterToggleId));

		}

		static int CalculateTotalTextureSize(int cellSize, int itemCount)
		{
			int spaceNeeded = cellSize * cellSize * itemCount;

			int testSize = 256;
			while (spaceNeeded > testSize * testSize)
			{
				testSize *= 2;
			}
			return testSize;
		}

		static Vector2Int GetOffsetFromIndex(int index, int cellSize, int totalSize)
		{
			var totalRows = totalSize / cellSize;
			int xOffset = (index % totalRows) * cellSize;
			int yOffset = (index / totalRows) * cellSize;
			return new Vector2Int(xOffset, yOffset);
		}

		public static GUID GenerateDeterministicSpriteId(string uniqueInput)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(uniqueInput));
				Guid guid = new Guid(hashBytes);
				return new GUID(guid.ToString("N"));
			}
		}
	}
}