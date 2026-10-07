# OcrShot

A small offline Windows app for copying printed text from a screen rectangle. Select a region on **one monitor**; everything outside it is dimmed during the drag. Choose **Clipboard** to copy Unicode text, or **Text Box** to edit the recognized text. The English/German selector uses bundled Tesseract models (German is selected by default for German UI locales). A progress bar appears while OCR runs.

## Run the self-contained folder (candidate portable ZIP)

On Windows 10/11 x64, unzip `artifacts/OcrShot-win-x64.zip` into a folder of your choice and start `OcrShot.exe` from that folder. Keep all accompanying files and subfolders together. No installer, administrator rights, preinstalled .NET or Tesseract, or model download is intended. The folder includes the self-contained .NET runtime, `x64/` native OCR libraries, app-local Visual C++ runtime files, `tessdata/eng.traineddata` and `deu.traineddata`, and `licenses/`/`THIRD-PARTY-NOTICES.md`. Launch on a clean machine without those runtimes has **not yet been verified**; this ZIP is a release candidate until that check passes.

Click **Select area**, drag over legible printed text and release. Start on any connected monitor; the selection is clipped to that monitor. Press **Esc** or right-click to cancel. Switching to **Text Box** reveals a multiline editable result; switching back hides it. An empty result or cancelled selection leaves the previous output unchanged. If the clipboard is busy, click **Retry copy** or switch to Text Box to recover the text.

## Build and package

Requires the .NET 10 SDK and Windows x64. For the portable ZIP, the build machine also needs a validly licensed Visual Studio 2022 installation with the **x64 Visual C++ redistributable** files; the script locates it with `vswhere`. It never copies the system-installed Windows DLLs or runs a redistributable installer.

```powershell
dotnet test tests/OcrShot.Tests/OcrShot.Tests.csproj -c Release
./scripts/Publish-Portable.ps1
```

The script publishes self-contained for `win-x64` into `artifacts/publish-win-x64`, copies the three necessary Microsoft VC++ x64 runtime files from Visual Studio's `Microsoft.VC143.CRT` directory to the application folder (and `x64/` for native DLL loading), removes unnecessary x86 DLLs and the application PDB, checks the bundled models' SHA-256 hashes and required assets, and creates `artifacts/OcrShot-win-x64.zip`. To replace a previous publish/ZIP, pass `-ReplaceExisting` (only use this for artifacts you are prepared to discard). To use a different licensed Visual Studio 2022 installation, pass `-VCRedistDirectory` pointing to its `x64/Microsoft.VC143.CRT` folder.

To publish without packaging, run `dotnet publish src/OcrShot/OcrShot.csproj -c Release -r win-x64 --self-contained true -o artifacts/publish-win-x64`, **but that output is not portable until the VC++ runtime files have been copied**. Use the script for a candidate ZIP and validate on a clean Windows x64 machine before calling it independently portable. See `src/OcrShot/THIRD-PARTY-NOTICES.md` for model provenance, licenses and VC++ redistribution conditions.

## Technical choices and limits

WinForms on .NET 10 keeps the UI and screen capture small, supports per-monitor DPI awareness and produces an unpackaged self-contained folder. Tesseract 5.2.0 with the fast English/German models works offline without Windows OCR package identity or installed language packs; printed text is favored over handwriting. A drag cannot cross monitors, and selection previews a frozen screen capture. Some protected content or secure desktops cannot be captured. OCR quality depends on font, size, contrast and the chosen language.

The app does not save screenshots or recognized text to disk, upload either, or send telemetry. Captured images and OCR results stay in memory until disposed or the app exits; **clipboard output remains on the Windows clipboard** and can be read by other applications until replaced. No OCR activity needs network access. The Windows OS and other applications may have their own clipboard or screen-capture policies.
