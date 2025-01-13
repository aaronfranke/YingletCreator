using UnityEngine;

[ExecuteInEditMode]
public class BakedVertexColorData : MonoBehaviour
{
	[SerializeField][HideInInspector] Color[] _colors = new Color[0];
	[SerializeField][HideInInspector] Mesh _originalMesh = null;

	void OnEnable()
	{
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		if (_originalMesh == null) return; // This was likely JUST instantiated by the editor
		if (meshFilter.sharedMesh != _originalMesh) return; // The mesh has already been swapped off the original

		
		var coloredMesh = Mesh.Instantiate(meshFilter.sharedMesh);  //make a deep copy
		coloredMesh.colors = _colors;
		meshFilter.sharedMesh = coloredMesh;
	}

	void OnDisable()
	{
		if (_originalMesh == null) return;
		gameObject.GetComponent<MeshFilter>().sharedMesh = _originalMesh;
	}


#if UNITY_EDITOR
	public void SetColors(Color[] colors)
	{
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		_colors = colors;
		if (_originalMesh == null)
		{
			_originalMesh = meshFilter.sharedMesh;
		}
		OnEnable();
	}
#endif
}
