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
        private String currentFileName = "";
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
            Close();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Check to see if data will be lost 
            if (spreadSheet.Changed)
            {
                if (MessageBox.Show("Save your changes before exit?", "Save changes?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    object sender = new object();
                    saveAsToolStripMenuItem_Click(sender, e);
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.getAppContext().RunForm(new Form1());
        }

        private void contentsInsert_Click(object sender, EventArgs e)
        {
            int row;
            int col;
            spreadsheetPanel1.GetSelection(out col, out row);

            // Get contents to be set as cell contents 
            String contents = cellContentsValue.Text;

            // Get name to be set as cell name 
            cellNameValue.Text = Char.ConvertFromUtf32(col + 65) + (row + 1).ToString();

            String cellName = cellNameValue.Text;

            try
            {
                // Add cell to spreadsheet
                IEnumerable<String> recalculate = spreadSheet.SetContentsOfCell(cellName, contents);

                // Recalculate cells 
                foreach (String cell in recalculate)
                {
                    String parsedCol = cell.Substring(0, 1);
                    String parsedRow = cell.Substring(1, cellName.Length - 1);

                    Char parsedChar = parsedCol[0];
                    int colNum = (int)parsedChar - 65;
                    int rowNum = Convert.ToInt32(parsedRow) - 1;
                    spreadsheetPanel1.SetValue(colNum, rowNum, spreadSheet.GetCellValue(cell).ToString());
                }

                // Get value of cell 
                String CellValue = spreadSheet.GetCellValue(cellName).ToString();
                String cellValue;
                Object cellContents;

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
            }
            catch (Formulas.FormulaFormatException fexception)
            {
                statusValue.Text = fexception.Message + "  (" + cellContentsValue.Text + ")";
                cellContentsValue.Clear();
            }
            catch (CircularException cexception)
            {
                statusValue.Text = cexception.Message + "  (" + cellContentsValue.Text + ")";
                cellContentsValue.Clear();
            }
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

                    try
                    {
                        // Add cell to spreadsheet
                        IEnumerable<String> recalculate = spreadSheet.SetContentsOfCell(cellName, contents);

                        // Recalculate cells 
                        foreach (String cell in recalculate)
                        {
                            String parsedCol = cell.Substring(0, 1);
                            String parsedRow = cell.Substring(1, cellName.Length - 1);

                            Char parsedChar = parsedCol[0];
                            int colNum = (int)parsedChar - 65;
                            int rowNum = Convert.ToInt32(parsedRow) - 1;

                            // String cellVal = spreadSheet.GetCellValue(cell).ToString();

                            spreadsheetPanel1.SetValue(colNum, rowNum, spreadSheet.GetCellValue(cell).ToString());
                            //  spreadsheetPanel1.

                        }

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
                        statusValue.Text = "Successful";
                    }
                    catch (Formulas.FormulaFormatException fexception )
                    {
                        statusValue.Text = fexception.Message + "  (" + cellContentsValue.Text + ")";
                        cellContentsValue.Clear();
                    }
                    catch (CircularException cexception)
                    {
                        statusValue.Text = cexception.Message + "  ("+cellContentsValue.Text + ")";
                        cellContentsValue.Clear();
                    }

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
                        statusValue.Text = "Successfull";
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
                    statusValue.Text = "Successfull";
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
                    statusValue.Text = "Successfull";
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
                    statusValue.Text = "Successfull";
                }
                break;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveAs = new SaveFileDialog();

            try
            {
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
                            currentFileName = saveAs.FileName;
                            writer.Close();
                        }
                    }
                }
            }
            catch (IOException ioexception)
            {
                MessageBox.Show("Problem saving file" + ioexception.Message);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "Spreadsheet File (*.ss)|*.ss|All Files |*.*";
            openFile.FilterIndex = 1;

            openFile.Multiselect = false;

            try
            {
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    StreamReader reader = new StreamReader(openFile.FileName);
                    AbstractSpreadsheet spreadsheet = new Spreadsheet(reader);
                    currentFileName = openFile.FileName;

                    IEnumerable<String> cells = spreadsheet.GetNamesOfAllNonemptyCells();
                    foreach (String cellName in cells)
                    {
                        String parsedCol = cellName.Substring(0, 1);
                        String parsedRow = cellName.Substring(1, cellName.Length - 1);

                        Char parsedChar = parsedCol[0];
                        int col = (int)parsedChar - 65;
                        int row = Convert.ToInt32(parsedRow) - 1;

                        spreadsheetPanel1.SetValue(col, row, spreadsheet.GetCellValue(cellName).ToString());
                        reader.Close();
                    }
                }
            }
            catch (SpreadsheetReadException rexception)
            {
                MessageBox.Show(rexception.Message);
                closeToolStripMenuItem_Click(sender, e); 
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(currentFileName))
            {
                SaveFileDialog saveAs = new SaveFileDialog();

           //     String fileName = Path.GetFileName(currentFileName);
                String xml = "";
                using (StreamWriter writer = File.CreateText(currentFileName))
               // using (StreamWriter writer2 = File.OpenWrite(fileName))
       //         using (StreamWriter writer = File.Open(fileName, FileMode.Open))
                {
                    spreadSheet.Save(writer);
                    xml = writer.ToString();
                    //currentFileName = saveAs.FileName;
                    writer.Close();
                }
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String message = "Controls for operating the spreadsheet: Use the arrow keys on the keyboard to naviage around the grid. ";
                   message += "Insert a value into the cell contents textbox and hit the insert button or press enter to insert data into the cell.  ";
                   message += "The file menu works similarly to how the file menu works for other programs. If you try to exit without saving changes, you will be prompted to save changes. ";
                   message += "Pressing the enter key without inserting cell contents will allow you to move down rows, but the column will stay the same. ";
                   message += "If you insert a formula into a cell that does not contain valid cell names, a FormulaError will be placed as the value of the cell. ";
                   message += "You cannot use the arrow keys to scroll over in a textbox. Using the arrow keys will select a different cell. You must use your mouse show the hidden data. ";
                   message += "Errors that do not appear in the spreadsheet panel will appear below the panel in the Status section. ";

            MessageBox.Show(message);
        }
    }
}
