using System.Drawing;
using System.Globalization;

namespace OcrShot;

internal sealed class MainForm : Form
{
    private readonly OcrService _ocr = new();
    private readonly Button _selectButton = new() { Text = "Select area", AutoSize = true };
    private readonly Button _retryButton = new() { Text = "Retry copy", AutoSize = true, Visible = false };
    private readonly RadioButton _clipboardMode = new() { Text = "Clipboard", Appearance = Appearance.Button, AutoSize = true, Checked = true };
    private readonly RadioButton _textMode = new() { Text = "Text Box", Appearance = Appearance.Button, AutoSize = true };
    private readonly ComboBox _language = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
    private readonly TextBox _result = new() { Multiline = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, Visible = false };
    private readonly ProgressBar _progress = new() { Style = ProgressBarStyle.Marquee, MarqueeAnimationSpeed = 25, Dock = DockStyle.Fill, Visible = false };
    private readonly Label _status = new() { Text = "Ready.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft };
    private readonly TableLayoutPanel _layout = new() { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new Padding(12) };
    private bool _busy;
    private string? _pendingClipboardText;

    internal MainForm()
    {
        Text = "OcrShot";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(480, 155);
        MinimumSize = new Size(430, 175);
        AutoScaleMode = AutoScaleMode.Dpi;
        MaximizeBox = false;

        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 0));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var actions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = false };
        actions.Controls.AddRange([_selectButton, _retryButton]);
        _layout.Controls.Add(actions, 0, 0);

        var languageControls = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = false };
        languageControls.Controls.Add(new Label { Text = "Language:", AutoSize = true, Margin = new Padding(0, 7, 4, 0) });
        _language.Items.AddRange(["English", "Deutsch"]);
        _language.SelectedIndex = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "de" ? 1 : 0;
        languageControls.Controls.Add(_language);
        _layout.Controls.Add(languageControls, 1, 0);

        var modes = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = false };
        modes.Controls.AddRange([_clipboardMode, _textMode]);
        _layout.Controls.Add(modes, 0, 1);
        _layout.SetColumnSpan(modes, 2);
        _layout.Controls.Add(_result, 0, 2);
        _layout.SetColumnSpan(_result, 2);
        _layout.Controls.Add(_progress, 0, 3);
        _layout.SetColumnSpan(_progress, 2);
        _layout.Controls.Add(_status, 0, 4);
        _layout.SetColumnSpan(_status, 2);
        Controls.Add(_layout);

        _selectButton.Click += async (_, _) => await SelectAndRecognizeAsync();
        _retryButton.Click += (_, _) => { if (_pendingClipboardText is not null) CopyToClipboard(_pendingClipboardText); };
        _textMode.CheckedChanged += (_, _) => UpdateMode();
        AcceptButton = _selectButton;
    }

    private void UpdateMode()
    {
        bool showTextBox = _textMode.Checked;
        _result.Visible = showTextBox;
        _layout.RowStyles[2].Height = showTextBox ? LogicalToDeviceUnits(120) : 0;
        ClientSize = new Size(ClientSize.Width, LogicalToDeviceUnits(showTextBox ? 295 : 155));
    }

    private async Task SelectAndRecognizeAsync()
    {
        if (_busy) return;
        _busy = true;
        _selectButton.Enabled = false;
        _language.Enabled = false;
        _clipboardMode.Enabled = false;
        _textMode.Enabled = false;
        _retryButton.Enabled = false;
        _status.Text = "Drag a region on one monitor. Esc or right-click cancels.";

        bool capturing = true;
        try
        {
            using Bitmap? image = await ScreenSelection.SelectAsync(this);
            if (image is null)
            {
                _status.Text = "Selection canceled; previous output kept.";
                return;
            }

            capturing = false;
            _status.Text = "Recognizing printed text…";
            _progress.Visible = true;
            string language = _language.SelectedIndex == 1 ? "deu" : "eng";
            string text = await Task.Run(() => _ocr.Recognize(image, language));

            if (!OcrText.HasText(text))
            {
                _status.Text = "No text found; previous output kept.";
            }
            else if (_clipboardMode.Checked)
            {
                CopyToClipboard(text);
            }
            else
            {
                _result.Text = text;
                _status.Text = "Text is ready to edit.";
            }
        }
        catch (Exception error)
        {
            _status.Text = capturing
                ? $"Screen capture failed ({error.GetType().Name}). Check screen access and try again."
                : $"OCR failed ({error.GetType().Name}). Check bundled tessdata and x64 native files.";
        }
        finally
        {
            _progress.Visible = false;
            _selectButton.Enabled = true;
            _language.Enabled = true;
            _clipboardMode.Enabled = true;
            _textMode.Enabled = true;
            _retryButton.Enabled = true;
            _busy = false;
        }
    }

    private void CopyToClipboard(string text)
    {
        try
        {
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
            _pendingClipboardText = null;
            _retryButton.Visible = false;
            _status.Text = "Copied Unicode text to clipboard.";
        }
        catch (Exception)
        {
            _pendingClipboardText = text;
            _result.Text = text;
            _retryButton.Visible = true;
            _status.Text = "Clipboard unavailable. Retry or switch to Text Box to retrieve the text.";
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_busy && e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            _status.Text = "Please finish the scan before closing.";
        }
        base.OnFormClosing(e);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _ocr.Dispose();
        base.OnFormClosed(e);
    }
}
