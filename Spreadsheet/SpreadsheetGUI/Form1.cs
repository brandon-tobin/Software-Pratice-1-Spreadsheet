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

        protected override void OnShown(EventArgs e)
        {
            cellContentsValue.Focus();
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
            // Check to see if data will be lost 
            if (spreadSheet.Changed)
            {
               if (MessageBox.Show("Save your changes before exit?", "Save changes?", MessageBoxButtons.YesNoCancel) == DialogResult.OK) 
                {
                    saveAsToolStripMenuItem_Click(sender, e);
               }
            }
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
            try
            {
                spreadSheet.SetContentsOfCell(cellName, contents);
                // Get value of cell 
                String cellValue = spreadSheet.GetCellValue(cellName).ToString();

                // Display value of cell in spreadsheetpanel 
                spreadsheetPanel1.SetValue(col, row, cellValue);
            }
            catch (CircularException ex)
            {
                String cellValue = ex.ToString();
                spreadsheetPanel1.SetValue(col, row, cellValue);
            }
           

            // Move selection down by one row 
            spreadsheetPanel1.SetSelection(col, row + 1);

            cellContentsValue.Clear();
        }

        private void contentsInsert_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void cellContentsValue_KeyDown(object sender, KeyEventArgs e)
        {
            int row;
            int col;
            // Get cell coordinates 
            spreadsheetPanel1.GetSelection(out col, out row);

            String cellValue;
            Object cellContents;
            
            switch (e.KeyData)
            {
                case Keys.Enter:

                    spreadsheetPanel1.GetSelection(out col, out row);

                    // Get contents to be set as cell contents 
                    String contents = cellContentsValue.Text;

                    // Get name to be set as cell name 
                    cellNameValue.Text = Char.ConvertFromUtf32(col + 65) + (row + 1).ToString();
                     
                    String cellName = cellNameValue.Text;

                    // Add cell to spreadsheet 
                    spreadSheet.SetContentsOfCell(cellName, contents);

                    // Get value of cell 
                    String CellValue = spreadSheet.GetCellValue(cellName).ToString();

                    // Display value of cell in spreadsheetpanel 
                    spreadsheetPanel1.SetValue(col, row, CellValue);

                    // Move selection down by one row 
                    spreadsheetPanel1.SetSelection(col, row + 1);

                    // Update row textbox 
                    rowValue.Text = (row + 2).ToString();
                    // Update cellName textbox
                    cellNameValue.Text = Char.ConvertFromUtf32(col + 65) + (row + 2).ToString();
                    // Update cellValue textbox
                    spreadsheetPanel1.GetValue(col, row + 1, out cellValue);
                    cellValueBox.Text = cellValue;
                    // Update cellContents textobx 
                    cellContents = spreadSheet.GetCellContents(cellNameValue.Text);
                    cellContentsValue.Text = cellContents.ToString();

                    break;

                case Keys.Right:
                    if (col < 25)
                    {
                        spreadsheetPanel1.SetSelection(col + 1, row);
                        // Update column textbox 
                        int asciiCol = col + 1 + 65;
                        columnValue.Text = Char.ConvertFromUtf32(asciiCol);
                        // Update cellName textbox 
                        cellNameValue.Text = Char.ConvertFromUtf32(asciiCol) + (row + 1).ToString();
                        // Update cellValue textbox
                        spreadsheetPanel1.GetValue(col + 1, row, out cellValue);
                        cellValueBox.Text = cellValue;
                        // Update cellContents textobx 
                        cellContents = spreadSheet.GetCellContents(cellNameValue.Text);
                        cellContentsValue.Text = cellContents.ToString();
                    }
                break;

                case Keys.Down:
                if (row < 98)
                {
                    spreadsheetPanel1.SetSelection(col, row + 1);
                    // Update row textbox 
                    rowValue.Text = (row + 2).ToString();
                    // Update cellName textbox
                    cellNameValue.Text = Char.ConvertFromUtf32(col + 65) + (row + 2).ToString();
                    // Update cellValue textbox
                    spreadsheetPanel1.GetValue(col, row + 1, out cellValue);
                    cellValueBox.Text = cellValue;
                    // Update cellContents textobx 
                    cellContents = spreadSheet.GetCellContents(cellNameValue.Text);
                    cellContentsValue.Text = cellContents.ToString();
                }
                break;

                case Keys.Left:
                if (col >= 1)
                {
                    spreadsheetPanel1.SetSelection(col - 1, row);
                    // Update column textbox 
                    int asciiCol2 = col - 1 + 65;
                    columnValue.Text = Char.ConvertFromUtf32(asciiCol2);
                    // Update cellName textbox 
                    cellNameValue.Text = Char.ConvertFromUtf32(asciiCol2) + (row + 1).ToString();
                    // Update cellValue textbox
                    spreadsheetPanel1.GetValue(col - 1, row, out cellValue);
                    cellValueBox.Text = cellValue;
                    // Update cellContents textobx 
                    cellContents = spreadSheet.GetCellContents(cellNameValue.Text);
                    cellContentsValue.Text = cellContents.ToString();
                }
                break;

                case Keys.Up:
                if (row >= 1)
                {
                    spreadsheetPanel1.SetSelection(col, row - 1);
                    // Update row textbox 
                    rowValue.Text = (row).ToString();
                    // Update cellName textbox 
                    cellNameValue.Text = char.ConvertFromUtf32(col + 65) + (row).ToString();
                    // Update cellValue textbox
                    spreadsheetPanel1.GetValue(col, row - 1, out cellValue);
                    cellValueBox.Text = cellValue;
                    // Update cellContents textobx 
                    cellContents = spreadSheet.GetCellContents(cellNameValue.Text);
                    cellContentsValue.Text = cellContents.ToString();
                }
                break;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveAs = new SaveFileDialog();

            if (saveAs.FileName == "")
            {
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
            else
            {
                saveAs.CheckFileExists = false;
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
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "Spreadsheet File (*.ss)|*.ss|All Files |*.*";
            openFile.FilterIndex = 1;

            openFile.Multiselect = false;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(openFile.FileName);
                AbstractSpreadsheet spreadsheet = new Spreadsheet(reader);

                IEnumerable<String> cells = spreadsheet.GetNamesOfAllNonemptyCells();
                foreach (String cellName in cells)
                {
                    String parsedCol = cellName.Substring(0, 1);
                    String parsedRow = cellName.Substring(1, cellName.Length - 1);

                    Char parsedChar = parsedCol[0];
                    int col = (int)parsedChar - 65;
                    int row = Convert.ToInt32(parsedRow) - 1;

                    spreadsheetPanel1.SetValue(col, row, spreadsheet.GetCellValue(cellName).ToString());
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsToolStripMenuItem_Click(sender, e);
        }
    }
}
