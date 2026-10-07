using System.Drawing;
using System.Drawing.Imaging;

namespace OcrShot;

internal static class ScreenSelection
{
    internal static async Task<Bitmap?> SelectAsync(Form mainForm)
    {
        var frames = new List<(Rectangle Bounds, Bitmap Image)>();
        var overlays = new List<SelectionOverlay>();
        bool wasVisible = mainForm.Visible;
        mainForm.Hide();

        try
        {
            // Let the desktop redraw before capturing; overlays must not appear in the image.
            await Task.Delay(120);
            foreach (Screen screen in Screen.AllScreens)
            {
                Rectangle bounds = screen.Bounds;
                if (bounds.Width <= 0 || bounds.Height <= 0)
                    throw new InvalidOperationException("A monitor has invalid screen bounds.");

                var frame = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
                try
                {
                    using var graphics = Graphics.FromImage(frame);
                    graphics.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size,
                        CopyPixelOperation.SourceCopy);
                    frames.Add((bounds, frame));
                }
                catch
                {
                    frame.Dispose();
                    throw;
                }
            }

            if (frames.Count == 0)
                throw new InvalidOperationException("No monitor is available for selection.");

            var completion = new TaskCompletionSource<Bitmap?>(TaskCreationOptions.RunContinuationsAsynchronously);
            foreach (var (bounds, frame) in frames)
            {
                var overlay = new SelectionOverlay(bounds, frame);
                overlays.Add(overlay);
                overlay.Cancelled += () => completion.TrySetResult(null);
                overlay.FormClosed += (_, _) => completion.TrySetResult(null);
                overlay.RegionSelected += (source, region) =>
                {
                    try
                    {
                        Rectangle crop = SelectionGeometry.ToLocal(region, source.MonitorBounds);
                        var selectedImage = source.Frame.Clone(crop, PixelFormat.Format32bppArgb);
                        if (!completion.TrySetResult(selectedImage)) selectedImage.Dispose();
                    }
                    catch (Exception error)
                    {
                        completion.TrySetException(error);
                    }
                };
                overlay.Show();
            }

            Rectangle currentScreen = Screen.FromPoint(Cursor.Position).Bounds;
            overlays.Find(o => o.MonitorBounds == currentScreen)?.Activate();
            return await completion.Task;
        }
        finally
        {
            foreach (SelectionOverlay overlay in overlays)
            {
                overlay.Close();
                overlay.Dispose();
            }
            foreach (var (_, frame) in frames) frame.Dispose();
            if (wasVisible && !mainForm.IsDisposed)
            {
                mainForm.Show();
                mainForm.Activate();
            }
        }
    }
}
