using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace CppCodeEditor
{
	public partial class Form1 : Form
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
		const int WM_USER = 0x400;
		const int EM_HIDESELECTION = WM_USER + 63;

		bool wrongSelection = false;
		int rightSelection = 0;

		public String savePath = "";
		public String deafultSaveLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		private bool locationChanged = true;

		public List<String> classNames = new List<String>();
		public List<String> variableNames = new List<String>();
		public List<String> allNames = new List<String>();

		//duljina teksta u codeArea prije promijene
		private int previousLength = 0;

		public Form1()
		{
			Syntax.loadSyntax();
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			string[] args = Environment.GetCommandLineArgs();
			try
			{
				savePath = args[1];
				OpenFile(savePath);
			}
			catch{}

			//full screen
			this.WindowState = FormWindowState.Maximized;
			AddLineNumbers();
			LineNumberTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
		}

		//Preuzeto s interneta
		public void AddLineNumbers()
		{
			// create & set Point pt to (0,0)    
			Point pt = new Point(0, 0);
			// get First Index & First Line from richTextBox1    
			int First_Index = codeArea.GetCharIndexFromPosition(pt);
			int First_Line = codeArea.GetLineFromCharIndex(First_Index);
			// set X & Y coordinates of Point pt to ClientRectangle Width & Height respectively    
			pt.X = ClientRectangle.Width;
			pt.Y = ClientRectangle.Height;
			// get Last Index & Last Line from richTextBox1    
			int Last_Index = codeArea.GetCharIndexFromPosition(pt);
			int Last_Line = codeArea.GetLineFromCharIndex(Last_Index);
			// set Center alignment to LineNumberTextBox    
			LineNumberTextBox.SelectionAlignment = HorizontalAlignment.Center;
			// set LineNumberTextBox text to null & width to getWidth() function value    
			LineNumberTextBox.Text = "";
			LineNumberTextBox.Width = 50;
			// now add each line number to LineNumberTextBox upto last line    
			for (int i = First_Line; i <= Last_Line + 2; i++)
			{
				LineNumberTextBox.Text += i + 1 + "\n";
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		


		private void codeArea_TextChanged(object sender, EventArgs e)
		{
			AddLineNumbers();
			if (wrongSelection)
			{
				codeArea.SelectionStart = rightSelection;
				wrongSelection = false;
			}

			if (codeArea.Text.Length <= 1)
			{
				return;
			}

			int currentLength = codeArea.Text.Length;
			if (Math.Abs(currentLength - previousLength) > 1)
			{
				replaceTabs();
				colorKeywords(codeArea.Text, 0);
				previousLength = currentLength;
				return;
			}
			previousLength = currentLength;

			int poc = codeArea.GetFirstCharIndexOfCurrentLine();
			int line = codeArea.GetLineFromCharIndex(poc);
			colorKeywords(codeArea.Lines[line], poc);
		}

		private void colorKeywords(String text, int textStartIndex)
		{
			int selectedIndex = codeArea.SelectionStart;

			//deselectamo codeArea i skrijemo selection
			debug.Focus();
			SendMessage(codeArea.Handle, EM_HIDESELECTION, 1, 0);


			//sva slova pobjamo u deafult boju
			codeArea.Select(textStartIndex, text.Length);
			codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.textColor);

			//detektiramo sva imena klasa i struktura
			Regex classNamesRegex = new Regex(@"(struct|class)\s+(\w+)");
			MatchCollection foundClassNames = classNamesRegex.Matches(codeArea.Text);
			classNames.Clear();
			allNames = new List<string>(Syntax.syntax.keywordInfo.keywords);
			foreach (Match match in foundClassNames)
			{
				classNames.Add(match.Groups[2].Value);
				allNames.Add(match.Groups[2].Value);
			}

			//detektiramo sva imena varijabli i funkcija
			Regex variableRegex;
			if (classNames.Count != 0)
				variableRegex = new Regex(@"(" + string.Join("|", Syntax.syntax.typenames) + "|" + string.Join("|", classNames) + @")\s+(\w+)");
			else
				variableRegex = new Regex(@"(" + string.Join("|", Syntax.syntax.typenames) + @")\s+(\w+)");
			MatchCollection foundVariables = variableRegex.Matches(codeArea.Text);
			variableNames.Clear();
			foreach (Match match in foundVariables)
			{
				Group name = match.Groups[2];
				variableNames.Add(name.Value);
				allNames.Add(name.Value);
			}

			//pobojamo sva pojavljivanja imena klase
			Regex classRegex = new Regex(@"\b(" + string.Join("|", classNames) + @")\b");
			MatchCollection foundClasses = classRegex.Matches(text);
			foreach (Match match in foundClasses)
			{
				codeArea.Select(match.Index + textStartIndex, match.Length);
				codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.classColor);
			}

			//pobojamo sva pojavljivanja imena varijabli
			Regex varRegex = new Regex(@"\b(" + string.Join("|", variableNames) + @")\b");
			MatchCollection foundVars = varRegex.Matches(text);
			foreach (Match match in foundVars)
			{
				codeArea.Select(match.Index + textStartIndex, match.Length);
				codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.variableColor);
			}

			// formira regex oblika "\b(keyword1|keyword2|keyword3)\b" 
			Regex keywordRegex = new Regex(@"\b(" + string.Join("|", Syntax.syntax.keywordInfo.keywords) + @")\b");
			MatchCollection foundKeywords = keywordRegex.Matches(text);
			//pobojamo sve kljucne rjeci
			foreach (Match match in foundKeywords)
			{
				codeArea.Select(match.Index + textStartIndex,  match.Length);
				codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.keywordInfo.color);
			}

			//detektiramo sve specialne znakove
			string specias = @"(" + string.Join("|", Syntax.syntax.specialsInfo.specials) + @")";
			Regex specialsRegex = new Regex(specias);
			MatchCollection foundSpecials = specialsRegex.Matches(text);
			//pobojamo sve kljucne rjeci
			foreach (Match match in foundSpecials)
			{
				codeArea.Select(match.Index + textStartIndex, match.Length);
				codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.specialsInfo.color);
			}

			//pobojamo sve wrappere
			foreach (var item in Syntax.syntax.wrappers)
			{
				Wrapper wrapper = item.Value;
				Regex wrapperRegex = new Regex("(" + wrapper.start + "(.|\n)*?(" + wrapper.end + "))", RegexOptions.Multiline);
				MatchCollection foundWrappers = wrapperRegex.Matches(codeArea.Text);
				foreach (Match match in foundWrappers)
				{
					codeArea.Select(match.Index, match.Length);
					codeArea.SelectionColor = ColorTranslator.FromHtml(wrapper.color);
				}
			}

			//vratimo focus na code area i ponovno prikazemo selekciju
			codeArea.Focus();
			SendMessage(codeArea.Handle, EM_HIDESELECTION, 0, 0);

			codeArea.SelectionStart = codeArea.SelectionStart;

			//resetiramo poziciju i boju selekcije
			codeArea.DeselectAll();
			codeArea.SelectionLength = 0;
			codeArea.SelectionStart = selectedIndex;
			codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.textColor);
		}

		private void codeArea_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '(' || e.KeyChar == '[' || e.KeyChar == '{' || e.KeyChar == '"' || e.KeyChar == '\'')
			{
				//deselectamo codeArea i skrijemo selection
				debug.Focus();
				SendMessage(codeArea.Handle, EM_HIDESELECTION, 1, 0);

				int selectedIndex = codeArea.SelectionStart;
				e.Handled = true;
				wrongSelection = true;
				rightSelection = codeArea.SelectionStart;
				switch (e.KeyChar)
				{
					case '(':
						codeArea.SelectedText = "()";
						break;
					case '[':
						codeArea.SelectedText = "[]";
						break;
					case '{':
						codeArea.SelectedText = "{}";
						break;
					case '"':
						codeArea.SelectedText = "\"\"";
						break;
					case '\'':
						codeArea.SelectedText = "''";
						break;
				}

				codeArea.SelectionStart = selectedIndex + 1;
				codeArea.SelectionLength = 0;

				//vratimo focus na code area i ponovno prikazemo selekciju
				codeArea.Focus();
				SendMessage(codeArea.Handle, EM_HIDESELECTION, 0, 0);
			}
			else if (e.KeyChar == '\t')
			{
				//deselectamo codeArea i skrijemo selection
				debug.Focus();
				SendMessage(codeArea.Handle, EM_HIDESELECTION, 1, 0);

				String currentWord = "";
				for (int i = codeArea.SelectionStart - 1; i >= 0 && Char.IsLetterOrDigit(codeArea.Text[i]); i--)
				{
					currentWord = currentWord.Insert(0, codeArea.Text[i].ToString());
				}
				
				int selected = codeArea.SelectionStart;
				if (currentWord != "")
				{
					foreach (String name in allNames)
					{
						if (name.StartsWith(currentWord) && name != currentWord)
						{
							wrongSelection = true;
							rightSelection = codeArea.SelectionStart;
							codeArea.Select(selected - currentWord.Length, currentWord.Length);
							codeArea.SelectedText = name;
							e.Handled = true;
							codeArea.SelectionStart = selected + (name.Length - currentWord.Length);

							//vratimo focus na code area i ponovno prikazemo selekciju
							codeArea.Focus();
							SendMessage(codeArea.Handle, EM_HIDESELECTION, 0, 0);
							return;
						}
					}
				}
				wrongSelection = true;
				rightSelection = codeArea.SelectionStart;
				codeArea.SelectedText = "    ";
				codeArea.SelectionStart += 4;
				e.Handled = true;

				//vratimo focus na code area i ponovno prikazemo selekciju
				codeArea.Focus();
				SendMessage(codeArea.Handle, EM_HIDESELECTION, 0, 0);
			}
		}
		private void codeArea_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
			{
				if (e.Modifiers == Keys.Shift)
				{
					savePath = "";
					SaveFile();
				}
				SaveFile();
			}
			else if (e.Control && e.KeyCode == Keys.V)
			{
				//maknemo formatting s zaljepljenoga texta
				((RichTextBox)sender).Paste(DataFormats.GetFormat("Text"));
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Enter)
			{
				int start = codeArea.GetFirstCharIndexOfCurrentLine();
				int cnt = 0;
				for (int i = start; i < codeArea.Text.Length; i++)
				{
					if (codeArea.Text[i].ToString() == " ") cnt++;
					else break;
				}

				codeArea.SelectedText = "\n" + String.Concat(Enumerable.Repeat(" ", cnt));
				e.Handled = true;
			}
		}

		private void replaceTabs()
		{
			int selected = codeArea.SelectionStart;
			codeArea.Text = codeArea.Text.Replace("\t", "    ");
			codeArea.SelectionStart = selected;
		}

		private void SaveFile()
		{
			if (savePath == "")
			{
				using (SaveFileDialog saveFile = new SaveFileDialog())
				{
					saveFile.InitialDirectory = deafultSaveLocation;
					saveFile.Filter = "C/C++ files(*.cpp)|*.cpp|All files (*.*)|*.*";
					saveFile.FileName = "test.cpp";
					if (saveFile.ShowDialog() == DialogResult.OK)
					{
						savePath = saveFile.FileName;
					}
					else
					{
						return;
					}
				}
			}
			using (StreamWriter sw = File.CreateText(savePath))
			{
				sw.Write(codeArea.Text);
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFile();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			savePath = "";
			locationChanged = true;
			SaveFile();
		}

		private void Build()
		{
			string buildFilePath = Path.GetTempPath() + @"\build.bat";
			if (locationChanged)
			{
				locationChanged = false;
				using (StreamWriter sw = File.CreateText(buildFilePath))
				{
					sw.WriteLine("@echo off");
					sw.WriteLine("cd " + Path.GetDirectoryName(savePath));
					sw.WriteLine("@echo on");
					sw.WriteLine("g++ " + Path.GetFileName(savePath) + " -o " + Path.GetFileNameWithoutExtension(savePath) + ".exe");
				};
			}

			compilerOutputBox.Text = "";
			Process build = new Process();
			build.StartInfo.FileName = buildFilePath;
			build.StartInfo.RedirectStandardError = true;
			build.StartInfo.RedirectStandardOutput = true;
			build.StartInfo.UseShellExecute = false;
			build.StartInfo.CreateNoWindow = true;

			build.Start();
			
			build.WaitForExit();	
			compilerOutputBox.Text = build.StandardError.ReadToEnd();
			if (compilerOutputBox.Text.Length == 0)
			{
				compilerOutputBox.Text = "Build succesfull!\n";
			}
		}

		private void Run()
		{
			string runPath = Path.GetDirectoryName(savePath) + "\\" + Path.GetFileNameWithoutExtension(savePath) + ".exe";
			try
			{
				Process.Start(runPath);
			}
			catch
			{
				compilerOutputBox.Text += "\nUnable to run file!";
			}
		}

		private void buildButton_Click(object sender, EventArgs e)
		{
			SaveFile();

			if (savePath == "")
			{
				compilerOutputBox.Text = "Save file first!";
				return;
			}

			Build();
		}

		private void runButton_Click(object sender, EventArgs e)
		{
			if (savePath == "")
			{
				compilerOutputBox.Text = "Save file first!";
				return;
			}
			Run();
		}

		private void buildAndRunButton_Click(object sender, EventArgs e)
		{
			SaveFile();

			if (savePath == "")
			{
				compilerOutputBox.Text = "Save file first!";
				return;
			}

			Build();
			if (compilerOutputBox.Text == "Build succesfull!\n")
				Run();
		}

		private void OpenFile(string openPath)
		{
			using (StreamReader sr = File.OpenText(openPath))
			{
				codeArea.Text = sr.ReadToEnd();
			}
			colorKeywords(codeArea.Text, 0);
		}

		private void openBtton_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog saveFile = new OpenFileDialog())
			{
				saveFile.InitialDirectory = deafultSaveLocation;
				saveFile.Filter = "C/C++ files(*.cpp)|*.cpp|All files (*.*)|*.*";
				saveFile.FileName = "test.cpp";
				if (saveFile.ShowDialog() == DialogResult.OK)
				{
					savePath = saveFile.FileName;
				}
				else
				{
					return;
				}
			}
			OpenFile(savePath);
		}

		private void codeArea_VScroll(object sender, EventArgs e)
		{
			LineNumberTextBox.Text = "";
			AddLineNumbers();
			LineNumberTextBox.Invalidate();
		}
	}
}
