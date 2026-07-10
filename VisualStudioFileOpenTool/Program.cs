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
			try
			{
				if (args != null && args.Length >= 3)
				{
					if (!int.TryParse(args[0], out int vsVersion) || vsVersion < 1)
					{
						MessageBox.Show("Invalid Visual Studio version: " + args[0]);
						return;
					}
					string vsString = GetVersionString(vsVersion);
					if (string.IsNullOrEmpty(vsString))
						return;

					String filename = args[1];

					if (!int.TryParse(args[2], out int fileline) || fileline < 1)
					{
						MessageBox.Show("Invalid line number: " + args[2]);
						return;
					}

					EnvDTE80.DTE2 dte2;
					dte2 = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject(vsString);
					dte2.MainWindow.Activate();
					SetForegroundWindow(new IntPtr(dte2.MainWindow.HWnd));
					EnvDTE.Window w = dte2.ItemOperations.OpenFile(filename, EnvDTE.Constants.vsViewKindTextView);
					((EnvDTE.TextSelection) dte2.ActiveDocument.Selection).GotoLine(fileline, true);
				}
				else
				{
					MessageBox.Show(GetHelpMessage());
				}
			}
			catch (Exception e)
			{
				Console.Write(e.Message);
			}
		}

		public static string GetHelpMessage()
		{
			var versions = new List<int>() { 2, 3, 5, 8, 10, 12, 13, 14, 15, 16, 17, 18 };
			string s = "Trying to open specified file at specified line in active Visual Studio instance\n\n";

			s += "Usage: <version> <file_path> <line_number>\n\n";

			s += String.Format("{0}\t{1}\n", "Visual Studio version", "Arg 1: <version>");
			s += String.Format("---------------------------\t---------------------\n");
			foreach (int version in versions)
			{
				if (version < 14)
					s += String.Format("{0}{1:D2}", "VisualStudio 20", version);
				else if (version == 14)
					s += String.Format("{0}", "VisualStudio 2015");
				else if (version == 15)
					s += String.Format("{0}", "VisualStudio 2017");
				else if (version == 16)
					s += String.Format("{0}", "VisualStudio 2019");
				else if (version == 17)
					s += String.Format("{0}", "VisualStudio 2022");
				else if (version == 18)
					s += String.Format("{0}", "VisualStudio 2025");

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
				case 18:
					return "VisualStudio.DTE.18.0"; // 2025
				case 17:
					return "VisualStudio.DTE.17.0"; // 2022
				case 16:
					return "VisualStudio.DTE.16.0"; // 2019
				case 15:
					return "VisualStudio.DTE.15.0"; // 2017
				case 14:
					return "VisualStudio.DTE.14.0"; // 2015
				case 13:
					return "VisualStudio.DTE.12.0"; // 2013
				case 12:
					return "VisualStudio.DTE.11.0"; // 2012
				case 10:
					return "VisualStudio.DTE.10.0"; // 2010
				case 8:
					return "VisualStudio.DTE.9.0"; // 2007
				case 5:
					return "VisualStudio.DTE.8.0"; // 2005
				case 3:
					return "VisualStudio.DTE.7.1"; // 2003
				case 2:
					return "VisualStudio.DTE.7";   // 2002
			}

			MessageBox.Show("Don't know this Visual Studio version.\n\n" + GetHelpMessage());

			return "";
		}
	}
}
