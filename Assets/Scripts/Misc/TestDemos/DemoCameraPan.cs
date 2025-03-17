using UnityEngine;

public class DemoCameraPan : MonoBehaviour
{
    [SerializeField] AnimationCurve CURVE;
    [SerializeField] float SPEED;

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, -CURVE.Evaluate(SPEED * Time.time) * 360, 0);
    }
}
