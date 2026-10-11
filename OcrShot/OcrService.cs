using Tesseract;

namespace OcrShot;

public sealed class OcrService : IDisposable
{
    private readonly Lock _sync = new();
    private TesseractEngine? _engine;
    private string? _language;
    private bool _disposed;

    public string Recognize(Bitmap image, string language)
    {
        ArgumentNullException.ThrowIfNull(image);
        if (language is not ("eng" or "deu"))
            throw new ArgumentException("Choose English or German.", nameof(language));

        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_engine is null || _language != language)
            {
                _engine?.Dispose();
                _engine = null;
                _language = null;
                string dataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");
                _engine = new TesseractEngine(dataPath, language, EngineMode.LstmOnly);
                _language = language;
            }

            using var pix = PixConverter.ToPix(image);
            using var page = _engine.Process(pix, PageSegMode.Auto);
            return page.GetText();
        }
    }

    public void Dispose()
    {
        lock (_sync)
        {
            if (_disposed) return;
            _engine?.Dispose();
            _engine = null;
            _disposed = true;
        }
    }
}
