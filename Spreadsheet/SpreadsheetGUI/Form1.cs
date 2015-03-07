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
    /// <summary>
    /// This class handles all of the mapping between the Spreadsheet GUI and the internal components of the spreadsheet. 
    /// </summary>
    public partial class Form1 : Form
    {
        // Create a spreadSheet object for holding the spreadsheet 
        private AbstractSpreadsheet spreadSheet;
        // Create a string instance variable for keeping track of the file path for determining if save or save as should be offered 
        private String currentFileName = "";

        /// <summary>
        /// Method for starting and initializing the form 
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // Set the columnValue text box to be A when the form starts 
            columnValue.Text = "A";
            // Set the rowValue text box to be 1 when the form starts 
            rowValue.Text = 1.ToString();
            // Set the cellNameValue text box to be A1 when the form starts 
            cellNameValue.Text = "A" + 1.ToString();

            // Initialize the spreadSheet when the form starts 
            spreadSheet = new Spreadsheet();

            // Keep track of when the spreadSheet changes selection and call displaySelection when it does to update the spreadsheet panel 
            spreadsheetPanel1.SelectionChanged += displaySelection;
        }

        /// <summary>
        /// Method for setting the focus for the spreadSheet GUI 
        /// This method will make sure the insertion bar is always in the cellContentsValue text box to make inserting into cells more seamless 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            cellContentsValue.Focus();
        }

        /// <summary>
        /// Helper method for updating the cell's value boxes when the selection changes 
        /// This method correctly sets the value textboxes: cellNameValue, columnValue, rowValue, cellContentsValue, and the cellValueBox textboxes 
        /// </summary>
        /// <param name="sheet"></param>
        private void displaySelection(SpreadsheetPanel sheet)
        {
            // Create variables for holding the row, column, and cell values 
            int row;
            int col;
            String value;

            // Get the current selection 
            sheet.GetSelection(out col, out row);
            // Get the cell's value once the selection is obtained 
            sheet.GetValue(col, row, out value);

            // Add ascii offset to the column number 
            int asciiCol = col + 65;
            // Convert the ascii value into a char and set it as the columnValue text box 
            columnValue.Text = Char.ConvertFromUtf32(asciiCol);

            // Add 1 to the row number since rows and columns starting index is 0
            int actualRow = row + 1;
            // Convert row number to a string and store it in the rowValue text box 
            rowValue.Text = actualRow.ToString();

            // Combine the column and row values as a string and set them as the cellNameValue text box
            cellNameValue.Text = Char.ConvertFromUtf32(asciiCol) + actualRow.ToString();

            // Get the contents of the cell and convert it to a string 
            String cellContents = spreadSheet.GetCellContents(cellNameValue.Text).ToString();
            // Set the cell's contents in the cellContentsValue text box 
            cellContentsValue.Text = cellContents;

            // Get the vlaue of the cell and convert it to a string 
            String cellValue = spreadSheet.GetCellValue(cellNameValue.Text).ToString();
            // Set the clel's value in the cellValueBox textbox 
            cellValueBox.Text = cellValue;
        }

        /// <summary>
        /// Method is not used, but too scared to delete it since strange things happen when you delete these methods. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Method for handling the Close button click 
        /// This method initiates the form closing proceedures 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Overrided method for changing how the closing of the form happens. 
        /// This method will prompt the user to save changes if the spreadsheet has been changed. 
        /// If the spreadsheet hasn't been changed, it will simply close the spreadsheet window 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Check to see if data will be lost 
            if (spreadSheet.Changed)
            {
                // Prompt user to see if save should take plce 
                if (MessageBox.Show("Would you like to save your changes before exit?", "Save changes?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Call method for performing save (Not SaveAs)
                    Object sender = new Object();
                    saveToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    // If user doesn't want to save, return and close form 
                    return;
                }
            }
            else
            {
                // If the spreadsheet hasn't changed, return and close form 
                return;
            }
        }

        /// <summary>
        /// This method handles creating a new spreadsheet window when the New button is clicked. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.getAppContext().RunForm(new Form1());
        }

        /// <summary>
        /// This method handles inserting data into the cell form the cellContentsValue textbox when clicked. 
        /// This method will also update the cell's to display their values after insertion takes place. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsInsert_Click(object sender, EventArgs e)
        {
            // Create variables for holding the row and column numbers 
            int row;
            int col;

            // Get cell coordinates 
            spreadsheetPanel1.GetSelection(out col, out row);

            // Create variables for holding the value and contents of the cell 
            String cellValue;
            Object cellContents;

            // Get contents to be set as cell contents 
            String contents = cellContentsValue.Text;

            // Get name to be set as cell name 
            cellNameValue.Text = Char.ConvertFromUtf32(col + 65) + (row + 1).ToString();

            // Get the cellName from the cellNameValue text box 
            String cellName = cellNameValue.Text;

            // Try to insert the value into the spreadsheet cell 
            try
            {
                // Add cell to spreadsheet and store the cells that need to be recalculated 
                IEnumerable<String> recalculate = spreadSheet.SetContentsOfCell(cellName, contents);
               
                // Loop through the cells that need to be recalculated recalculating their values 
                foreach (String cell in recalculate)
                {
                    // Parse the column out of the cell name 
                    String parsedCol = cell.Substring(0, 1);
                    // Parse the row out of the cell name 
                    String parsedRow = cell.Substring(1, cellName.Length - 1);

                    // Convert the column into a char 
                    Char parsedChar = parsedCol[0];
                    // Convert the column into an int 
                    int colNum = (int)parsedChar - 65;
                    // Convert the row into an int 
                    int rowNum = Convert.ToInt32(parsedRow) - 1;

                    // Set the value of the cell into the spreadsheet panel 
                    spreadsheetPanel1.SetValue(colNum, rowNum, spreadSheet.GetCellValue(cell).ToString());
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
                // Set the spreadsheet status to successful since the cell was successfully inserted 
                statusValue.Text = "Successful";
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
                        if (recalculate.Count() == 0)
                        {
                            recalculate = spreadSheet.GetNamesOfAllNonemptyCells();
                        }
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
                    this.spreadSheet = new Spreadsheet(reader);
                    spreadsheetPanel1.Clear();
                    currentFileName = openFile.FileName;

                    IEnumerable<String> cells = spreadSheet.GetNamesOfAllNonemptyCells();
                    foreach (String cellName in cells)
                    {
                        String parsedCol = cellName.Substring(0, 1);
                        String parsedRow = cellName.Substring(1, cellName.Length - 1);

                        Char parsedChar = parsedCol[0];
                        int col = (int)parsedChar - 65;
                        int row = Convert.ToInt32(parsedRow) - 1;

                        spreadsheetPanel1.SetValue(col, row, spreadSheet.GetCellValue(cellName).ToString());
                        reader.Close();


                        cellContentsValue.Text = spreadSheet.GetCellContents("A1").ToString();
                        cellValueBox.Text = spreadSheet.GetCellValue("A1").ToString();
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
                String xml = "";
                using (StreamWriter writer = File.CreateText(currentFileName))
                {
                    spreadSheet.Save(writer);
                    xml = writer.ToString();
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
            message += "To delete a cell, remove the cell's contents and click Insert or press Enter";

            MessageBox.Show(message);
        }
    }
}
