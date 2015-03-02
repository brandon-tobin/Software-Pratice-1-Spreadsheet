using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SS
{
    public partial class Form1 : Form
    {
        private AbstractSpreadsheet spreadSheet;
        public Form1()
        {
            InitializeComponent();

            columnValue.Text = "A";
            rowValue.Text = 1.ToString();
            cellNameValue.Text = "A" + 1.ToString();

            spreadSheet = new Spreadsheet();

            spreadsheetPanel1.SelectionChanged += displaySelection;
        }

        private void displaySelection(SpreadsheetPanel sheet)
        {
            int row;
            int col;
            String value;
            sheet.GetSelection(out col, out row);
            sheet.GetValue(col, row, out value);

            int asciiCol = col + 65;
            columnValue.Text = Char.ConvertFromUtf32(asciiCol);

            int actualRow = row + 1;
            rowValue.Text = actualRow.ToString();

            cellNameValue.Text = Char.ConvertFromUtf32(asciiCol) + actualRow.ToString();
           
           String cellContents = spreadSheet.GetCellContents(cellNameValue.Text).ToString();
            cellContentsValue.Text = cellContents;

            String cellValue = spreadSheet.GetCellValue(cellNameValue.Text).ToString();
            cellValueBox.Text = cellValue;
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

        private void contentsInsert_Click(object sender, EventArgs e)
        {
            int row;
            int col;
            // Get cell coordinates 
            spreadsheetPanel1.GetSelection(out col, out row);

            // Get contents to be set as cell contents 
            String contents = cellContentsValue.Text;

            // Get name to be set as cell name 
            String cellName = cellNameValue.Text;

            // Add cell to spreadsheet 
            spreadSheet.SetContentsOfCell(cellName, contents);

            // Get value of cell 
            String CellValue = spreadSheet.GetCellValue(cellName).ToString();

            // Display value of cell in spreadsheetpanel 
            spreadsheetPanel1.SetValue(col, row, CellValue);

            // Move selection down by one row 
            spreadsheetPanel1.SetSelection(col, row + 1);

            cellContentsValue.Clear();
        }

        private void contentsInsert_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void cellContentsValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                int row;
                int col;
                // Get cell coordinates 
                spreadsheetPanel1.GetSelection(out col, out row);

                // Get contents to be set as cell contents 
                String contents = cellContentsValue.Text;
               
                // Get name to be set as cell name 
                String cellName = cellNameValue.Text;

                // Add cell to spreadsheet 
                spreadSheet.SetContentsOfCell(cellName, contents);

                // Get value of cell 
                String CellValue = spreadSheet.GetCellValue(cellName).ToString();

                // Display value of cell in spreadsheetpanel 
                spreadsheetPanel1.SetValue(col, row, CellValue);

                // Move selection down by one row 
                spreadsheetPanel1.SetSelection(col, row + 1);
                
                cellContentsValue.Clear();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveAs = new SaveFileDialog();

            saveAs.Filter = "Spreadsheet File (*.ss)|*.ss|All Files |*.*";
            saveAs.FilterIndex = 1;
            saveAs.RestoreDirectory = true;

            if (saveAs.ShowDialog() == DialogResult.OK)
            {
                String xml = "";
                using (StreamWriter writer = File.CreateText(saveAs.FileName))
                {
                    spreadSheet.Save(writer);
                    xml = writer.ToString();
                }     
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
