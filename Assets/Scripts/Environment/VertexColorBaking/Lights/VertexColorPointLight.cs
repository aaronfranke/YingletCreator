using UnityEngine;

public class VertexColorPointLight : MonoBehaviour, IVertexColorLight
{
    [ColorUsage(showAlpha: true, hdr: true)]
    [SerializeField] Color Color = Color.white;
    [SerializeField] float Intensity = 1;
    [SerializeField] float Range = 5;

    Color LowAlphaColor
    {
        get
        {
            var color = Color;
            color.a = .3f;
            return color;
        }
    }

    Color IVertexColorLight.Color => Color;

    float IVertexColorLight.Intensity => Intensity;

    float IVertexColorLight.Range => Range;

    public float GetDistance(Vector3 from)
    {
        return Vector3.Distance(from, this.transform.position);
    }

    // This is called every frame in the Scene View
    void OnDrawGizmos()
    {
        Gizmos.color = Color; // Set the gizmo color
        Gizmos.DrawSphere(transform.position, .2f); // Draw a filled sphere
    }

    // Optional: Only draw when the GameObject is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = LowAlphaColor;
        Gizmos.DrawWireSphere(transform.position, Range * .75f); // Draw a wireframe sphere
    }
}
