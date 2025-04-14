using UnityEngine;
using UnityEngine.UI;

public class SliderMeshEffect : BaseMeshEffect
{
	public override void ModifyMesh(VertexHelper vh)
	{
		if (!IsActive())
			return;

		UIVertex vertex = new UIVertex();

		for (int i = 0; i < vh.currentVertCount; i++)
		{
			vh.PopulateUIVertex(ref vertex, i);
			Vector2 newUV = new Vector2(vertex.position.x > 1 ? 1 : 0, 0);
			vertex.uv1 = newUV;

			vh.SetUIVertex(vertex, i);
		}
	}
}
