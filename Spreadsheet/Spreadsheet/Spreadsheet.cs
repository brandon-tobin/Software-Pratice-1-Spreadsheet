using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.Collections;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        // Instance variables 
        // spreadsheetCells will represent the link between a Cell object and its name
        Dictionary<String, Cell> spreadsheetCells;
        // dependencies will act as a dependnecy graph for keeping track of dependees and dependents
        DependencyGraph dependencies;
       
        /// <summary>
        /// Constructor for Spreadsheet to create a new spreadsheetCells dictionary 
        /// and a dependencies dependency graph object. 
        /// When Spreadsheet constructor is called, it will create a new spreadsheet object 
        /// </summary>
        public Spreadsheet()
        {
            spreadsheetCells = new Dictionary<String, Cell>();
            dependencies = new DependencyGraph();
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
                if (temp.cellContents != "")
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
            // Create regex pattern for checking cell names 
            String cellPattern = @"^[a-zA-Z]+[1-9]\d*$";
            // Make sure name is a valid cell name 
            if (Regex.IsMatch(name, cellPattern))
            {
                // Create new Cell 
                Cell temp;
                // Try to get the contents of name. If we can, then return it
                if (spreadsheetCells.TryGetValue(name, out temp))
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

        public override ISet<string> SetCellContents(string name, double number)
        {
            // Check if name is null, if it is, throw new InvalidNameException 
            if (name == null)
            {
                throw new InvalidNameException();
            }

            //Create regex pattern for checking cell names 
            String cellPattern = @"^[a-zA-Z]+[1-9]\d*$";
            // Make sure name is a valid cell name 
            if (Regex.IsMatch(name, cellPattern))
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

                // Return 
                return returnValues;
            }

            // If name was not valid, throw InvalidNameException 
            else
            {
                throw new InvalidNameException();
            }

        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            // Check to see if text is null, if it is, throw ArguementNullException  
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            // Check to see if name is null, if it is, throw InvalidNameException 
            if (name == null)
            {
                throw new InvalidNameException();
            }
            // Check to see if text is empty 
            if (text.Equals(""))
            {
                return new HashSet<String>();
            }
            // Create regex pattern for checking cell names 
            String cellPattern = @"^[a-zA-Z]+[1-9]\d*$";
            // Make sure name is a valid cell name 
            if (Regex.IsMatch(name, cellPattern))
            {
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
                // return 
                return returnValues;
            }

            // If name was not valid, throw InvalidNameExeception 
            else
            {
                throw new InvalidNameException();
            }
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            // Check to see if formula is null, if so, throw ArguementNullException 
            if (formula == null)
            {
                throw new ArgumentNullException();
            }
            // Check to see if name is null, if so, throw InvalidNameException  
            if (name == null)
            {
                throw new InvalidNameException();
            }
            // Set up regex pattern for checking cell names 
            String cellPattern = @"^[a-zA-Z]+[1-9]\d*$";

            // Parse the formula to get the variables 
            IEnumerable variables = formula.GetVariables();

            // Check to make sure name is a valid cell name 
            if (Regex.IsMatch(name, cellPattern))
            {
                // If the cell already exists, remove it 
                if (spreadsheetCells.ContainsKey(name))
                {
                    spreadsheetCells.Remove(name);
                    foreach (String variable in variables) 
                    {
                        dependencies.RemoveDependency(name, variable);
                    }
                }
            
                // Check for circular dependencies 
                try
                {
                    foreach (String variable in variables)
                    {
                        GetCellsToRecalculate(variable);
                    }

                    GetCellsToRecalculate(name);
                }
                catch (CircularException e)
                {
                    throw new CircularException();
                }

                // Create new cell object 
                Cell toBeAdded = new Cell();
                // Set cellName equal to name
                toBeAdded.cellName = name;
                // Set cellContents eequal to formula 
                toBeAdded.cellContents = formula;
                
                // Add (name, cell) to spreadsheetCells dictionary 
                spreadsheetCells.Add(name, toBeAdded);

                // Add to dependency Graph 
                foreach (String variable in variables)
                {
                    dependencies.AddDependency(name, variable);
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

                // return 
                return returnValues;
            }
            
            // If name was not valid, throw InvlaidNameException 
            else
            {
                throw new InvalidNameException();
            }
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
                // Create regex pattern for checking cell names
                String cellPattern = @"^[a-zA-Z]+[1-9]\d*$";
                // Check if name is a valid cell name 
                if (Regex.IsMatch(name, cellPattern))
                {
                    // Create returnvalues for a return set 
                    HashSet<String> returnvalues = new HashSet<String>();
                    // Get direct dependents by calling GetDependents on dependencies 
                    IEnumerable dependents = dependencies.GetDependents(name);
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
            public Object cellContents {get; set;}
            // Property for getting / setting cellValue
            public Object cellValue {get; set;}
            // Property for getting / setting cellName 
            public String cellName {get; set;}
        }
    }
}
