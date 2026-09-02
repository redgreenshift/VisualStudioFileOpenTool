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
        static Dictionary<int, string> Versions = new Dictionary<int, string>()
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

                if (!int.TryParse(args[0], out int vsYearSuffix) || vsYearSuffix < 1)
                    throw new ArgumentException("Invalid Visual Studio version: " + args[0]);
                if (!Versions.TryGetValue(vsYearSuffix, out string visualStudioProgId) || IsNullOrWhiteSpace(visualStudioProgId))
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

            // Intentionally allow reserved-looking names. NTFS/WSL may contain files such
            // as "con.txt"; ordinary Windows APIs may handle those paths differently.
            //
            // // Reject reserved device names (CON, PRN, AUX, NUL, COM1-9, LPT1-9) anywhere in the path.
            // if (IsReservedDeviceName(path)) return false;

            // Verify file actually exists on disk before passing to Visual Studio COM interface.
            if (!System.IO.File.Exists(path)) return false;

            return true;
        }

        // Windows reserved device names: CON, PRN, AUX, NUL, COM1-9, LPT1-9
        private static readonly string[] ReservedDeviceNames = CreateReservedDeviceNameList();

        /// <summary>
        /// Creates the list of reserved Windows device names.
        /// </summary>
        /// <returns>
        /// An array containing the base device names <c>CON</c>, <c>PRN</c>,
        /// <c>AUX</c>, and <c>NUL</c>, along with <c>COM1</c> through <c>COM9</c>
        /// and <c>LPT1</c> through <c>LPT9</c>.
        /// </returns>
        private static string[] CreateReservedDeviceNameList()
        {
            // CON, PRN, AUX, NUL + COM1-9 + LPT1-9
            string[] arr = new string[4 + (9 * 2)];
            int idx = 0;

            arr[idx++] = "CON";
            arr[idx++] = "PRN";
            arr[idx++] = "AUX";
            arr[idx++] = "NUL";

            for (int i = 1; i <= 9; i++)
            {
                arr[idx++] = "COM" + i;
                arr[idx++] = "LPT" + i;
            }

            return arr;
        }

        /// <summary>
        /// Determines whether the filename component of the specified filename or
        /// path matches a reserved Windows device name.
        /// </summary>
        /// <remarks>
        /// The comparison is case-insensitive. Trailing spaces and periods are
        /// ignored, and any extension is ignored when identifying the device name.
        /// For example, <c>CON</c>, <c>CON.txt</c>, <c>COM1</c>, and
        /// <c>LPT9.anything</c> are considered reserved.
        /// </remarks>
        /// <param name="nameOrPath">
        /// A filename or path whose final component should be checked.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the base name of the final filename component is
        /// <c>CON</c>, <c>PRN</c>, <c>AUX</c>, <c>NUL</c>, <c>COM1</c> through
        /// <c>COM9</c>, or <c>LPT1</c> through <c>LPT9</c>, ignoring case and any
        /// extension; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsReservedDeviceName(string nameOrPath)
        {
            if (IsNullOrWhiteSpace(nameOrPath))
                return false;

            string s = nameOrPath.Trim();

            // Keep only the filename portion (strip directories).
            int lastSlash = Math.Max(s.LastIndexOf('\\'), s.LastIndexOf('/'));
            if (lastSlash >= 0 && lastSlash + 1 < s.Length)
                s = s.Substring(lastSlash + 1);

            if (s.Length == 0)
                return false;

            // NTFS treats trailing spaces and dots as if removed.
            s = TrimEndSpacesAndDots(s);
            if (s.Length == 0)
                return false;

            // Match the part before the first dot.
            int dotIndex = s.IndexOf('.');
            string devicePart = (dotIndex >= 0) ? s.Substring(0, dotIndex) : s;

            // Compare case-insensitively to reserved device names.
            for (int i = 0; i < ReservedDeviceNames.Length; ++i)
            {
                if (string.Equals(devicePart, ReservedDeviceNames[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Removes trailing spaces and periods from the specified string.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <returns>
        /// The string without any trailing spaces or periods.
        /// If <paramref name="s"/> consists entirely of spaces and periods,
        /// an empty string is returned.
        /// </returns>
        private static string TrimEndSpacesAndDots(string s)
        {
            int end = s.Length;

            while (end > 0)
            {
                char c = s[end - 1];
                if (c == ' ' || c == '.')
                    end--;
                else
                    break;
            }

            return s.Substring(0, end);
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

        /// <summary>
        /// Returns a help message describing how to invoke the application and
        /// listing the supported Visual Studio releases.
        /// </summary>
        /// <remarks>
        /// The message documents the expected positional command-line arguments:
        /// a two-digit suffix of the Visual Studio release year, a file path, and a
        /// one-based line number. For example, <c>22</c> identifies Visual Studio
        /// 2022.
        /// </remarks>
        public static string GetHelpMessage()
        {
            string s = "Trying to open specified file at specified line in active Visual Studio instance\n\n";

            s += "Usage: <version> <file_path> <line_number>\n\n";

            s += String.Format("{0}\t{1}\n", "Visual Studio version", "Arg 1: <version>");
            s += String.Format("---------------------------\t---------------------\n");
            foreach (var info in Versions)
            {
                int vsYearSuffix = info.Key;
                s += String.Format("{0}{1:D2}\t\t{2}\n", "VisualStudio 20", vsYearSuffix, vsYearSuffix);
            }

            return s;
        }
    }
}
