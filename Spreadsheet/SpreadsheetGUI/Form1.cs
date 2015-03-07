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

            // Create variables for holding the cellValue and cellContents 
            String cellValue;
            Object cellContents;

            // Get the row and column values for the current selection 
            spreadsheetPanel1.GetSelection(out col, out row);

            // Get contents to be set as cell contents 
            String contents = cellContentsValue.Text;

            // Get name to be set as cell name 
            cellNameValue.Text = Char.ConvertFromUtf32(col + 65) + (row + 1).ToString();

            // Set the cellNameValue textbox to hold the cellName 
            String cellName = cellNameValue.Text;

            // Try to insert the cell into the spreadsheet 
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
            // Catch any FormulaFormatExceptions and set the spreadsheet status to show the error 
            catch (Formulas.FormulaFormatException fexception)
            {
                statusValue.Text = fexception.Message + "  (" + cellContentsValue.Text + ")";
                cellContentsValue.Clear();
            }
            // Catch any CircularExceptions and set the spreadsheet status to show the error 
            catch (CircularException cexception)
            {
                statusValue.Text = cexception.Message + "  (" + cellContentsValue.Text + ")";
                cellContentsValue.Clear();
            }
        }

        /// <summary>
        /// Method is not used, but too scared to delete it since strange things happen when you delete these methods. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsInsert_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        /// <summary>
        /// This method handles updating the cell's information when navigating around the spreadsheet grid using the arrow keys on the keyboard. 
        /// It also allows the user to insert information into a cell by writing in the cellContentsValue text box and pressing enter instead of the insert button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cellContentsValue_KeyDown(object sender, KeyEventArgs e)
        {
            // Create variables for holding row and column values 
            int row;
            int col;

            // Get cell coordinates 
            spreadsheetPanel1.GetSelection(out col, out row);

            // Create varaible sfor holding the cellValue and cellContents 
            String cellValue;
            Object cellContents;

            // Start switch statement for determining which key is pressed on the keyboard 
            switch (e.KeyData)
            {
                // Switch case -- enter key has been pressed 
                case Keys.Enter:
                    // Get the row and column values for the current selection 
                    spreadsheetPanel1.GetSelection(out col, out row);

                    // Get contents to be set as cell contents 
                    String contents = cellContentsValue.Text;

                    // Get name to be set as cell name 
                    cellNameValue.Text = Char.ConvertFromUtf32(col + 65) + (row + 1).ToString();

                    // Set the cellNameValue textbox to hold the cellName 
                    String cellName = cellNameValue.Text;

                    // Try to insert the cell into the spreadsheet 
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
                    // Catch any FormulaFormatExceptions and set the spreadsheet status to show the error 
                    catch (Formulas.FormulaFormatException fexception)
                    {
                        statusValue.Text = fexception.Message + "  (" + cellContentsValue.Text + ")";
                        cellContentsValue.Clear();
                    }
                    // Catch any CircularExceptions and set the spreadsheet status to show the error 
                    catch (CircularException cexception)
                    {
                        statusValue.Text = cexception.Message + "  (" + cellContentsValue.Text + ")";
                        cellContentsValue.Clear();
                    }
                    break;

                // Switch case -- Right Arrow Key 
                case Keys.Right:
                    // Check to see if we will be going out of bounds of the grid 
                    if (col < 25)
                    {
                        // Move the cell selection one column to the right 
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
                
                // Switch case -- Down arrow key 
                case Keys.Down:
                    // Check to see if we will be going out of bounds of the grid  
                    if (row < 98)
                    {
                        // Move the cell selection one row down 
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

                // Switch case -- Left arrow key 
                case Keys.Left:
                    // Check to see if we will be going out of bounds of the grid 
                    if (col >= 1)
                    {
                        // Move the cell selection one column to the left 
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
                
                // Switch case -- Up arrow key 
                case Keys.Up:
                    // Check to see if we will be going out of bounds of the grid 
                    if (row >= 1)
                    {
                        // Move the cell selection one row up 
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

        /// <summary>
        /// Method for handling the SaveAs button click. 
        /// This method will open a SaveAs dialog box for the user to select where they want to save the file. 
        /// Once the user presses Save, the spreadsheet will be saved to the XML file they specified
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create new SaveFileDialog object 
            SaveFileDialog saveAs = new SaveFileDialog();

            // Try to perform the save process 
            try
            {
                    // Set the saveAs filter to show just .ss files by default, but give option to allow all files to be shown 
                    saveAs.Filter = "Spreadsheet File (*.ss)|*.ss|All Files |*.*";
                    // Set the default file view 
                    saveAs.FilterIndex = 1;
                    saveAs.RestoreDirectory = true;

                    // If the user presses save, perform the file writing 
                    if (saveAs.ShowDialog() == DialogResult.OK)
                    {
                        String xml = "";
                        // Create a new StreamWriter and file for writing the object 
                        using (StreamWriter writer = File.CreateText(saveAs.FileName))
                        {
                            // Write the spreadsheet 
                            spreadSheet.Save(writer);
                            xml = writer.ToString();
                            // Set the instance variable to remember the file name 
                            currentFileName = saveAs.FileName;
                            // Close the file writer 
                            writer.Close();
                        }
                    }
            }
            // Catch the save exceptions and show a dialog box with the error 
            catch (IOException ioexception)
            {
                MessageBox.Show("Problem saving file" + ioexception.Message);
            }
        }

        /// <summary>
        /// Method for handling the opening of files when the Open button is clicked. 
        /// When a file is opened, all previous data in the current spreadsheet is lost and replaced with the data from the file being opened. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create new OpenFileDialog object 
            OpenFileDialog openFile = new OpenFileDialog();

            // Set filter for which files can be shown 
            openFile.Filter = "Spreadsheet File (*.ss)|*.ss|All Files |*.*";
            // Set default filter to .ss files 
            openFile.FilterIndex = 1;
            openFile.Multiselect = false;

            // Try to open the file 
            try
            {
                // If Open has been selected 
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    // Read in the file 
                    StreamReader reader = new StreamReader(openFile.FileName);
                    // Replace the old spreadSheet object with the new one created by the file opener 
                    this.spreadSheet = new Spreadsheet(reader);
                    // Clear the spreadsheet panel to get the old cells off of it 
                    spreadsheetPanel1.Clear();
                    // Update the fileName in the instance variable 
                    currentFileName = openFile.FileName;

                    // Add all of the cells in the spreadsheet into the spreadsheet panel 
                    IEnumerable<String> cells = spreadSheet.GetNamesOfAllNonemptyCells();
                    foreach (String cellName in cells)
                    {
                        // Parse cell's row and column information 
                        String parsedCol = cellName.Substring(0, 1);
                        String parsedRow = cellName.Substring(1, cellName.Length - 1);

                        Char parsedChar = parsedCol[0];
                        int col = (int)parsedChar - 65;
                        int row = Convert.ToInt32(parsedRow) - 1;

                        // Add cell into the spreadsheet panel 
                        spreadsheetPanel1.SetValue(col, row, spreadSheet.GetCellValue(cellName).ToString());

                        // Set the values for the default selected cell A1 
                        cellContentsValue.Text = spreadSheet.GetCellContents("A1").ToString();
                        cellValueBox.Text = spreadSheet.GetCellValue("A1").ToString();
                        spreadsheetPanel1.SetSelection(0, 0);
                    }

                    // Close the reader 
                    reader.Close();
                }
            }
            // Catch any SpreadsheetReadExceptions and show the exception in a dialog box 
            catch (SpreadsheetReadException rexception)
            {
                MessageBox.Show(rexception.Message);
                closeToolStripMenuItem_Click(sender, e);
            }
        }

        /// <summary>
        /// Method for handling the saving for the spreadsheet. 
        /// This method is the alternative way of saving the spreadsheet when the user doesn't want to go through the Save As process. 
        /// If the spreadsheet has never been saved before, the Save As dailog box will appear. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If the spreadsheet has been saved before, skip the Save As dialog process and save the spreadsheet to the file used before. 
            if (!String.IsNullOrEmpty(currentFileName))
            {
                // Create new SaveFialDialog object 
                SaveFileDialog saveAs = new SaveFileDialog();
                String xml = "";
                // Create StreamWriter and open the file for saving 
                using (StreamWriter writer = File.CreateText(currentFileName))
                {
                    // Perform the save 
                    spreadSheet.Save(writer);
                    xml = writer.ToString();
                    // Close the writer 
                    writer.Close();
                }
            }
            // If the spreadsheet hasn't been saved before, perform the Save As process 
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        /// <summary>
        /// Method for showing the help information when the Help button is clicked. 
        /// The dialog box shown will reveal how to use the basic controls of the spreadsheet. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create string for holding the directions 
            String message = "Controls for operating the spreadsheet: Use the arrow keys on the keyboard to naviage around the grid. ";
            message += "Insert a value into the cell contents textbox and hit the insert button or press enter to insert data into the cell.  ";
            message += "The file menu works similarly to how the file menu works for other programs. If you try to exit without saving changes, you will be prompted to save changes. ";
            message += "Pressing the enter key without inserting cell contents will allow you to move down rows, but the column will stay the same. ";
            message += "If you insert a formula into a cell that does not contain valid cell names, a FormulaError will be placed as the value of the cell. ";
            message += "You cannot use the arrow keys to scroll over in a textbox. Using the arrow keys will select a different cell. You must use your mouse show the hidden data. ";
            message += "Errors that do not appear in the spreadsheet panel will appear below the panel in the Status section. ";
            message += "To delete a cell, remove the cell's contents and click Insert or press Enter";

            // Show message box with the directions 
            MessageBox.Show(message);
        }
    }
}
