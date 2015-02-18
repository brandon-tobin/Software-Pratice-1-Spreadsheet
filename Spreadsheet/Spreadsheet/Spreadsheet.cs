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
        Dictionary<String, Cell> spreadsheetCells;
        DependencyGraph dependencies;
        public Spreadsheet()
        {
            spreadsheetCells = new Dictionary<String, Cell>();
            dependencies = new DependencyGraph();
        }
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return spreadsheetCells.Keys;
        }

        public override object GetCellContents(string name)
        {
            if (name == null)
            {
                throw new InvalidNameException();
            }
            String cellPattern = @"[a-zA-Z]+[1-9]\d*";
            if (Regex.IsMatch(name, cellPattern))
            {
                Cell temp;
                if (spreadsheetCells.TryGetValue(name, out temp))
                {
                    return temp.cellContents;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                throw new InvalidNameException();
            }

        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            // Check validity of name 
            if (name == null)
            {
                throw new InvalidNameException();
            }

            String cellPattern = @"[a-zA-Z]+[1-9]\d*";
            if (Regex.IsMatch(name, cellPattern))
            {
                Cell toBeAdded = new Cell();
                toBeAdded.cellName = name;
                toBeAdded.cellContents = number;
                toBeAdded.cellValue = number;

                spreadsheetCells.Add(name, toBeAdded);

                HashSet<String> returnValues = new HashSet<String>();
                IEnumerable dependents = GetCellsToRecalculate(name);
                foreach (String temp in dependents)
                {
                    returnValues.Add(temp);
                }

                return returnValues;
            }
            else
            {
                throw new InvalidNameException();
            }

        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            // Check validity of text 
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            // Check validity of name 
            if (name == null)
            {
                throw new InvalidNameException();
            }
            String cellPattern = @"[a-zA-Z]+[1-9]\d*";
            if (Regex.IsMatch(name, cellPattern))
            {
                Cell toBeAdded = new Cell();
                toBeAdded.cellName = name;
                toBeAdded.cellContents = text;
                toBeAdded.cellValue = text;

                spreadsheetCells.Add(name, toBeAdded);

                HashSet<String> returnValues = new HashSet<String>();
                IEnumerable dependents = GetCellsToRecalculate(name);
                foreach (String temp in dependents)
                {
                    returnValues.Add(temp);
                }

                return returnValues;
            }
            else
            {
                throw new InvalidNameException();
            }
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException();
            }
            // Check validity of name 
            if (name == null)
            {
                throw new InvalidNameException();
            }
            String cellPattern = @"[a-zA-Z]+[1-9]\d*";
            if (Regex.IsMatch(name, cellPattern))
            {
                // if check for circular dependency 
                IEnumerable variables = formula.GetVariables();
                try
                {
                    foreach (String variable in variables)
                    {
                        GetCellsToRecalculate(variable);
                    }
                }
                catch
                {
                    throw new CircularException();
                }

                Cell toBeAdded = new Cell();
                toBeAdded.cellName = name;
                toBeAdded.cellContents = formula;
                spreadsheetCells.Add(name, toBeAdded);

                //dependencies.AddDependency(name, )

                // Add to dependency Graph 
                foreach (String variable in variables)
                {

                    dependencies.AddDependency(name, variable);
                }

                HashSet<String> returnValues = new HashSet<String>();
                returnValues.Add(name);

               // HashSet<String> returnValues = new HashSet<String>();
                IEnumerable dependents = GetCellsToRecalculate(name);
               // IEnumerable dependents = dependencies.GetDependents(name);
                foreach (String temp in dependents)
                {
                    returnValues.Add(temp);
                }

                return returnValues;
                // Direct dependents 
                //IEnumerable dependents = dependencies.GetDependees(name);
                //foreach (String temp in dependents)
                //{
                //    returnvalues.Add(temp);
                //}

                //return returnvalues;
            }
            else
            {
                throw new InvalidNameException();
            }
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name.Equals(""))
            {
                throw new ArgumentNullException();
            }
            else
            {
                // Check validity of name 
                String cellPattern = @"[a-zA-Z]+[1-9]\d*";
                if (Regex.IsMatch(name, cellPattern))
                {
                    HashSet<String> returnvalues = new HashSet<String>();
                    // Direct dependents 
                    IEnumerable dependents = dependencies.GetDependents(name);
                    foreach (String temp in dependents)
                    {
                        returnvalues.Add(temp);
                    }

                    return returnvalues;
                }
                else
                {
                    throw new InvalidNameException();
                }
            }
        }


        public class Cell
        {
            public Object cellContents {get; set;}
            public Object cellValue {get; set;}
            public String cellName {get; set;}
        }
    }

}
