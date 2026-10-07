using System.Drawing;
using System.Drawing.Text;

namespace OcrShot.Tests;

public class OcrServiceTests
{
    [Fact]
    public void RecognizesEnglishText()
    {
        using var image = RenderText("INVOICE 12345");
        using var ocr = new OcrService();

        string result = ocr.Recognize(image, "eng");

        Assert.Contains("INVOICE", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("12345", result);
    }

    [Fact]
    public void RecognizesGermanText()
    {
        using var image = RenderText("RECHNUNG 67890");
        using var ocr = new OcrService();

        string result = ocr.Recognize(image, "deu");

        Assert.Contains("RECHNUNG", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("67890", result);
    }

    [Fact]
    public void RecognizesGermanUmlautsAndSharpS()
    {
        using var image = RenderText("Grüße Straße Äpfel Öl Übung");
        using var ocr = new OcrService();

        string result = ocr.Recognize(image, "deu");

        Assert.Contains("Grüße", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Straße", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Äpfel", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Öl", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Übung", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CanSwitchLanguagesAndRejectUnsupportedOnes()
    {
        using var image = RenderText("TEST 12345");
        using var ocr = new OcrService();

        Assert.Contains("12345", ocr.Recognize(image, "eng"));
        Assert.Contains("12345", ocr.Recognize(image, "deu"));
        Assert.Throws<ArgumentException>(() => ocr.Recognize(image, "fra"));
    }

    private static Bitmap RenderText(string text)
    {
        var image = new Bitmap(850, 110);
        using var graphics = Graphics.FromImage(image);
        using var font = new Font("Arial", 44, FontStyle.Regular, GraphicsUnit.Pixel);
        graphics.Clear(Color.White);
        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        graphics.DrawString(text, font, Brushes.Black, new PointF(12, 12));
        return image;
    }
}
