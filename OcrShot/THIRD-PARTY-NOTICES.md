# Third-party components

- `Tesseract` and `Tesseract.Drawing` NuGet packages, version 5.2.0, including
  `x64/tesseract50.dll`: [charlesw/tesseract](https://github.com/charlesw/tesseract),
  [Tesseract OCR](https://github.com/tesseract-ocr/tesseract). Apache-2.0;
  the license text is in `tessdata/LICENSE-tessdata_fast.txt`.
- `x64/leptonica-1.82.0.dll`: [Leptonica 1.82.0](https://github.com/DanBloomberg/leptonica/tree/1.82.0).
  Copyright (C) 2001-2020 Leptonica. License in `licenses/LICENSE-Leptonica.txt`
  (from revision `f4138265b390f1921b9891d6669674d3157887d8`).
- `tessdata/eng.traineddata` and `tessdata/deu.traineddata`:
  [tessdata_fast](https://github.com/tesseract-ocr/tessdata_fast), revision
  `87416418657359cb625c412a48b6e1d6d41c29bd`. Apache-2.0;
  license in `tessdata/LICENSE-tessdata_fast.txt`.
  SHA-256 (`eng.traineddata`): `7d4322bd2a7749724879683fc3912cb542f19906c83bcc1a52132556427170b2`.
  SHA-256 (`deu.traineddata`): `19d219bbb6672c869d20a9636c6816a81eb9a71796cb93ebe0cb1530e2cdb22d`.
- `msvcp140.dll`, `vcruntime140.dll`, and `vcruntime140_1.dll` (also copied
  into `x64/` for Tesseract's native loader): unmodified x64 Visual C++ runtime
  files from the `Microsoft.VC143.CRT` redistributable directory of a licensed
  Visual Studio 2022 installation. Distribution is subject to the Visual Studio
  license terms and [Visual Studio 2022 Redistribution](https://learn.microsoft.com/en-us/visualstudio/releases/2022/redistribution).
  No runtime installer is invoked by OcrShot.
- Self-contained .NET 10 runtime and Windows Forms libraries: Microsoft .NET
  Library license in `licenses/LICENSE-DotNet.txt`; bundled dependency notices
  in `licenses/THIRD-PARTY-NOTICES-DotNet.txt`.

Other OS components, including the Universal CRT APIs, are supplied by Windows.
