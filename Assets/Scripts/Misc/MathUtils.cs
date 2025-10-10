using UnityEngine;

public static class MathUtils
{
    public static void SpringDampTowards(
        ref Vector3 current,
        ref Vector3 velocity,
        Vector3 target,
        float springStrength,
        float damping)
    {
        // Direction toward target
        Vector3 toTarget = target - current;

        // Hooke's Law acceleration
        Vector3 acceleration = toTarget * springStrength;

        // Integrate velocity
        velocity += acceleration * Time.deltaTime;

        // Apply damping (prevents oscillation)
        velocity *= Mathf.Exp(-damping * Time.deltaTime);

        // Integrate position
        current += velocity * Time.deltaTime;
    }

    public static void SpringDampTowards(
        ref PupilOffsets current,
        ref PupilOffsets velocity,
        PupilOffsets target,
        float springStrength,
        float damping)
    {
        // Direction toward target
        PupilOffsets toTarget = target - current;

        // Hooke's Law acceleration
        PupilOffsets acceleration = toTarget * springStrength;

        // Integrate velocity
        velocity += acceleration * Time.deltaTime;

        // Apply damping (prevents oscillation)
        float dampFactor = Mathf.Exp(-damping * Time.deltaTime);
        velocity *= dampFactor;

        // Integrate position
        current += velocity * Time.deltaTime;
    }
}
