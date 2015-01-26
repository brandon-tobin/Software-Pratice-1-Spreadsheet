// Skeleton written by Joe Zachary for CS 3500, January 2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
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
        {
            // Regex Patterns 
            String varPattern = @"[a-zA-Z]+\d+";
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String numbers = @"[0-9]";
            String opPattern = @"[\+\-*/]";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";

            // Pattern finds ( or operators 
             String lpOpersPattern = String.Format("({0}) | ({1})", lpPattern, opPattern);
            // Pattern finds opening paren, a variable, a number 
             String lpVarsNums = String.Format("({0}) | ({1}) | ({2}) | ({3})", lpPattern, varPattern, numbers, doublePattern);
            // Pattern finds a closing paren, a variable, a number  
             String cpVarsNums = String.Format("({0}) | ({1}) | ({2}) | ({3})", rpPattern, varPattern, numbers, doublePattern);
            // Pattern finds a closing paren or an operator 
             String cpOpers = String.Format("({0}) | ({1})", rpPattern, opPattern);

            // Split formula up into tokens 
            IEnumerable<String> tokens = GetTokens(formula);

            // Loop through tokens to make sure there are no invalid tokens 
            int totalTokens = 0;
            int rightParen = 0;
            int leftParen = 0;
            String firstToken = "";
            String lastToken = "";
            String previousTemp = "";
            String currentTemp = "";
            foreach (String temp in tokens)
            {
                previousTemp = currentTemp;
                currentTemp = temp;

                // Grab first token 
                if (totalTokens == 0)
                    firstToken = temp;

                // Grab last token 
                lastToken = temp;
                // Token Variables 
                totalTokens++;


                // Count parens in the formula 
                if (temp.Equals("("))
                    leftParen++;
                if (temp.Equals(")"))
                    rightParen++;

              

                // Rightparen should not be greater than leftparen 
                if (rightParen > leftParen)
                    throw new FormulaFormatException("Closing paren greater than opening paren");

                // Token following ( or operator must be a number, a variable, or an opening paren 
                if (Regex.IsMatch(previousTemp, lpOpersPattern, RegexOptions.IgnorePatternWhitespace))
                    if (!Regex.IsMatch(currentTemp, lpVarsNums, RegexOptions.IgnorePatternWhitespace))
                          throw new FormulaFormatException("Token following ( or operator is invalid");

                // Token following a number, a variable, or ) must be an operator or ) 
                if (Regex.IsMatch(previousTemp, cpVarsNums, RegexOptions.IgnorePatternWhitespace))
                    if (!Regex.IsMatch(currentTemp, cpOpers, RegexOptions.IgnorePatternWhitespace))
                        throw new FormulaFormatException("Token following ) or operator is invalid");
            }

            // There must be at least one token 
            if (totalTokens == 0)
                throw new FormulaFormatException("There are no tokens");

            // Total number of leftparens should equal total number of rightparens 
            if ((leftParen != rightParen))
                throw new FormulaFormatException("Leftparen does not equal rightparen"); 

            // First token must be a number, a variable, or an opening paren
            String openingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3})", lpPattern, varPattern, numbers, doublePattern);
            if (!Regex.IsMatch(firstToken, openingPattern, RegexOptions.IgnorePatternWhitespace))
                throw new FormulaFormatException("Formula does not start with a valid char");

            // Last token must be a number, a vairable, or closing paren 
            String closingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3})", rpPattern, varPattern, numbers, doublePattern);
            if (!Regex.IsMatch(lastToken, closingPattern, RegexOptions.IgnorePatternWhitespace))
                throw new FormulaFormatException("Formula does not end with a valid char");


        }

        /// <summary>
        /// A Lookup function is one that maps some strings to double values.  Given a string,
        /// such a function can either return a double (meaning that the string maps to the
        /// double) or throw an IllegalArgumentException (meaning that the string is unmapped.
        /// Exactly how a Lookup function decides which strings map to doubles and which
        /// don't is up to the implementation of that function.
        /// </summary>
        public delegate double Lookup(string s);

        /// <summary>
        /// Evaluates this Formula, using lookup to determine the values of variables.  
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, throw a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            return 0;
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
            String varPattern = @"[a-zA-Z]+\d+";
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
}
