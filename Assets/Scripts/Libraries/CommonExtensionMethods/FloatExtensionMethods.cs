public static class FloatExtensionMethods
{
	public static float Wrap01(this float value)
	{
		return (value % 1 + 1) % 1;
	}
}
