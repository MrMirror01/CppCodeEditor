using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


		String savePath = "";
		String deafultSaveLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

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
			colorKeywords();
		}

		private void colorKeywords()
		{
			int selectedIndex = codeArea.SelectionStart;

			//deselectamo codeArea i skrijemo selection
			debug.Focus();
			SendMessage(codeArea.Handle, EM_HIDESELECTION, 1, 0);


			//sva slova pobjamo u deafult boju
			codeArea.SelectAll();
			codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.textColor);

			// formira regex oblika "\b(keyword1|keyword2|keyword3)\b" 
			Regex keywordRegex = new Regex(@"\b(" + string.Join("|", Syntax.syntax.keywordInfo.keywords) + @")\b");
			MatchCollection foundKeywords = keywordRegex.Matches(codeArea.Text);
			//pobojamo sve kljucne rjeci
			foreach (Match match in foundKeywords)
			{
				codeArea.Select(match.Index,  match.Length);
				codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.keywordInfo.color);
			}

			//detektiramo sve specialne znakove
			string specias = @"(" + string.Join("|", Syntax.syntax.specialsInfo.specials) + @")";
			Regex specialsRegex = new Regex(specias);
			MatchCollection foundSpecials = specialsRegex.Matches(codeArea.Text);
			//pobojamo sve kljucne rjeci
			foreach (Match match in foundSpecials)
			{
				codeArea.Select(match.Index, match.Length);
				codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.specialsInfo.color);
			}

			//pobojamo sve wrappere
			foreach (var item in Syntax.syntax.wrappers)
			{
				Wrapper wrapper = item.Value;
				Regex wrapperRegex = new Regex("(" + wrapper.start + "(.|\n)*?(" + wrapper.end + "))");
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

			//resetiramo poziciju i boju selekcije
			codeArea.SelectionStart = selectedIndex;
			codeArea.SelectionLength = 0;
			codeArea.SelectionColor = ColorTranslator.FromHtml(Syntax.syntax.textColor);
		}

		private void codeArea_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '(' || e.KeyChar == '[' || e.KeyChar == '{')
			{
				int selectedIndex = codeArea.SelectionStart;
				e.Handled = true;
				if (e.KeyChar == '(')
					codeArea.Text = codeArea.Text.Insert(codeArea.SelectionStart, "()");
				else if (e.KeyChar == '[')
					codeArea.Text = codeArea.Text.Insert(codeArea.SelectionStart, "[]");
				else
					codeArea.Text = codeArea.Text.Insert(codeArea.SelectionStart, "{}");
				codeArea.SelectionStart = selectedIndex + 1;
				codeArea.SelectionLength = 0;
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
