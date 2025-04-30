using System.Linq;
using UnityEditor;
using UnityEngine;

public class VertexColorBakingLogic
{
	public static void ClearVertexColorData(Transform root)
	{
		if (root == null) return;

		var targetObjects = root.GetComponentsInChildren<Transform>();

		for (int i = 0; i < targetObjects.Length; i++)
		{
			var targetObject = targetObjects[i];
			EditorUtility.DisplayProgressBar("Vertex Color Baking", $"Clearing data ({i}/{targetObjects.Length})", i / (float)targetObjects.Length);
			var bakedData = targetObject.GetComponent<BakedVertexColorData>();
			if (bakedData != null)
			{
				Undo.DestroyObjectImmediate(bakedData);
			}
		}
		EditorUtility.ClearProgressBar();
	}

	public static void BakeVertexColors(Transform root, VertexColorBakingSettings settings)
	{
		if (root == null) return;

		var targetObjects = root.GetComponentsInChildren<Transform>();

		ClearVertexColorData(root);

		EditorUtility.DisplayProgressBar("Vertex Color Baking", "Setting up", 0.0f);
		using var settingsApplier = new VertexColorObjectSettingsApplier(targetObjects);
		using var temporaryPlane = new TemporaryPlane(root);
		using var renderSettingsDisabler = new RenderSettingDisabler();
		using var prefabInstantiator = new VertexColorPrefabInstantiator(root);
		using var samplerCamera = new AmbientOcclusionSamplerCamera(settings, prefabInstantiator);
		var lights = GetLights();

		for (int i = 0; i < targetObjects.Length; i++)
		{
			var targetObject = targetObjects[i];
			EditorUtility.DisplayProgressBar("Vertex Color Baking", $"Coloring objects ({i}/{targetObjects.Length})", i / (float)targetObjects.Length);
			ColorObject(targetObject);
		}

		EditorUtility.ClearProgressBar();

		IVertexColorLight[] GetLights()
		{
			return targetObjects
						.Select(o => o.GetComponent<IVertexColorLight>())
						.Where(o => o != null)
						.ToArray();
		}

		void ColorObject(Transform targetObject)
		{

			var meshFilter = targetObject.GetComponent<MeshFilter>();
			if (meshFilter == null) return;

			var bakedData = Undo.AddComponent<BakedVertexColorData>(targetObject.gameObject);
			Undo.RecordObject(bakedData, "Set vertex color data");

			var localVerts = meshFilter.sharedMesh.vertices.ToArray();
			var localNormals = meshFilter.sharedMesh.normals.ToArray();
			var worldVerts = localVerts.Select(v => targetObject.TransformPoint(v)).ToArray();
			var worldNormals = localNormals.Select(n => Vector3.Normalize(targetObject.TransformDirection(n))).ToArray();
			var colors = new Color[worldVerts.Length];
			for (int i = 0; i < worldVerts.Length; i++)
			{
				colors[i] = GetColorForVert(worldVerts[i], worldNormals[i]);
			}

			bakedData.SetColors(colors);

			Color GetColorForVert(Vector3 worldPos, Vector3 normal)
			{
				var ao = samplerCamera.SampleAO(worldPos, normal);
				ao = settings.RawBakeValToP.Evaluate(ao);
				var color = Color.Lerp(settings.ShadowColor, settings.SkyColor, ao);
				foreach (var light in lights)
				{
					var distance = light.GetDistance(worldPos);
					var power = settings.PointLightFalloff.Evaluate(distance / light.Range);
					power *= light.Intensity;
					if (power < .01f) continue;
					var targetColor = light.Color * color;
					color = Color.Lerp(color, targetColor, power);
				}
				return color;
			}
		}
	}
	public static void RevertChildMeshFilters(Transform root)
	{
		if (root == null) return;

		var targetObjects = root.GetComponentsInChildren<Transform>();

		for (int i = 0; i < targetObjects.Length; i++)
		{
			var targetObject = targetObjects[i];
			var meshFilter = targetObject.GetComponent<MeshFilter>();
			if (meshFilter != null && PrefabUtility.IsPartOfPrefabInstance(meshFilter))
			{
				// This code is just not working :(
				PrefabUtility.RevertObjectOverride(meshFilter, InteractionMode.AutomatedAction);
				PrefabUtility.RecordPrefabInstancePropertyModifications(meshFilter);
			}
		}
	}
}