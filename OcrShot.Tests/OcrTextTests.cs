namespace OcrShot.Tests;

public class OcrTextTests
{
    [Fact]
    public void KeepsUnicodeLeadingSpacesAndInternalLineBreaks()
    {
        string recognized = "  Grüße, Straße\r\nÄpfel und Öl\n\f";

        Assert.Equal("  Grüße, Straße\r\nÄpfel und Öl", OcrText.WithoutTrailingTerminators(recognized));
        Assert.True(OcrText.HasText(recognized));
    }

    [Fact]
    public void WhitespaceOnlyIsNotRecognizedText()
    {
        Assert.False(OcrText.HasText(OcrText.WithoutTrailingTerminators("\r\n\f")));
        Assert.True(OcrText.HasText("  ß "));
    }
}
