
namespace CppCodeEditor
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buildToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buildAndRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.codeArea = new System.Windows.Forms.RichTextBox();
			this.codeAreaPanel = new System.Windows.Forms.Panel();
			this.LineNumberTextBox = new System.Windows.Forms.RichTextBox();
			this.debug = new System.Windows.Forms.Label();
			this.compilerOutputBox = new System.Windows.Forms.RichTextBox();
			this.menuStrip1.SuspendLayout();
			this.codeAreaPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.buildToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1884, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.saveAsToolStripMenuItem.Text = "Save as";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openBtton_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// buildToolStripMenuItem
			// 
			this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildToolStripMenuItem1,
            this.runToolStripMenuItem,
            this.buildAndRunToolStripMenuItem});
			this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
			this.buildToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
			this.buildToolStripMenuItem.Text = "Build";
			// 
			// buildToolStripMenuItem1
			// 
			this.buildToolStripMenuItem1.Name = "buildToolStripMenuItem1";
			this.buildToolStripMenuItem1.Size = new System.Drawing.Size(145, 22);
			this.buildToolStripMenuItem1.Text = "Build";
			this.buildToolStripMenuItem1.Click += new System.EventHandler(this.buildButton_Click);
			// 
			// runToolStripMenuItem
			// 
			this.runToolStripMenuItem.Name = "runToolStripMenuItem";
			this.runToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
			this.runToolStripMenuItem.Text = "Run";
			this.runToolStripMenuItem.Click += new System.EventHandler(this.runButton_Click);
			// 
			// buildAndRunToolStripMenuItem
			// 
			this.buildAndRunToolStripMenuItem.Name = "buildAndRunToolStripMenuItem";
			this.buildAndRunToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
			this.buildAndRunToolStripMenuItem.Text = "Build and run";
			this.buildAndRunToolStripMenuItem.Click += new System.EventHandler(this.buildAndRunButton_Click);
			// 
			// codeArea
			// 
			this.codeArea.AcceptsTab = true;
			this.codeArea.BackColor = System.Drawing.Color.WhiteSmoke;
			this.codeArea.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.codeArea.ForeColor = System.Drawing.Color.Black;
			this.codeArea.Location = new System.Drawing.Point(58, 3);
			this.codeArea.Name = "codeArea";
			this.codeArea.Size = new System.Drawing.Size(1788, 701);
			this.codeArea.TabIndex = 0;
			this.codeArea.Text = "";
			this.codeArea.WordWrap = false;
			this.codeArea.VScroll += new System.EventHandler(this.codeArea_VScroll);
			this.codeArea.TextChanged += new System.EventHandler(this.codeArea_TextChanged);
			this.codeArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.codeArea_KeyDown);
			this.codeArea.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.codeArea_KeyPress);
			// 
			// codeAreaPanel
			// 
			this.codeAreaPanel.Controls.Add(this.LineNumberTextBox);
			this.codeAreaPanel.Controls.Add(this.codeArea);
			this.codeAreaPanel.Location = new System.Drawing.Point(0, 38);
			this.codeAreaPanel.Name = "codeAreaPanel";
			this.codeAreaPanel.Size = new System.Drawing.Size(1846, 706);
			this.codeAreaPanel.TabIndex = 1;
			// 
			// LineNumberTextBox
			// 
			this.LineNumberTextBox.Enabled = false;
			this.LineNumberTextBox.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LineNumberTextBox.Location = new System.Drawing.Point(3, 3);
			this.LineNumberTextBox.Name = "LineNumberTextBox";
			this.LineNumberTextBox.Size = new System.Drawing.Size(49, 703);
			this.LineNumberTextBox.TabIndex = 1;
			this.LineNumberTextBox.Text = "";
			this.LineNumberTextBox.WordWrap = false;
			// 
			// debug
			// 
			this.debug.AutoSize = true;
			this.debug.Location = new System.Drawing.Point(12, 22);
			this.debug.Name = "debug";
			this.debug.Size = new System.Drawing.Size(0, 13);
			this.debug.TabIndex = 1;
			// 
			// compilerOutputBox
			// 
			this.compilerOutputBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.compilerOutputBox.Location = new System.Drawing.Point(3, 750);
			this.compilerOutputBox.Name = "compilerOutputBox";
			this.compilerOutputBox.ReadOnly = true;
			this.compilerOutputBox.Size = new System.Drawing.Size(1843, 190);
			this.compilerOutputBox.TabIndex = 1;
			this.compilerOutputBox.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1884, 961);
			this.Controls.Add(this.compilerOutputBox);
			this.Controls.Add(this.debug);
			this.Controls.Add(this.codeAreaPanel);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.codeAreaPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem buildAndRunToolStripMenuItem;
		private System.Windows.Forms.RichTextBox codeArea;
		private System.Windows.Forms.Panel codeAreaPanel;
		private System.Windows.Forms.Label debug;
		private System.Windows.Forms.RichTextBox compilerOutputBox;
		private System.Windows.Forms.RichTextBox LineNumberTextBox;
	}
}

