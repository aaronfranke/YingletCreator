using UnityEngine;

public struct YingPosePupilData
{
	public YingPosePupilData(float yOffset, float xLeftOffset, float xRightOffset)
	{
		YOffset = yOffset;
		XLeftOffset = xLeftOffset;
		XRightOffset = xRightOffset;
	}

	public float YOffset { get; }
	public float XLeftOffset { get; }
	public float XRightOffset { get; }
}


public static class YingPosePupilDataExtensions
{
	public static Vector2 GetLeftEyeOffsets(this YingPosePupilData data)
	{
		return new Vector2(data.XLeftOffset, data.YOffset);
	}
	public static Vector2 GetRightEyeOffsets(this YingPosePupilData data)
	{
		return new Vector2(data.XRightOffset, data.YOffset);
	}
}