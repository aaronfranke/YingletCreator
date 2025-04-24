using Character.Creator;
using Character.Data;
using System.IO;
using UnityEditor;
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
			var camPos = AssetDatabase.LoadAssetAtPath<SnapshotterCameraPosition>(CameraPosRelativePath);

			string text = File.ReadAllText(PresetPath);
			var serializedData = JsonUtility.FromJson<SerializableCustomizationData>(text);

			int totalTexSize = CalculateTotalTextureSize(references.SizeInPixels, toggles.Length);
			Debug.Log($"Texture size required: {totalTexSize}x{totalTexSize}");


			Texture2D tex = new Texture2D(totalTexSize, totalTexSize, TextureFormat.RGBA32, false);
			MakeTransparent(tex);

			var totalRows = totalTexSize / references.SizeInPixels;
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
				int xOffset = (i % totalRows) * references.SizeInPixels;
				int yOffset = (i / totalRows) * references.SizeInPixels;
				tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), xOffset, yOffset);
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
	}
}