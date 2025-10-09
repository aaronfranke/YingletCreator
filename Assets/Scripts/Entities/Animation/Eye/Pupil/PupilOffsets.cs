using System;
using UnityEngine;

public struct PupilOffsets
{
    public PupilOffsets(float yOffset, float xLeftOffset, float xRightOffset)
    {
        YOffset = yOffset;
        XLeftOffset = xLeftOffset;
        XRightOffset = xRightOffset;
    }

    public float YOffset { get; }
    public float XLeftOffset { get; }
    public float XRightOffset { get; }

    public override bool Equals(object obj)
    {
        if (obj is PupilOffsets other)
        {
            return YOffset == other.YOffset &&
                   XLeftOffset == other.XLeftOffset &&
                   XRightOffset == other.XRightOffset;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(YOffset, XLeftOffset, XRightOffset);
    }

    public static PupilOffsets Lerp(PupilOffsets a, PupilOffsets b, float t)
    {
        return new PupilOffsets(
            Mathf.Lerp(a.YOffset, b.YOffset, t),
            Mathf.Lerp(a.XLeftOffset, b.XLeftOffset, t),
            Mathf.Lerp(a.XRightOffset, b.XRightOffset, t)
        );
    }
}


public static class PupilFloatsExtensionMethods
{
    public static Vector2 GetLeftEyeOffsets(this PupilOffsets data)
    {
        return new Vector2(data.XLeftOffset, data.YOffset);
    }
    public static Vector2 GetRightEyeOffsets(this PupilOffsets data)
    {
        return new Vector2(data.XRightOffset, data.YOffset);
    }

    public static PupilOffsets ShiftBothBy(this PupilOffsets data, Vector2 offset)
    {
        return new PupilOffsets(data.YOffset + offset.y, data.XLeftOffset + offset.x, data.XRightOffset - offset.x);
    }
}