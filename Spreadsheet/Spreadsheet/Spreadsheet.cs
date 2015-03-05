using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        // Instance variables 
        // spreadsheetCells will represent the link between a Cell object and its name
        Dictionary<String, Cell> spreadsheetCells;
        // dependencies will act as a dependnecy graph for keeping track of dependees and dependents
        DependencyGraph dependencies;
        // isValidRegex will hold the regex sent in by one of the constructors 
        Regex isValidRegex;

        /// <summary>
        /// Constructor for Spreadsheet to create a new spreadsheetCells dictionary 
        /// and a dependencies dependency graph object. 
        /// This constructor creates an empty Spreadsheet whose IsValid regular expression accepts
        /// every string. 
        /// </summary>
        public Spreadsheet()
        {
            // Create an empty spreadsheetCells dictionary 
            spreadsheetCells = new Dictionary<String, Cell>();
            // Create an empty dependencies DG 
            dependencies = new DependencyGraph();
            // Set isValidRegex to allow every string 
            isValidRegex = new Regex(@"[\s\S]");
            // Set Chagned equal to false 
            Changed = false;
        }

        /// <summary>
        /// Constructor for Spreadsheet to create a new spreadsheetCells dictionary, a
        /// dependencies dependency graph object, and a isValidRegex regex object.
        /// This constructor creates an empty Spreadsheet whose IsValid regular expression is provided
        /// as the parameter. 
        /// </summary>
        public Spreadsheet(Regex isValid)
        {
            // Create an empty spreadsheetCells dictionary 
            spreadsheetCells = new Dictionary<String, Cell>();
            // Create an empty dependencies DG 
            dependencies = new DependencyGraph();
            // Set isValidRegex to isValid 
            isValidRegex = isValid;
            // Set Changed equal to false 
            Changed = false;
        }

        /// <summary>
        /// Constructor for Spreadsheet to create a new spreadsheetCells dictionary 
        /// and a dependencies dependency graph object. 
        /// This constructor creates a Spreadsheet that is a duplicate of the spreadsheet 
        /// saved in source. 
        /// If there's a problem reading source, throws an IOException 
        /// If the contents of source is not formatted properly, throws a SpreadsheetReadException. 
        /// </summary>
        public Spreadsheet(TextReader source)
        {
            try
            {
                // Create an empty spreadsheetCells dictionary 
                spreadsheetCells = new Dictionary<String, Cell>();
                // Create an empty dependencies DG 
                dependencies = new DependencyGraph();
                // Set Changed equal to false 
                Changed = false;

                // Read in the XML file 
                using (XmlReader reader = XmlReader.Create(source))
                {
                    // Create a temporary cell object 
                    Cell readCell = new Cell();
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                // If reader is on spreadsheet, read in and set the isvalid regex 
                                case "spreadsheet":
                                    reader.MoveToFirstAttribute();
                                    reader.ReadAttributeValue();
                                    String regex = reader.Value;
                                    Regex tempRegex = new Regex(regex);
                                    isValidRegex = tempRegex;
                                    break;

                                // If reader is on cell, initialize a new temporary cell 
                                case "cell":
                                    readCell = new Cell();
                                    break;

                                // If reader is on name, set the temporary cell's name to name 
                                case "name":
                                    reader.Read();
                                    String name = reader.Value;
                                    readCell.cellName = name;
                                    break;

                                // If reader is on contents, set the temporary cell's contents to contents 
                                // and add the temporary cell to the spreadsheet 
                                case "contents":
                                    reader.Read();
                                    String contents = reader.Value;
                                    readCell.cellContents = contents;
                                    SetContentsOfCell(readCell.cellName, contents);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (IOException)
            {
                throw new IOException();
            }
        }
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<String> returnValues = new HashSet<String>();
            // Return all key values in the spreadsheetCells dictionary 
            IEnumerable keys = spreadsheetCells.Keys;
            foreach (String tempKey in keys)
            {
                Cell temp;
                spreadsheetCells.TryGetValue(tempKey, out temp);
                if (!temp.cellContents.Equals(""))
                {
                    returnValues.Add(tempKey);
                }
            }
            return spreadsheetCells.Keys;
        }

        public override object GetCellContents(string name)
        {
            // Check if name is null, if so, throw InvalidNameException 
            if (name == null)
            {
                throw new InvalidNameException();
            }

            String normalName = name.ToUpper();
            // Create regex pattern for checking cell names 
            String cellPattern = @"^[a-zA-Z]+[1-9]\d*$";
            // Make sure name is a valid cell name 
            if (Regex.IsMatch(normalName, cellPattern))
            {
                // Create new Cell 
                Cell temp;
                // Try to get the contents of name. If we can, then return it
                if (spreadsheetCells.TryGetValue(normalName, out temp))
                {
                    return temp.cellContents;
                }
                // If name does not exist / has no contents, return empty string 
                else
                {
                    return "";
                }
            }
            // If name was not a valid cell name, throw InvalidNameException 
            else
            {
                throw new InvalidNameException();
            }

        }

        protected override ISet<string> SetCellContents(string name, double number)
        {
            // If the cell already exists, remove it 
            if (spreadsheetCells.ContainsKey(name))
            {
                spreadsheetCells.Remove(name);
            }

            // Create new cell
            Cell toBeAdded = new Cell();
            // Set cellName equal to name
            toBeAdded.cellName = name;
            // Set cellContents equal to number 
            toBeAdded.cellContents = number;
            // Set cellValue equal to number 
            toBeAdded.cellValue = number;
            // Set cellContentsType equal to double
            toBeAdded.cellContentsType = "double";

            // Add (name, cell) to our spreadsheetCells dictionary 
            spreadsheetCells.Add(name, toBeAdded);

            // Get dependecies for return portion of method call 
            HashSet<String> returnValues = new HashSet<String>();
            // Call GetCellsToRecalculate to get direct dependents 
            IEnumerable dependents = GetCellsToRecalculate(name);
            foreach (String temp in dependents)
            {
                // Add direct dependents to returnValues 
                returnValues.Add(temp);
            }

            // Call GetDependees on dependencies to get indirect dependents 
            IEnumerable dependees = dependencies.GetDependees(name);
            foreach (String temp in dependees)
            {
                // Add indirect dependents to returnValues
                returnValues.Add(temp);
            }

            // Recalculate
            Cell recalculation;
            foreach (String depend in dependents)
            {
                if (spreadsheetCells.TryGetValue(depend, out recalculation))
                {
                    // Set value of cell 
                    try
                    {
                        String equalPattern = @"^=";
                        String cellContents = recalculation.cellContents.ToString();
                        if (Regex.IsMatch(cellContents, equalPattern))
                        {
                            String parsedContents = cellContents.Substring(1);
                            Formula f = new Formula(parsedContents);
                            recalculation.cellValue = f.Evaluate((x) => (double)GetCellValue(x));
                            //SetContentsOfCell(recalculation.cellName, recalculation.cellContents.ToString());
                        }
                    }
                    catch (InvalidCastException)
                    {
                        recalculation.cellValue = new FormulaError();
                    }
                    catch (FormulaEvaluationException)
                    {
                        recalculation.cellValue = new FormulaError();
                    }
                }
            }

            // Change changed to true since we made a change to the spreadsheet 
            Changed = true;

            // Return 
            return returnValues;
        }

        protected override ISet<string> SetCellContents(string name, string text)
        {
            // Check to see if text is empty 
            if (text.Equals(""))
            {
                return new HashSet<String>();
            }

            // If the cell already exists, remove it 
            if (spreadsheetCells.ContainsKey(name))
            {
                spreadsheetCells.Remove(name);
            }

            // Create new cell object 
            Cell toBeAdded = new Cell();
            // Set cellName equal to name 
            toBeAdded.cellName = name;
            // Set cellContents equal to text 
            toBeAdded.cellContents = text;
            // Set cellValue equal to text 
            toBeAdded.cellValue = text;
            // Set cellContentsType equal to string 
            toBeAdded.cellContentsType = "string";

            // Add (name, cell) to spreadsheetCells dictionary 
            spreadsheetCells.Add(name, toBeAdded);

            // Get dependencies for return method call 
            HashSet<String> returnValues = new HashSet<String>();
            // Call GetCellsToRecalculate to get direct dependents 
            IEnumerable dependents = GetCellsToRecalculate(name);
            foreach (String temp in dependents)
            {
                // Store direct dependents in returnValues 
                returnValues.Add(temp);
            }

            // Call GetDependees on dependencies to get indirect dependents 
            IEnumerable dependees = dependencies.GetDependees(name);
            foreach (String temp in dependees)
            {
                // Store indirect dependents in returnValues 
                returnValues.Add(temp);
            }
            // Change changed to true since we made a change to the spreadsheet 
            Changed = true;
            // return 
            return returnValues;
        }

        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            // Parse the formula to get the variables 
            IEnumerable variables = formula.GetVariables();

            // Get features of cell prior to removing it in case there is a circular dependency 
            Cell beforeCircleCheck = new Cell();
            Object contents = new Object();
            if (spreadsheetCells.TryGetValue(name, out beforeCircleCheck))
            {
                contents = beforeCircleCheck.cellContents;
            }

            // If the cell already exists, remove it 
            if (spreadsheetCells.ContainsKey(name))
            {
                spreadsheetCells.Remove(name);
                foreach (String variable in variables)
                {
                    dependencies.RemoveDependency(name, variable);
                }
            }

            // Create new cell object 
            Cell toBeAdded = new Cell();
            // Set cellName equal to name
            toBeAdded.cellName = name;
            // Set cellContents eequal to formula 
            toBeAdded.cellContents = "=" + formula;
            // Set cellContentsType equal to formula 
            toBeAdded.cellContentsType = "formula";

            // Add (name, cell) to spreadsheetCells dictionary 
            spreadsheetCells.Add(name, toBeAdded);

            // Add to dependency Graph 
            foreach (String variable in variables)
            {
                dependencies.AddDependency(name, variable);
            }

            // Check for circular dependencies. If one is found, restore origional cell 
            try
            {
                GetCellsToRecalculate(name);
            }
            catch (CircularException e)
            {
                // Restore origional cell
                if (!(beforeCircleCheck == null))
                {
                    spreadsheetCells.Remove(name);
                    foreach (String var in variables)
                    {
                        dependencies.AddDependency(name, var);
                    }
                    toBeAdded.cellContents = beforeCircleCheck.cellContents;
                    spreadsheetCells.Add(name, toBeAdded);
                }
                throw e;
            }

            // Set value of cell 
            try
            {
                toBeAdded.cellValue = formula.Evaluate((x) => (double)GetCellValue(x));
            }
            catch (InvalidCastException)
            {
                toBeAdded.cellValue = new FormulaError();
            }
            catch (FormulaEvaluationException)
            {
                toBeAdded.cellValue = new FormulaError();
            }

            // Get dependencies for return method call 
            HashSet<String> returnValues = new HashSet<String>();
            // Call GetCellsToRecalculate for direct dependents 
            IEnumerable dependents = GetCellsToRecalculate(name);
            foreach (String temp in dependents)
            {
                // Add direct dependents to returnValues 
                returnValues.Add(temp);
            }

            // Call GetDependees on dependencies to get indirect dependents 
            IEnumerable dependees = dependencies.GetDependees(name);
            foreach (String temp in dependees)
            {
                // Add indriect dependents to returnValues 
                returnValues.Add(temp);
            }

            // Recalculate
            Cell recalculation;
            foreach (String depend in dependents)
            {
                if (spreadsheetCells.TryGetValue(depend, out recalculation))
                {
                    // Set value of cell 
                    try
                    {
                        recalculation.cellValue = formula.Evaluate((x) => (double)GetCellValue(x));
                    }
                    catch (InvalidCastException)
                    {
                        recalculation.cellValue = new FormulaError();
                    }
                    catch (FormulaEvaluationException)
                    {
                        recalculation.cellValue = new FormulaError();
                    }
                }
            }

            // Change changed to true since we made a change to the spreadsheet 
            Changed = true;

            // return 
            return returnValues;
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            // Check to see if name is null, if it is, throw ArgumentNullException 
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                String normalName = name.ToUpper();
                // Create regex pattern for checking cell names
                String cellPattern = @"^[a-zA-Z]+[1-9]\d*$";
                // Check if name is a valid cell name 
                if (Regex.IsMatch(normalName, cellPattern))
                {
                    // Create returnvalues for a return set 
                    HashSet<String> returnvalues = new HashSet<String>();
                    // Get direct dependents by calling GetDependents on dependencies 
                    IEnumerable dependents = dependencies.GetDependees(normalName);
                    // IEnumerable dependents = dependencies.GetDependents(name);
                    foreach (String temp in dependents)
                    {
                        // Add direct dependents to returnValues 
                        returnvalues.Add(temp);
                    }
                    // return 
                    return returnvalues;
                }

                // If name was not valid, throw InvalidNameException 
                else
                {
                    throw new InvalidNameException();
                }
            }
        }

        /// <summary>
        /// This class is used to create Cell objects 
        /// Cell objects have three properties:
        ///     cellContents which stores the cell's contents
        ///     cellValue which stores the cell's value 
        ///     cellName which stores the cell's name
        /// </summary>
        public class Cell
        {
            // Property for getting / setting cellContents 
            public Object cellContents { get; set; }
            // Property for getting / setting cellValue
            public Object cellValue { get; set; }
            // Property for getting / setting cellName 
            public String cellName { get; set; }
            // Property for getting / setting cellContentsType 
            public String cellContentsType { get; set; }
        }

        public override bool Changed { get; protected set; }

        public override void Save(System.IO.TextWriter dest)
        {
            // Get all cell names 
            IEnumerable cells = GetNamesOfAllNonemptyCells();

            try
            {
                using (XmlWriter writer = XmlWriter.Create(dest))
                {
                    // Write spreadsheet isValid Regex 
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("isvalid", isValidRegex.ToString());

                    // Write out cell elements 
                    foreach (String cellName in cells)
                    {
                        Cell cell;
                        spreadsheetCells.TryGetValue(cellName, out cell);

                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", cell.cellName);

                        // Determine cellContentsValue 
                        // If cell contents is a string 
                        if (cell.cellContentsType.Equals("string"))
                        {
                            writer.WriteElementString("contents", cell.cellContents.ToString());
                        }

                        // If cell contents is a double 
                        if (cell.cellContentsType.Equals("double"))
                        {
                            writer.WriteElementString("contents", cell.cellContents.ToString());
                        }

                        // If cell contents is a formula 
                        if (cell.cellContentsType.Equals("formula"))
                        {
                            Object cellContents = cell.cellContents;
                            writer.WriteElementString("contents", cellContents.ToString());
                        }

                        // Write the end of the cell element 
                        writer.WriteEndElement();
                    }

                    // Write the end of the document 
                    writer.WriteEndDocument();
                    // Change Changed to false since it was just saved 
                    Changed = false;
                }
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        public override object GetCellValue(string name)
        {
            // Check for cell name being null 
            if (name == null)
            {
                throw new InvalidNameException();
            }

            // Normalize name 
            String normalName = name.ToUpper();

            // Check if name is valid 
            if (NameIsValid(normalName))
            {
                Cell temp = new Cell();
                if (spreadsheetCells.TryGetValue(normalName, out temp))
                {
                    return temp.cellValue;
                }
                else
                {
                    return "";
                    //throw new InvalidNameException();
                }
            }
            else
            {
                throw new InvalidNameException();
            }


        }

        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            // Check to see if content is null
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            // Check to see if name is null 
            if (name == null)
            {
                throw new InvalidNameException();
            }

            // Normalize name 
            String normalName = name.ToUpper();

            // Check to see if name is valid 
            if (NameIsValid(normalName))
            {
                // If content parses as a double
                double parsedContent;
                if (double.TryParse(content, out parsedContent))
                {
                    return SetCellContents(normalName, parsedContent);
                }

                // If content begins with an = sign 
                String equalPattern = @"^=";
                if (Regex.IsMatch(content, equalPattern))
                {
                    String contentToBeParsed = content.Substring(1).ToUpper();

                    // Try to parse into formula 
                    Formula parsedFormula;
                    try
                    {
                        parsedFormula = new Formula(contentToBeParsed);
                    }
                    catch (FormulaFormatException e)
                    {
                        throw e;
                    }

                    // Try to set contents of name to be formula, throw CircularException if needed
                    try
                    {
                        return SetCellContents(normalName, parsedFormula);
                    }
                    catch (CircularException e)
                    {
                        throw e;
                    }
                }

                // Content should just be a string since it isn't a formula or double 
                else
                {
                    return SetCellContents(normalName, content);
                }
            }
            else
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// This formula checks to make sure the cell name is valid and also that the 
        /// regex is matched for the cell name. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool NameIsValid(String name)
        {
            // Check the validity of name 
            String cellPattern = @"^[A-Z]+[1-9]\d*$";
            if (Regex.IsMatch(name, cellPattern))
            {
                if (isValidRegex.IsMatch(name))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }     
        }
    }
}
