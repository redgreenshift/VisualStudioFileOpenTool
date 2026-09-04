VisualStudioFileOpenTool
========================

A command-line tool that opens a specified file in the active Visual Studio instance and navigates to the specified line. By default, the line receives focus; with `--select`, the entire line is selected for replacement. The tool fails gracefully if line navigation or selection is unsuccessful.

[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?logo=data:image/svg%2bxml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48IS0tIFVwbG9hZGVkIHRvOiBTVkcgUmVwbywgd3d3LnN2Z3JlcG8uY29tLCBHZW5lcmF0b3I6IFNWRyBSZXBvIE1peGVyIFRvb2xzOyBoYW5kIG1vZGlmaWVkIHRvIHdoaXRlIG1vbm9jaHJvbWUgLS0+CjxzdmcgZmlsbD0iI0ZGRkZGRiIgd2lkdGg9IjgwMHB4IiBoZWlnaHQ9IjgwMHB4IiB2aWV3Qm94PSIwIDAgMzIgMzIiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+CiAgPHBhdGggZD0iTTIzLjQzOCAwLjA5NGMtMC41MDUtMC4wMDUtMSAwLjE3Ny0xLjM3NSAwLjUyMS0wLjAyMSAwLjAyMS0wLjA0NyAwLjA0Mi0wLjA2OCAwLjA2M2wtMTAuNjIgMTEuNzQ1LTYuMjAzLTUuMDgzLTAuNTQyLTAuNDY5Yy0wLjM4LTAuMzEzLTAuOTA2LTAuMzk2LTEuMzY1LTAuMjAzLTAuMDA1IDAtMC4wMTAgMC4wMDUtMC4wMTYgMC4wMDVsLTIuNDIyIDFjLTAuMDMxIDAuMDE2LTAuMDY4IDAuMDMxLTAuMDk5IDAuMDQ3LTAuMDI2IDAuMDE2LTAuMDQ3IDAuMDI2LTAuMDY4IDAuMDM2LTAuMDE2IDAuMDEwLTAuMDMxIDAuMDIxLTAuMDQ3IDAuMDMxLTAuMDIxIDAuMDE2LTAuMDQ3IDAuMDMxLTAuMDY4IDAuMDQ3LTAuMDEwIDAuMDEwLTAuMDI2IDAuMDIxLTAuMDM2IDAuMDMxLTAuMDIxIDAuMDE2LTAuMDQyIDAuMDMxLTAuMDU3IDAuMDQ3LTAuMDIxIDAuMDE2LTAuMDM2IDAuMDMxLTAuMDUyIDAuMDQ3LTAuMDEwIDAuMDEwLTAuMDI2IDAuMDI2LTAuMDQyIDAuMDQ3LTAuMDE2IDAuMDE2LTAuMDMxIDAuMDMxLTAuMDQyIDAuMDQ3LTAuMDE2IDAuMDIxLTAuMDMxIDAuMDQyLTAuMDQ3IDAuMDYzLTAuMDEwIDAuMDEwLTAuMDIxIDAuMDI2LTAuMDMxIDAuMDQyLTAuMDE2IDAuMDIxLTAuMDMxIDAuMDQ3LTAuMDQ3IDAuMDY4LTAuMDA1IDAuMDE2LTAuMDE2IDAuMDMxLTAuMDI2IDAuMDQ3LTAuMDEwIDAuMDIxLTAuMDIxIDAuMDQyLTAuMDMxIDAuMDY4LTAuMDEwIDAuMDIxLTAuMDE2IDAuMDM2LTAuMDI2IDAuMDU3LTAuMDA1IDAuMDIxLTAuMDE2IDAuMDQyLTAuMDIxIDAuMDYzLTAuMDEwIDAuMDIxLTAuMDE2IDAuMDQyLTAuMDIxIDAuMDYzLTAuMDEwIDAuMDIxLTAuMDE2IDAuMDQ3LTAuMDIxIDAuMDczLTAuMDA1IDAuMDE2LTAuMDEwIDAuMDMxLTAuMDE2IDAuMDUyIDAgMC4wMjEtMC4wMDUgMC4wNDctMC4wMTAgMC4wNzMgMCAwLjAyMS0wLjAwNSAwLjA0Mi0wLjAwNSAwLjA2OC0wLjAwNSAwLjAzNi0wLjAwNSAwLjA3My0wLjAwNSAwLjEwOXYxNC4yMDhjMCAwLjUzNiAwLjMyMyAxLjAyMSAwLjgxOCAxLjIyNGwyLjQyMiAxLjAyMWMwLjQ2NCAwLjE5MyAxIDAuMTA0IDEuMzgtMC4yMTlsMC41NDItMC40NjkgNi4yMDMtNS4wODMgMTAuNjIgMTEuNzQ1YzAuMDMxIDAuMDMxIDAuMDY4IDAuMDYzIDAuMDk5IDAuMDg5IDAuMDI2IDAuMDIxIDAuMDUyIDAuMDQ3IDAuMDc4IDAuMDY4IDAuMDIxIDAuMDIxIDAuMDQ3IDAuMDM2IDAuMDczIDAuMDU3IDAuMDMxIDAuMDIxIDAuMDU3IDAuMDM2IDAuMDgzIDAuMDU3IDAuMDMxIDAuMDE2IDAuMDYzIDAuMDM2IDAuMDg5IDAuMDUyIDAuMDMxIDAuMDE2IDAuMDU3IDAuMDMxIDAuMDg5IDAuMDQ3czAuMDU3IDAuMDI2IDAuMDg5IDAuMDQyYzAuMDMxIDAuMDE2IDAuMDYzIDAuMDI2IDAuMDk0IDAuMDM2IDAuMDMxIDAuMDE2IDAuMDY4IDAuMDI2IDAuMDk5IDAuMDM2IDAuMDI2IDAuMDEwIDAuMDU3IDAuMDE2IDAuMDg5IDAuMDI2czAuMDY4IDAuMDIxIDAuMTA0IDAuMDI2YzAuMDMxIDAuMDA1IDAuMDYzIDAuMDE2IDAuMDk0IDAuMDIxczAuMDYzIDAuMDEwIDAuMDk5IDAuMDEwYzAuMDMxIDAuMDA1IDAuMDY4IDAuMDEwIDAuMDk5IDAuMDEwczAuMDYzIDAuMDA1IDAuMDk5IDAuMDA1YzAuMDMxIDAgMC4wNjggMCAwLjA5OSAwIDAuMDM2IDAgMC4wNzMtMC4wMDUgMC4xMDQtMC4wMDUgMC4wMzEtMC4wMDUgMC4wNjMtMC4wMTAgMC4wODktMC4wMTAgMC4wNDItMC4wMDUgMC4wNzgtMC4wMTAgMC4xMi0wLjAyMSAwLjAyNi0wLjAwNSAwLjA1Mi0wLjAxMCAwLjA3OC0wLjAxNiAwLjAzNi0wLjAxMCAwLjA3My0wLjAxNiAwLjEwOS0wLjAzMSAwLjAyNi0wLjAwNSAwLjA1Ny0wLjAxNiAwLjA4OS0wLjAyNnMwLjA2My0wLjAyMSAwLjA5NC0wLjAzNmMwLjA0Mi0wLjAxNiAwLjA3OC0wLjAzMSAwLjEyLTAuMDQ3bDYuNTg5LTMuMTcyYzAuMjQtMC4xMTUgMC40NTgtMC4yNzYgMC42My0wLjQ3OSAwLjA0Ny0wLjA0NyAwLjA4My0wLjA5OSAwLjEyNS0wLjE1MSAwIDAgMC0wLjAwNSAwLjAwNS0wLjAxMCAwLjAzNi0wLjA1MiAwLjA3My0wLjEwOSAwLjEwNC0wLjE2NyAwLjAzMS0wLjA1MiAwLjA1Ny0wLjEwNCAwLjA4My0wLjE1NiAwLjAwNS0wLjAxMCAwLjAwNS0wLjAxNiAwLjAxMC0wLjAyNiAwLjAyMS0wLjA0NyAwLjA0Mi0wLjA5NCAwLjA1Ny0wLjE0MSAwLjAwNS0wLjAxNiAwLjAxMC0wLjAzMSAwLjAxNi0wLjA1MiAwLjAxNi0wLjA0NyAwLjAzMS0wLjA5NCAwLjA0Mi0wLjE0MSAwLjAwNS0wLjAxNiAwLjAxMC0wLjAzMSAwLjAxMC0wLjA0NyAwLjAxMC0wLjA0NyAwLjAxNi0wLjA5NCAwLjAyNi0wLjEzNSAwLTAuMDIxIDAuMDA1LTAuMDQ3IDAuMDEwLTAuMDY4IDAtMC4wNDIgMC4wMDUtMC4wNzggMC4wMDUtMC4xMiAwLjAwNS0wLjAzNiAwLjAwNS0wLjA3MyAwLjAwNS0wLjEwNHYtMjEuNDg0YzAtMC4wMTYgMC0wLjAzMSAwLTAuMDQ3IDAtMC4wNjMtMC4wMDUtMC4xMy0wLjAxMC0wLjE5OC0wLjA4My0wLjY3Ny0wLjUwNS0xLjI2Ni0xLjEyLTEuNTU3bC02LjU4OS0zLjE2N2MtMC4yNi0wLjEyNS0wLjU0Mi0wLjE5My0wLjgyOC0wLjE5OHpNMjMuOTk1IDkuMjI5djEzLjU0MmwtOC4yNi02Ljc3MXpNNC4wMDUgMTEuNDMybDQuMTMgNC41NjgtNC4xMyA0LjU2OHoiLz4KPC9zdmc+)](https://visualstudio.microsoft.com/)
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
