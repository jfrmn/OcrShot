namespace OcrShot;

public static class OcrText
{
    public static string WithoutTrailingTerminators(string text) => text.TrimEnd('\r', '\n', '\f');

    public static bool HasText(string text) => !string.IsNullOrWhiteSpace(text);
}
