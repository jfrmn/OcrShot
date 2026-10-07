using System.Drawing;

namespace OcrShot;

public static class SelectionGeometry
{
    public static Rectangle ClipToScreen(Point start, Point end, Rectangle screenBounds)
    {
        if (screenBounds.Width <= 0 || screenBounds.Height <= 0)
            return Rectangle.Empty;

        int left = Math.Clamp(Math.Min(start.X, end.X), screenBounds.Left, screenBounds.Right);
        int top = Math.Clamp(Math.Min(start.Y, end.Y), screenBounds.Top, screenBounds.Bottom);
        int right = Math.Clamp(Math.Max(start.X, end.X), screenBounds.Left, screenBounds.Right);
        int bottom = Math.Clamp(Math.Max(start.Y, end.Y), screenBounds.Top, screenBounds.Bottom);
        if (left == right || top == bottom) return Rectangle.Empty;
        return Rectangle.FromLTRB(left, top, right, bottom);
    }

    public static Rectangle ToLocal(Rectangle selection, Rectangle screenBounds)
    {
        Rectangle clipped = Rectangle.Intersect(selection, screenBounds);
        if (clipped.Width <= 0 || clipped.Height <= 0) return Rectangle.Empty;

        clipped.Offset(-screenBounds.X, -screenBounds.Y);
        return clipped;
    }
}
