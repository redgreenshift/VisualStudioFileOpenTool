VisualStudioFileOpenTool
========================

A tool to open a specified file at a specific line in the active Visual Studio instance.

[![C#](https://img.shields.io/badge/language-C%23-68217A.svg?logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET Framework](https://img.shields.io/badge/Framework-2.0-512BD4.svg?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/platform/support/policy/dotnet-framework)
[![License: Unlicense](https://img.shields.io/badge/License-Unlicense-blue.svg)](https://unlicense.org/)
[![Windows](https://img.shields.io/badge/Windows-0078D6.svg?logo=windows&logoColor=white)](https://www.microsoft.com/windows)

[Download binary](https://github.com/diimdeep/VisualStudioFileOpenTool/blob/master/VisualStudioFileOpenTool/bin/Release/VisualStudioFileOpenTool.exe)

**Usage:** `VisualStudioFileOpenTool.exe <version> <file_path> <line_number>`

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
| VisualStudio 2025 | 25 |

*(Note: The first argument is the `<version>` value from the table above.)*

**GrepWin settings:**

	VisualStudioFileOpenTool.exe 25 %path% %line%

**Beyond Compare settings (Options - Open With - Command line):**

	VisualStudioFileOpenTool.exe 25 %f %l

*Inspired by this StackOverflow question: ["Open a file in Visual Studio at a specific line number"](http://stackoverflow.com/questions/350323/open-a-file-in-visual-studio-at-a-specific-line-number).*

[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/diimdeep/VisualStudioFileOpenTool/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

## AI Policy

Contributions from AI agents are welcome, provided they are reviewed by a
human before being committed. Every change MUST be approved by a real person;
approval by an automated process or another AI agent alone is insufficient.

AI tools may be used to suggest code ideas or help draft comments, but all
code is reviewed by the project author before committing. Code that the
author does not fully understand is not committed.