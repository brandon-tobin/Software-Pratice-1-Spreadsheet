// Skeleton written by Joe Zachary for CS 3500, January 2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        // Instance variables 
        // eValuateFormula will hold the formula itself for being evaluated after 
        // it is built by the constructor 
        String evaluateFormula;
        // normalizedVariables will keep track of the variables that have been 
        // normalized in the constructor 
        HashSet<String> normalizedVaraibles = new HashSet<String>();

        /// <summary>
        /// This constructor takes a String formula, Normalizer N, and Validator V. 
        /// It will construct a formula and apply the normalizer and validator that was sent 
        /// in by the caller. 
        /// </summary>
        public Formula(String formula, Normalizer N, Validator V)
        {
            // Regex Patterns 
            String varPattern = @"^[a-zA-Z_][a-zA-Z0-9]*$";
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String numbers = @"[0-9]";
            String opPattern = @"[\+\-*/]";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";

            // Pattern finds ( or operators 
            String lpOpersPattern = @"^[\+\-*\/\(]$";
            // Pattern finds (, a variable, a number 
            String lpVarsNums = String.Format("({0}) | ({1}) | ({2}) | ({3})", lpPattern, varPattern, numbers, doublePattern);
            // Pattern finds a ), a variable, a number  
            String cpVarsNums = String.Format("({0}) | ({1}) | ({2}) | ({3})", rpPattern, varPattern, numbers, doublePattern);
            // Pattern finds a ) or an operator 
            String cpOpers = String.Format("({0}) | ({1})", rpPattern, opPattern);

            // Split formula up into tokens 
            IEnumerable<String> tokens = GetTokens(formula);

            // Variables for keeping track of certain aspects of the formula  
            int totalTokens = 0;
            int rightParen = 0;
            int leftParen = 0;
            String firstToken = "";
            String lastToken = "";
            String previousTemp = "";
            String currentTemp = "";

            // Loop through tokens to make sure there are no invalid tokens 
            foreach (String temp in tokens)
            {
                // Keep track of previous and current tokens 
                previousTemp = currentTemp;
                currentTemp = temp;

                // Perform normalizer 
                if (Regex.IsMatch(temp, varPattern, RegexOptions.IgnorePatternWhitespace))
                {
                    currentTemp = N(temp);

                    if (!Regex.IsMatch(currentTemp, varPattern, RegexOptions.IgnorePatternWhitespace))
                    {
                        throw new FormulaFormatException("Invalid variable after normalize");
                    }

                    // Perform validator 
                    if (!V(currentTemp))
                    {
                        throw new FormulaFormatException("Token validator failed");
                    }
                    normalizedVaraibles.Add(currentTemp);
                }

                // Grab first token 
                if (totalTokens == 0)
                    firstToken = temp;

                // Grab last token 
                lastToken = temp;

                // Increment token counter 
                totalTokens++;

                // Count parens in the formula 
                if (temp.Equals("("))
                    leftParen++;
                if (temp.Equals(")"))
                    rightParen++;

                // ( should not be greater than ) 
                if (rightParen > leftParen)
                    throw new FormulaFormatException("Closing paren greater than opening paren -- not enough opening parens");

                // Token following ( or operator must be a number, a variable, or an opening paren 
                if (Regex.IsMatch(previousTemp, lpOpersPattern, RegexOptions.IgnorePatternWhitespace))
                    if (!Regex.IsMatch(currentTemp, lpVarsNums, RegexOptions.IgnorePatternWhitespace))
                        throw new FormulaFormatException("Token following ( or operator is invalid");

                // Token following a number, a variable, or ) must be an operator or ) 
                if (Regex.IsMatch(previousTemp, cpVarsNums, RegexOptions.IgnorePatternWhitespace))
                    if (!Regex.IsMatch(currentTemp, cpOpers, RegexOptions.IgnorePatternWhitespace))
                        throw new FormulaFormatException("Token following ) or operator is invalid");

                evaluateFormula += currentTemp;
            }

            // There must be at least one token 
            if (totalTokens == 0)
                throw new FormulaFormatException("There are no tokens");

            // Total number of ( should equal total number of ) 
            if ((leftParen != rightParen))
                throw new FormulaFormatException("Number of left parens does not equal number of right parens");

            // First token must be a number, a variable, or (
            String openingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3})", lpPattern, varPattern, numbers, doublePattern);
            if (!Regex.IsMatch(firstToken, openingPattern, RegexOptions.IgnorePatternWhitespace))
                throw new FormulaFormatException("Formula does not start with a valid char");

            // Last token must be a number, a vairable, or ) 
            String closingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3})", rpPattern, varPattern, numbers, doublePattern);
            if (!Regex.IsMatch(lastToken, closingPattern, RegexOptions.IgnorePatternWhitespace))
                throw new FormulaFormatException("Formula does not end with a valid char");
        }

        /// <summary>
        /// The Normalizer delegate will take a method passed in by the caller 
        /// to be used in the constructor. This delegate is added to allow the user 
        /// to place more restrictions on the variables in the formula.
        /// </summary>
        public delegate string Normalizer(string n);

        /// <summary>
        /// The Validator delegate will take a method passed in by the caller 
        /// to be used in the constructor to check if the variable is still 
        /// valid after being normalized. The Validator also allows the user to place 
        /// more restrictions on the variables in the formula. 
        /// </summary>
        public delegate bool Validator(string s);

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using standard C# syntax for double/int literals), 
        /// variable symbols (one or more letters followed by one or more digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// An example of a valid parameter to this constructor is "2.5e9 + x5 / 17".
        /// Examples of invalid parameters are "x", "-5.3", and "2 5 + 3";
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
            :this(formula, x => x, d => true)
        {
        }

        /// <summary>
        /// Evaluates this Formula, using lookup to determine the values of variables.  
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, throw a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            // Create value stack 
            Stack<string> values = new Stack<string>();
            // Create operator stack 
            Stack<string> operators = new Stack<string>();

            // Regex Patterns 
            String varPattern = @"[a-zA-Z_][a-zA-Z0-9]*";
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPlusMinus = @"[\+\-]";
            String opMultDivide = @"[*/]";

            // Split formula up into tokens 
            IEnumerable<String> tokens = GetTokens(evaluateFormula);

            // Loop through each token
            foreach (String temp in tokens)
            {
                // If token temp is a double
                double outVal;
                if (Double.TryParse(temp, out outVal))
                {
                    // If the first entry on the operator stack is a *
                    if (operators.Count != 0 && operators.Peek().Equals("*"))
                    {
                        // Pop value stack 
                        double value = Double.Parse(values.Pop());
                        // Parse temp into a double 
                        double tempValue = Double.Parse(temp);
                        // Pop operator stack 
                        operators.Pop();
                        // Apply popped operator to temp and the popped value
                        tempValue = tempValue * value;
                        // Push the result back onto the value stack 
                        values.Push(tempValue.ToString());
                    }
                    // If the first entry on the operator stack is a /
                    else if (operators.Count != 0 && operators.Peek().Equals("/"))
                    {
                        // Pop value stack
                        double value = Double.Parse(values.Pop());
                        // Parse temp into a double 
                        double tempValue = Double.Parse(temp);
                        // Pop operator stack 
                        operators.Pop();
                        // Check to see if tempValue = 0
                        if (tempValue.Equals(0))
                        {
                            // If tempValue = 0, throw FormulaEvaluationException -- Division by 0
                            throw new FormulaEvaluationException("Cannot divide by 0");
                        }
                        else
                        {
                            // If tempValue != 0, apply popped operator to temp and the popped value 
                            tempValue = value / tempValue;
                            // Push the result back onto the value stack 
                            values.Push(tempValue.ToString());
                        }
                    }
                    // If the first entry on the operator stack is not * or /, just push the temp value onto the values stack 
                    else
                    {
                        values.Push(temp);
                    }
                }

                // If token temp is a variable 
                else if (Regex.IsMatch(temp, varPattern, RegexOptions.IgnorePatternWhitespace))
                {
                    // If the first entry on the operator stack is a *
                    if (operators.Count != 0 && operators.Peek().Equals("*"))
                    {
                        // Pop the value stack 
                        double value = Double.Parse(values.Pop());
                        double tempValue;
                       // Try to look up the value of the token temp 
                        try
                        {
                            tempValue = lookup(temp);
                        }
                        // If the token temp doesn't have a value, throw FormulaEvaluationException -- Empty variables 
                        catch (ArgumentException)
                        {
                            throw new FormulaEvaluationException("Empty variables");
                        }
                        // Pop the operator stack 
                        operators.Pop();
                        // Apply the popped operator to temp and the popped value 
                        tempValue = tempValue * value;
                        // Push result back onto the values stack 
                        values.Push(tempValue.ToString());
                    }
                    // If the first entry on the operator stack is a /
                    else if (operators.Count != 0 && operators.Peek().Equals("/"))
                    {
                        // Pop the value stack 
                        double value = Double.Parse(values.Pop());
                        double tempVal;
                        // Try to look up the value of the token temp 
                        try
                        {
                            tempVal = lookup(temp);
                        }
                        // If the token temp doesn't have a value, throw FormulaEvaluationException -- Empty variables 
                        catch (ArgumentException)
                        {
                            throw new FormulaEvaluationException("Empty variables");
                        }
                        // Pop the operator stack 
                        operators.Pop();
                        // Check to see if tempVal = 0
                        if (tempVal.Equals(0))
                        {
                            // If tempVal = 0, throw FormulaEvaluationException -- Division by 0
                            throw new FormulaEvaluationException("Cannot divide by 0");
                        }
                        // If tempVal != 0
                        else
                        {
                            // Apply the popped operator to temp and the popped value 
                            tempVal = value / tempVal;
                            // Push the result back onto the values stack 
                            values.Push(tempVal.ToString());
                        }
                    }
                    // If the first entry on the operator stack is not * or /, just push the temp value onto the values stack 
                    else
                    {
                        // Look up the value for temp and push it onto the values stack 
                        try
                        {
                            values.Push(lookup(temp).ToString());
                        }
                        catch (ArgumentException)
                        {
                            // If temp doesn't have a value throw FormulaEvaluationException -- Empty variables 
                            throw new FormulaEvaluationException("Empty variables");
                        }
                    }
                }

                // If token is a + or - 
                else if (Regex.IsMatch(temp, opPlusMinus, RegexOptions.IgnorePatternWhitespace))
                {
                    // If the top value of the operator stack is +
                    if (operators.Count != 0 && operators.Peek().Equals("+"))
                    {
                        // Pop the values stack 
                        double val1 = Double.Parse(values.Pop());
                        // Pop the values stack 
                        double val2 = Double.Parse(values.Pop());
                        // Pop the operators stack 
                        operators.Pop();
                        // Apply the operator to the two popped values 
                        double tempValue = val1 + val2;
                        // Push result back onto values stack 
                        values.Push(tempValue.ToString());
                    }
                    // If the top value of the operator stack is -
                    else if (operators.Count != 0 && operators.Peek().Equals("-"))
                    {
                        // Pop the values stack 
                        double val1 = Double.Parse(values.Pop());
                        // Pop the values stack 
                        double val2 = Double.Parse(values.Pop());
                        // Pop the operators stack 
                        operators.Pop();
                        // Apply the operator to the two popped values
                        double tempValue = val2 - val1;
                        // Push result back onto values stack 
                        values.Push(tempValue.ToString());
                        // Push temp onto operator stack 
                        operators.Push(temp);
                    }

                    // Push temp onto operator stack 
                    operators.Push(temp);
                }

                // If token is a * or / 
                else if (Regex.IsMatch(temp, opMultDivide, RegexOptions.IgnorePatternWhitespace))
                {
                    // Push temp onto operator stack 
                    operators.Push(temp);
                }

                // If token is a (
                else if (Regex.IsMatch(temp, lpPattern, RegexOptions.IgnorePatternWhitespace))
                {
                    // Push temp onto operator stack 
                    operators.Push(temp);
                }

                // If toekn is a ) 
                else if (Regex.IsMatch(temp, rpPattern, RegexOptions.IgnorePatternWhitespace))
                {
                    // If top value of operator stack is + 
                    if (operators.Count != 0 && operators.Peek().Equals("+"))
                    {
                        // Pop the values stack 
                        double val1 = Double.Parse(values.Pop());
                        // Pop the values stack 
                        double val2 = Double.Parse(values.Pop());
                        // Pop the operators stack 
                        operators.Pop();
                        // Apply the operator to the two values 
                        double tempValue = val1 + val2;
                        // Push result back onto values stack
                        values.Push(tempValue.ToString());
                    }
                    // If top value of operator stack is - 
                    else if (operators.Count != 0 && operators.Peek().Equals("-"))
                    {
                        // Pop the values stack 
                        double val1 = Double.Parse(values.Pop());
                        // Pop the values stack 
                        double val2 = Double.Parse(values.Pop());
                        // Pop the operators stack 
                        operators.Pop();
                        // Apply the operator to the two values 
                        double tempValue = val2 - val1;
                        // Push result back onto values stack 
                        values.Push(tempValue.ToString());
                    }

                    // Pop operator stack 
                    operators.Pop();

                    // If top value of operator stack is * 
                    if (operators.Count != 0 && operators.Peek().Equals("*"))
                    {
                        // Pop the values stack 
                        double val1 = Double.Parse(values.Pop());
                        // Pop the values stack 
                        double val2 = Double.Parse(values.Pop());
                        // Pop the operators stack 
                        operators.Pop();
                        // Apply the operator to the two values 
                        double tempVal = val1 * val2;
                        // Push result back onto values stack 
                        values.Push(tempVal.ToString());
                    }
                    // If top value of operator stack is / 
                    if (operators.Count != 0 && operators.Peek().Equals("/"))
                    {
                        // Pop values stack 
                        double val1 = Double.Parse(values.Pop());
                        // Pop values stack 
                        double val2 = Double.Parse(values.Pop());
                        // Pop operators stack 
                        operators.Pop();

                        // Check to see if val2 = 0
                        if (val1.Equals(0))
                        {
                            // If val2 = 0, throw FormulaEvaluationException -- Division by 0
                            throw new FormulaEvaluationException("Cannot divide by 0");
                        }
                        else
                        {
                            // If val1 != 0, apply popped operator to temp and the popped value 
                            double tempVal = val2 / val1;
                            // Push the result back onto the value stack 
                            values.Push(tempVal.ToString());
                        }
                    }
                }
            }

            // If the operators stack is empty 
            if (operators.Count == 0)
            {
                // Pop and return the last value on the values stack 
                return Double.Parse(values.Pop());
            }
            else
            {
                // If top value of operator stack is + 
                if (operators.Peek().Equals("+"))
                {
                    // Pop values stack 
                    double val1 = Double.Parse(values.Pop());
                    // Pop values stack 
                    double val2 = Double.Parse(values.Pop());
                    // Apply operator to both values and return value 
                    return val1 + val2;
                }
                else
                {
                    // Pop values stack 
                    double val1 = Double.Parse(values.Pop());
                    // Pop values stack 
                    double val2 = Double.Parse(values.Pop());
                    // Apply operator to both values and return value 
                    return val2 - val1;
                }
            }
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of one or more
        /// letters followed by one or more digits, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_][a-zA-Z0-9]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// This method returns an IEnumerable of variables that are present in the formula. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> GetVariables()
        {
            return normalizedVaraibles;
        }

        /// <summary>
        /// This method returns the String version of the formula, therefore acting as the converter 
        /// between the formula object and the string representation. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return evaluateFormula;
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// A function is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an IllegalArgumentException (meaning that the string is unmapped.
    /// Exactly how a Lookup function decides which strings map to doubles and which
    /// don't is up to the implementation of that function.
    /// </summary>
    public delegate double Lookup(string s);
}