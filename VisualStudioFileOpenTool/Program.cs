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

		[STAThread]
		static void Main(string[] args)
		{
			EnvDTE80.DTE2 dte2 = null;
			try
			{
				if (args != null && args.Length >= 3)
				{
					if (!int.TryParse(args[0], out int vsVersion) || vsVersion < 1)
						throw new ArgumentException("Invalid Visual Studio version: " + args[0]);
					string vsString = GetVersionString(vsVersion);
					if (IsNullOrWhiteSpace(vsString))
						throw new ArgumentException("Invalid Visual Studio version: " + args[0]);

					String filename = args[1];
					if (!ValidAndSafeFilename(filename))
						throw new ArgumentException("Invalid file name: " + args[1]);

					if (!int.TryParse(args[2], out int fileline) || fileline < 1)
						throw new ArgumentException("Invalid line number: " + args[2]);

					dte2 = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject(vsString);
					dte2.MainWindow.Activate();
					SetForegroundWindow(new IntPtr(dte2.MainWindow.HWnd));

					EnvDTE.Window w = dte2.ItemOperations.OpenFile(filename, EnvDTE.Constants.vsViewKindTextView);
					((EnvDTE.TextSelection)dte2.ActiveDocument.Selection).GotoLine(fileline);
				}
				else
				{
					MessageBox.Show(GetHelpMessage());
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message + "\n\n" + GetHelpMessage());
			}
			finally
			{
				// Clean up COM objects if necessary
				if (dte2 != null)
					Marshal.ReleaseComObject(dte2);
			}
		}

		// TODO: Remove this method and use string.IsNullOrWhiteSpace() instead if targeting .NET 4.0 or higher.
		public static bool IsNullOrWhiteSpace(string value)
		{
			if (value == null)
				return true;
			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Verifies that the specified filename is valid and safe to pass to Visual Studio COM interface.
		/// </summary>
		/// <param name="filename">The filename to validate.</param>
		/// <returns>True if the filename is valid and safe to pass to Visual Studio COM interface; otherwise, false.</returns>
		static bool ValidAndSafeFilename(string filename)
		{
			if (IsNullOrWhiteSpace(filename)) return false;

			// Reject path traversal attempts anywhere in the string
			if (filename.Contains("..")) return false; // Intentionally overly agressive, could relax to @"\..\"

			// Check for any invalid characters in the path portion of the string.
			if (filename.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0) return false;

			// Reject absolute network/UNC style locations via explicit detection
			if (IsUncPath(filename)) return false;

			// Require fully-qualified local path!
			if (!System.IO.Path.IsPathRooted(filename)) return false;

			// Verify file actually exists on disk before passing to Visual Studio COM interface.
			if (!System.IO.File.Exists(filename)) return false;

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

			return path.StartsWith(@"\\?\UNC\", StringComparison.OrdinalIgnoreCase) // Handle extended UNC prefix \\?\UNC\ used with long paths
						// device namespace prefix used to access devices like \\.\PhysicalDrive0
				|| (   !path.StartsWith(@"\\.\", StringComparison.OrdinalIgnoreCase)
						// Traditional network path formats: backslash or forward slash variants supported by shell API.
					&& (path.StartsWith(@"\\", StringComparison.Ordinal) || path.StartsWith(@"//", StringComparison.Ordinal)));
		}

		public static string GetHelpMessage()
		{
			var versions = new List<int>() { 2, 3, 5, 8, 10, 12, 13, 15, 17, 19, 22, 25 };
			string s = "Trying to open specified file at specified line in active Visual Studio instance\n\n";

			s += "Usage: <version> <file_path> <line_number>\n\n";

			s += String.Format("{0}\t{1}\n", "Visual Studio version", "Arg 1: <version>");
			s += String.Format("---------------------------\t---------------------\n");
			foreach (int version in versions)
			{
				s += String.Format("{0}{1:D2}", "VisualStudio 20", version);
				s += String.Format("\t\t{0}\n", version);
			}

			s += "";

			return s;
		}

		public static string GetVersionString(int visualStudioVersionNumber)
		{
			// Sources:
			// http://www.mztools.com/articles/2011/MZ2011011.aspx
			// https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_automationinterface/242746251.html&id=
			// https://reactos.org/wiki/Visual_Studio_Versions
			// https://en.wikipedia.org/wiki/Visual_Studio
			switch (visualStudioVersionNumber)
			{
				case 25: return "VisualStudio.DTE.18.0"; // 2025
				case 22: return "VisualStudio.DTE.17.0"; // 2022
				case 19: return "VisualStudio.DTE.16.0"; // 2019
				case 17: return "VisualStudio.DTE.15.0"; // 2017
				case 15: return "VisualStudio.DTE.14.0"; // 2015
				case 13: return "VisualStudio.DTE.12.0"; // 2013
				case 12: return "VisualStudio.DTE.11.0"; // 2012
				case 10: return "VisualStudio.DTE.10.0"; // 2010
				case 8:  return "VisualStudio.DTE.9.0";  // 2008
				case 5:  return "VisualStudio.DTE.8.0";  // 2005
				case 3:  return "VisualStudio.DTE.7.1";  // 2003
				case 2:  return "VisualStudio.DTE.7";    // 2002
			}

			return "";
		}
	}
}
