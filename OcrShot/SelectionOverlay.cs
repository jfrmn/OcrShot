using System.Drawing;

namespace OcrShot;

internal sealed class SelectionOverlay : Form
{
    private readonly Bitmap _frame;
    private Point _start;
    private Rectangle _selection;
    private bool _dragging;

    internal SelectionOverlay(Rectangle monitorBounds, Bitmap frame)
    {
        MonitorBounds = monitorBounds;
        _frame = frame;
        AutoScaleMode = AutoScaleMode.None;
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        Bounds = monitorBounds;
        ShowInTaskbar = false;
        TopMost = true;
        DoubleBuffered = true;
        KeyPreview = true;
        Cursor = Cursors.Cross;
    }

    internal Rectangle MonitorBounds { get; }
    internal Bitmap Frame => _frame;
    internal event Action<SelectionOverlay, Rectangle>? RegionSelected;
    internal event Action? Cancelled;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.DrawImageUnscaled(_frame, Point.Empty);
        using var dim = new SolidBrush(Color.FromArgb(160, Color.Black));
        e.Graphics.FillRectangle(dim, ClientRectangle);

        Rectangle local = SelectionGeometry.ToLocal(_selection, MonitorBounds);
        if (local.Width > 0 && local.Height > 0)
        {
            e.Graphics.DrawImage(_frame, local, local, GraphicsUnit.Pixel);
            using var outline = new Pen(Color.DeepSkyBlue, 2);
            e.Graphics.DrawRectangle(outline, local.X, local.Y, local.Width - 1, local.Height - 1);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Right)
        {
            Cancelled?.Invoke();
        }
        else if (e.Button == MouseButtons.Left)
        {
            _start = PointToScreen(e.Location);
            _selection = Rectangle.Empty;
            _dragging = true;
            Capture = true;
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (!_dragging) return;

        _selection = SelectionGeometry.ClipToScreen(_start, PointToScreen(e.Location), MonitorBounds);
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (!_dragging || e.Button != MouseButtons.Left) return;

        _dragging = false;
        Capture = false;
        _selection = SelectionGeometry.ClipToScreen(_start, PointToScreen(e.Location), MonitorBounds);
        if (_selection.Width > 0 && _selection.Height > 0)
            RegionSelected?.Invoke(this, _selection);
        else
            Cancelled?.Invoke();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            Cancelled?.Invoke();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
}
