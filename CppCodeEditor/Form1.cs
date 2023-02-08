using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

		//Semaphore semaphore = new Semaphore(1, 1);
		bool wrongSelection = false;
		int rightSelection = 0;

		public String savePath = "";
		public String deafultSaveLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		public List<String> classNames = new List<String>();
		public List<String> variableNames = new List<String>();
		public List<String> allNames = new List<String>();

		public Form1()
		{
			Syntax.loadSyntax();
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//fullscreen form
			//this.TopMost = true;
			//this.FormBorderStyle = FormBorderStyle.None;
			//this.WindowState = FormWindowState.Maximized;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		private void codeArea_TextChanged(object sender, EventArgs e)
		{
			//semaphore.WaitOne();
			if (wrongSelection)
			{
				codeArea.SelectionStart = rightSelection;
				wrongSelection = false;
			}

			if (codeArea.Text.Length <= 1)
			{
				//semaphore.Release();
				return;
			}

			int poc;
			for (poc = codeArea.SelectionStart - 1; poc >= 0; poc--)
			{
				if (codeArea.Text[poc] == '\n') break;
			}
			if (poc < 0) poc = 0;
			int kraj;
			for (kraj = codeArea.SelectionStart; kraj < codeArea.Text.Length; kraj++)
			{
				if (codeArea.Text[kraj] == '\n') break;
			}
			colorKeywords(codeArea.Text.Substring(poc, kraj - poc), poc);
			//semaphore.Release();
		}

		private void colorKeywords(String text, int textStartIndex)
		{
			Console.WriteLine(text);
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
				Console.WriteLine("--" + match.Value + "--");
			}

			//pobojamo sva pojavljivanja imena varijabli
			Regex varRegex = new Regex(@"\b(" + string.Join("|", variableNames) + @")\b");
			MatchCollection foundVars = varRegex.Matches(text);
			foreach (Match match in foundVars)
			{
				codeArea.Select(match.Index + textStartIndex, match.Length);
				codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.variableColor);
				Console.WriteLine("++" + match.Value + "++");
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
			//semaphore.WaitOne();
			if (e.KeyChar == '(' || e.KeyChar == '[' || e.KeyChar == '{')
			{
				//deselectamo codeArea i skrijemo selection
				debug.Focus();
				SendMessage(codeArea.Handle, EM_HIDESELECTION, 1, 0);

				int selectedIndex = codeArea.SelectionStart;
				e.Handled = true;
				//semaphore.Release();
				wrongSelection = true;
				rightSelection = codeArea.SelectionStart;
				if (e.KeyChar == '(')
					codeArea.SelectedText = "()";
				else if (e.KeyChar == '[')
					codeArea.SelectedText = "[]";
				else
					codeArea.SelectedText = "{}";
				//semaphore.WaitOne();
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
							//semaphore.Release();
							wrongSelection = true;
							rightSelection = codeArea.SelectionStart;
							codeArea.Select(selected - currentWord.Length, currentWord.Length);
							codeArea.SelectedText = name;
							//semaphore.WaitOne();
							e.Handled = true;
							codeArea.SelectionStart = selected + (name.Length - currentWord.Length);

							//vratimo focus na code area i ponovno prikazemo selekciju
							codeArea.Focus();
							SendMessage(codeArea.Handle, EM_HIDESELECTION, 0, 0);
							//semaphore.Release();
							return;
						}
					}
				}
				//semaphore.Release();
				wrongSelection = true;
				rightSelection = codeArea.SelectionStart;
				codeArea.SelectedText = "    ";
				//semaphore.WaitOne();
				codeArea.SelectionStart += 4;
				e.Handled = true;

				//vratimo focus na code area i ponovno prikazemo selekciju
				codeArea.Focus();
				SendMessage(codeArea.Handle, EM_HIDESELECTION, 0, 0);
			}
			//semaphore.Release();
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
			SaveFile();
		}

		
	}
}
