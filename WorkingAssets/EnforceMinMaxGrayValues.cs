// Name: Clamp RGB Minimum 25%
// Author: ChatGPT
// Submenu: Adjustments
// Keywords: clamp|min|RGB|floor|threshold
// Desc: Ensures each R, G, B value is at least 25%
// URL: https://yourdomain.com
// Help:

#region UICode
#endregion

void Render(Surface dst, Surface src, Rectangle rect)
{
    const byte minValue = 64;
    const byte maxValue = 192;

    for (int y = rect.Top; y < rect.Bottom; y++)
    {
        for (int x = rect.Left; x < rect.Right; x++)
        {
            ColorBgra c = src[x, y];

            byte r = (byte)Math.Clamp(c.R, minValue, maxValue);
            byte g = (byte)Math.Clamp(c.G, minValue, maxValue);
            byte b = (byte)Math.Clamp(c.B, minValue, maxValue);

            dst[x, y] = ColorBgra.FromBgra(b, g, r, c.A);
        }
    }
}