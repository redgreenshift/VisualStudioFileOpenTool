VisualStudioFileOpenTool
========================

A command-line tool that opens a specified file in the active Visual Studio instance and navigates to the specified line. By default, the line receives focus; with `--select`, the entire line is selected for replacement. The tool fails gracefully if line navigation or selection is unsuccessful.

[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?logo=data:image/svg%2bxml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48IS0tIFVwbG9hZGVkIHRvOiBTVkcgUmVwbywgd3d3LnN2Z3JlcG8uY29tLCBHZW5lcmF0b3I6IFNWRyBSZXBvIE1peGVyIFRvb2xzOyBoYW5kIG1vZGlmaWVkIHRvIHdoaXRlIG1vbm9jaHJvbWUgLS0+CjxzdmcgZmlsbD0iI0ZGRkZGRiIgd2lkdGg9IjgwMHB4IiBoZWlnaHQ9IjgwMHB4IiB2aWV3Qm94PSItMC41IDAgMjQgMjQiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHBhdGggZD0ibTE3Ljg1OCAyMy45OTgtOS43NzEtOS40ODQtNS44NjYgNC40NjUtMi4yMjEtMS4xMTV2LTExLjcxOWwyLjIzNC0xLjEyMSA1Ljg3IDQuNDY5IDkuNzQ3LTkuNDkzIDUuNTg3IDIuMjM5djE5LjUzMWwtNS41NzkgMi4yM3ptLS41NjMtMTYuMTg2LTUuNTc3IDQuMTczIDUuNTggNC4yMDJ6bS0xNC41MDcgMS42ODV2NS4wMTZsMi43ODctMi41MjV6Ii8+PC9zdmc+)](https://visualstudio.microsoft.com/)
[![Windows](https://img.shields.io/badge/Windows-0078D4.svg?logo=data:image/svg%2bxml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48IS0tIE9yaWdpbmFsIGZyb206IFNWRyBSZXBvLCB3d3cuc3ZncmVwby5jb20sIEdlbmVyYXRvcjogU1ZHIFJlcG8gTWl4ZXIgVG9vbHM7IGhhbmQgbW9kaWZpZWQgdG8gd2hpdGUgbW9ub2Nocm9tZSAtLT4KPHN2ZyBmaWxsPSIjRkZGRkZGIiB3aWR0aD0iODAwcHgiIGhlaWdodD0iODAwcHgiIHZpZXdCb3g9IjAgMCA1MTIgNTEyIiBpZD0iaWNvbnMiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHBhdGggZD0iTTMxLjg3LDMwLjU4SDI0NC43VjI0My4zOUgzMS44N1oiLz48cGF0aCBkPSJNMjY2Ljg5LDMwLjU4SDQ3OS43VjI0My4zOUgyNjYuODlaIi8+PHBhdGggZD0iTTMxLjg3LDI2NS42MUgyNDQuN3YyMTIuOEgzMS44N1oiLz48cGF0aCBkPSJNMjY2Ljg5LDI2NS42MUg0NzkuN3YyMTIuOEgyNjYuODlaIi8+PC9zdmc+)](https://www.microsoft.com/windows)
[![C#](https://img.shields.io/badge/C%23-512BD4.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET Framework 3.5](https://img.shields.io/badge/3.5-512BD4.svg?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/platform/support/policy/dotnet-framework)
[![License: Unlicense](https://img.shields.io/badge/License-Unlicense-808080.svg?logo=unlicense&logoColor=white)](https://unlicense.org/)

[Download binary](https://github.com/redgreenshift/VisualStudioFileOpenTool/blob/master/VisualStudioFileOpenTool/bin/Release/VisualStudioFileOpenTool.exe)

**Usage:** `VisualStudioFileOpenTool.exe <version> <file_path> <line_number> [-s | --select]`

| Visual Studio version | Arg 1: `<version>` |
| :--- | :--- |
| VisualStudio 2002 | 2 |
| VisualStudio 2003 | 3 |
| VisualStudio 2005 | 5 |
| VisualStudio 2008 | 8 |
| VisualStudio 2010 | 10 |
| VisualStudio 2012 | 12 |
| VisualStudio 2013 | 13 |
| VisualStudio 2015 | 15 |
| VisualStudio 2017 | 17 |
| VisualStudio 2019 | 19 |
| VisualStudio 2022 | 22 |
| VisualStudio 2026 | 26 |

> [!NOTE]
> The first argument is the `<version>` value from the table above.

**GrepWin settings:**

	VisualStudioFileOpenTool.exe 26 %path% %line%

**Beyond Compare settings (Options - Open With - Command line):**

	VisualStudioFileOpenTool.exe 26 %f %l --select

## License

This project is released under [The Unlicense](LICENSE), a public-domain
dedication with a fallback license for jurisdictions where a public-domain
dedication is not legally effective.

You may use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the software without restriction, subject to the terms of The
Unlicense.

Some portions of this project are based on or inspired by the following works:

- [Stack Overflow question 350323: "Open a file in Visual Studio at a specific line number"](https://stackoverflow.com/questions/350323/open-a-file-in-visual-studio-at-a-specific-line-number)
- [VisualStudioFileOpenTool by diimdeep](https://github.com/diimdeep/VisualStudioFileOpenTool)
- [VisualStudioFileOpenTool by akof1314](https://github.com/akof1314/VisualStudioFileOpenTool)

Please see the source-file comments and the `LICENSE` file for additional
attribution and licensing information.

## AI Policy

Contributions from AI agents are welcome, provided they are reviewed by a
human before being committed. Every change MUST be approved by a real person;
approval by an automated process or another AI agent alone is insufficient.

AI tools may be used to suggest code ideas or help draft comments, but all
code is reviewed by the project author before committing. Code that the
author does not fully understand is not committed.