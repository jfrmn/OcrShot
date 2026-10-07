using System.Drawing;

namespace OcrShot.Tests;

public class SelectionGeometryTests
{
    [Fact]
    public void NormalizesReverseDrag()
    {
        var screen = new Rectangle(-1920, 0, 1920, 1080);

        Rectangle selection = SelectionGeometry.ClipToScreen(
            new Point(-200, 250), new Point(-500, 100), screen);

        Assert.Equal(new Rectangle(-500, 100, 300, 150), selection);
        Assert.Equal(new Rectangle(1420, 100, 300, 150), SelectionGeometry.ToLocal(selection, screen));
    }

    [Fact]
    public void ClipsToStartMonitorEvenIfPointerCrossesTheBoundary()
    {
        var leftScreen = new Rectangle(-1920, 0, 1920, 1080);

        Rectangle selection = SelectionGeometry.ClipToScreen(
            new Point(-20, 40), new Point(300, 100), leftScreen);

        Assert.Equal(new Rectangle(-20, 40, 20, 60), selection);
        Assert.Equal(new Rectangle(1900, 40, 20, 60), SelectionGeometry.ToLocal(selection, leftScreen));
    }

    [Fact]
    public void ClipsAllEdgesAndNegativeOrigins()
    {
        var screen = new Rectangle(-1920, -1080, 1920, 1080);

        Rectangle selection = SelectionGeometry.ClipToScreen(
            new Point(-2300, -1200), new Point(100, 100), screen);

        Assert.Equal(screen, selection);
        Assert.Equal(new Rectangle(0, 0, 1920, 1080), SelectionGeometry.ToLocal(selection, screen));
    }

    [Fact]
    public void IgnoresZeroAreaAndOutOfBoundsSelections()
    {
        var screen = new Rectangle(-1920, 0, 1920, 1080);

        Assert.True(SelectionGeometry.ClipToScreen(new Point(-20, 15), new Point(-20, 15), screen).IsEmpty);
        Assert.True(SelectionGeometry.ClipToScreen(new Point(5, 10), new Point(50, 80), screen).IsEmpty);
        Assert.True(SelectionGeometry.ToLocal(new Rectangle(10, 10, 20, 20), screen).IsEmpty);
    }
}
