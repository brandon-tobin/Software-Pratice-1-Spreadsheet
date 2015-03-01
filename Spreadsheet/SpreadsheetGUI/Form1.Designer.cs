namespace SS
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
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colName = new System.Windows.Forms.Label();
            this.columnName = new System.Windows.Forms.Label();
            this.columnValue = new System.Windows.Forms.TextBox();
            this.rowName = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 87);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1372, 778);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1372, 42);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 38);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(187, 36);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 36);
            this.openToolStripMenuItem.Text = "Open...";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(187, 36);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(187, 36);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(187, 36);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // colName
            // 
            this.colName.AutoSize = true;
            this.colName.Location = new System.Drawing.Point(0, 841);
            this.colName.Name = "colName";
            this.colName.Size = new System.Drawing.Size(0, 25);
            this.colName.TabIndex = 3;
            // 
            // columnName
            // 
            this.columnName.AutoSize = true;
            this.columnName.Location = new System.Drawing.Point(12, 56);
            this.columnName.Name = "columnName";
            this.columnName.Size = new System.Drawing.Size(91, 25);
            this.columnName.TabIndex = 4;
            this.columnName.Text = "Column:";
            // 
            // columnValue
            // 
            this.columnValue.Location = new System.Drawing.Point(109, 50);
            this.columnValue.Name = "columnValue";
            this.columnValue.Size = new System.Drawing.Size(100, 31);
            this.columnValue.TabIndex = 5;
            // 
            // rowName
            // 
            this.rowName.AutoSize = true;
            this.rowName.Location = new System.Drawing.Point(248, 56);
            this.rowName.Name = "rowName";
            this.rowName.Size = new System.Drawing.Size(60, 25);
            this.rowName.TabIndex = 6;
            this.rowName.Text = "Row:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(315, 49);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 31);
            this.textBox1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1372, 865);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rowName);
            this.Controls.Add(this.columnValue);
            this.Controls.Add(this.columnName);
            this.Controls.Add(this.colName);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Label colName;
        private System.Windows.Forms.Label columnName;
        private System.Windows.Forms.TextBox columnValue;
        private System.Windows.Forms.Label rowName;
        private System.Windows.Forms.TextBox textBox1;
    }
}

