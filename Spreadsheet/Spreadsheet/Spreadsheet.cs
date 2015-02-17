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
           // throw new NotImplementedException();
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
            String cellPattern = @"[a-zA-Z]+[1-9]\d*";
            if (Regex.IsMatch(name, cellPattern))
            {
                Cell toBeAdded = new Cell();
                toBeAdded.cellName = name;
                toBeAdded.cellContents = number.ToString();
                toBeAdded.cellValue = number.ToString();

                spreadsheetCells.Add(name, toBeAdded);

                HashSet<String> returnValues = new HashSet<String>();
                IEnumerable dependents = GetCellsToRecalculate(name);
                foreach (String temp in dependents)
                {
                    returnValues.Add(temp);
                }

                return returnValues;
              //  returnvalues.Add(name);

                // Direct dependents 
                //IEnumerable dependents = dependencies.GetDependees(name);
                //foreach (String temp in dependents)
                //{
                //    returnvalues.Add(temp);
                //}

                //// Indirect dependents 
                //IEnumerable indirects = GetCellsToRecalculate(name);
                //foreach (String temp in indirects)
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

        public override ISet<string> SetCellContents(string name, string text)
        {
            // Check validity of text 
            if (text.Equals(""))
            {
                throw new ArgumentNullException();
            }
            // Check validity of name 
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

              //  returnvalues.Add(name);
            //    // Direct dependents 
            //    IEnumerable dependents = dependencies.GetDependees(name);
            //    foreach (String temp in dependents)
            //    {
            //        returnvalues.Add(temp);
            //    }

            //    return returnvalues;
            //}
            else
            {
                throw new InvalidNameException();
            }
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula.Equals(""))
            {
                throw new ArgumentNullException();
            }
            // Check validity of name 
            String cellPattern = @"[a-zA-Z]+[1-9]\d*";
            if (Regex.IsMatch(name, cellPattern))
            {
                // if check for circular dependency 
                try
                {

                }
                catch
                {

                }
                // else 
                Cell toBeAdded = new Cell();
                toBeAdded.cellName = name;
                toBeAdded.cellContents = formula.ToString();
                //toBeAdded.cellValue = text;

                spreadsheetCells.Add(name, toBeAdded);

                HashSet<String> returnvalues = new HashSet<String>();
                returnvalues.Add(name);
                // Direct dependents 
                IEnumerable dependents = dependencies.GetDependees(name);
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
                    IEnumerable dependents = dependencies.GetDependees(name);
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
          //  String cellName;
            //String cellContents;
           // String cellValue;
           // private Cell(String name, String contents)
           // {
               // cellName = name;
               // cellContents = contents;
                //cellValue = "";
           // }

            public String cellContents {get; set;}
            public String cellValue {get; set;}
            public String cellName {get; set;}


            //public String GetCellContents()
            //{
             //   return cellContents;
            //}
        }
    }

}
