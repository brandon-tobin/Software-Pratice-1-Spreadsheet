using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            columnValue.Text = "A";
            rowValue.Text = 1.ToString();

            spreadsheetPanel1.SelectionChanged += displaySelection;
        }

        private void displaySelection(SpreadsheetPanel sheet)
        {
            int row, col;
            String value;
            sheet.GetSelection(out col, out row);
            sheet.GetValue(col, row, out value);

            int asciiCol = col + 65;
            columnValue.Text = Char.ConvertFromUtf32(asciiCol);

            int actualRow = row + 1;
            rowValue.Text = actualRow.ToString();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.getAppContext().RunForm(new Form1());
        }

        
    }
}
