//Inspired by http://stackoverflow.com/questions/350323/open-a-file-in-visual-studio-at-a-specific-line-number

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VisualStudioFileOpenTool
{
    class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // Sources:
        // http://www.mztools.com/articles/2011/MZ2011011.aspx
        // https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_automationinterface/242746251.html&id=
        // https://reactos.org/wiki/Visual_Studio_Versions
        // https://en.wikipedia.org/wiki/Visual_Studio
        static Dictionary<int, string> theVersionInfo = new Dictionary<int, string>()
        {
            [2] = "VisualStudio.DTE.7",     // 2002
            [3] = "VisualStudio.DTE.7.1",   // 2003
            [5] = "VisualStudio.DTE.8.0",   // 2005
            [8] = "VisualStudio.DTE.9.0",   // 2008
            [10] = "VisualStudio.DTE.10.0", // 2010
            [12] = "VisualStudio.DTE.11.0", // 2012
            [13] = "VisualStudio.DTE.12.0", // 2013
            [15] = "VisualStudio.DTE.14.0", // 2015
            [17] = "VisualStudio.DTE.15.0", // 2017
            [19] = "VisualStudio.DTE.16.0", // 2019
            [22] = "VisualStudio.DTE.17.0", // 2022
            [26] = "VisualStudio.DTE.18.0", // 2026
        };

        [STAThread]
        static void Main(string[] args)
        {
            EnvDTE80.DTE2 visualStudio = null;
            try
            {
                if (args == null || args.Length < 3)
                {
                    MessageBox.Show(GetHelpMessage());
                    return;
                }

                if (!int.TryParse(args[0], out int vsVersion) || vsVersion < 1)
                    throw new ArgumentException("Invalid Visual Studio version: " + args[0]);
                if (!theVersionInfo.TryGetValue(vsVersion, out string visualStudioProgId) || IsNullOrWhiteSpace(visualStudioProgId))
                    throw new ArgumentException("Invalid Visual Studio version: " + args[0]);

                string filename = args[1];
                if (!ValidAndSafeFilename(filename))
                    throw new ArgumentException("Invalid file name: " + args[1]);

                if (!int.TryParse(args[2], out int fileLine) || fileLine < 1)
                    throw new ArgumentException("Invalid line number: " + args[2]);

                visualStudio = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject(visualStudioProgId);
                visualStudio.MainWindow.Activate();
                SetForegroundWindow(new IntPtr(visualStudio.MainWindow.HWnd));
                EnvDTE.Window window = visualStudio.ItemOperations.OpenFile(filename, EnvDTE.Constants.vsViewKindTextView);

                try
                {
                    ((EnvDTE.TextSelection)visualStudio.ActiveDocument.Selection).GotoLine(fileLine);
                }
                catch (Exception)
                {
                    // Occasionally, I get a weird error about a failed RPC call,
                    // but everything is working except for the selected line.
                    // I often don't notice until I see many dialog boxes. It's
                    // such a PAIN to close all the dialog boxes. Let's streamline
                    // the experience by silently failing to select the line.
                    //
                    // Easier for user to retrigger an edit command from a diff tool
                    // than close all the dialogs that appear behind Visual Studio.
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + GetHelpMessage());
            }
            finally
            {
                // Clean up COM objects if necessary
                if (visualStudio != null)
                    Marshal.ReleaseComObject(visualStudio);
            }
        }

        // TODO: Remove this method and use string.IsNullOrWhiteSpace() instead if targeting .NET 4.0 or higher.
        public static bool IsNullOrWhiteSpace(string value)
        {
            if (value == null)
                return true;
            for (int i = 0; i < value.Length; ++i)
            {
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Verifies that the specified filename (including path) is valid and safe to pass to Visual Studio COM interface.
        /// </summary>
        /// <param name="path">The filename and path to validate.</param>
        /// <returns>True if the filename and path is valid and safe to pass to Visual Studio COM interface; otherwise, false.</returns>
        static bool ValidAndSafeFilename(string path)
        {
            if (IsNullOrWhiteSpace(path)) return false;

            // Reject path traversal attempts anywhere in the string
            if (path.StartsWith("..") || path.Contains(@"\..") || path.Contains(@"/..")) return false;

            // trailing dots or spaces can behave strangely on Windows
            if (path.EndsWith(".") || char.IsWhiteSpace(path[path.Length - 1])) return false;

            // Check for any invalid characters in the path portion of the string.
            if (path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0) return false;

            // Reject absolute network/UNC style locations via explicit detection
            if (IsUncPath(path)) return false;

            // Require fully-qualified local path
            if (!System.IO.Path.IsPathRooted(path)) return false;

            // Verify file actually exists on disk before passing to Visual Studio COM interface.
            if (!System.IO.File.Exists(path)) return false;

            return true;
        }

        /// <summary>
        /// Determines whether the specified path is a UNC (Universal Naming Convention) path.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the path is a UNC path; otherwise, false.</returns>
        public static bool IsUncPath(string path)
        {
            if (IsNullOrWhiteSpace(path))
                return false;

            path = path.Trim();
            if (path.Length < 3)
                return false; // Minimum length for a UNC path is 3 characters (e.g., \\a\b)

            // Handle extended UNC prefix \\?\UNC\ used with long paths
            return path.StartsWith(@"\\?\UNC\", StringComparison.OrdinalIgnoreCase)
                // device namespace prefix used to access devices like \\.\PhysicalDrive0
                || (!path.StartsWith(@"\\.\", StringComparison.OrdinalIgnoreCase)
                    // Traditional network path formats: backslash or forward slash variants supported by shell API.
                    && (path.StartsWith(@"\\", StringComparison.Ordinal) || path.StartsWith(@"//", StringComparison.Ordinal)));
        }

        public static string GetHelpMessage()
        {
            string s = "Trying to open specified file at specified line in active Visual Studio instance\n\n";

            s += "Usage: <version> <file_path> <line_number>\n\n";

            s += String.Format("{0}\t{1}\n", "Visual Studio version", "Arg 1: <version>");
            s += String.Format("---------------------------\t---------------------\n");
            foreach (var info in theVersionInfo)
            {
                int version = info.Key;
                s += String.Format("{0}{1:D2}", "VisualStudio 20", version);
                s += String.Format("\t\t{0}\n", version);
            }

            return s;
        }
    }
}
