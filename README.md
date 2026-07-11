VisualStudioFileOpenTool
========================

A tool to open a specified file at a specific line in the active Visual Studio instance.

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

