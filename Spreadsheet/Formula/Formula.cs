// Skeleton written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016

// Alex Kofford
// u0358110
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
        private String thisFormula;
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            if (GetTokens(formula).Count() == 0)
            {
                throw new FormulaFormatException("No tokens.");
            }

            int lpCount = 0;
            int rpCount = 0;
            String previousValue = "";

            foreach (String t in GetTokens(formula))
            {
                if (!IsValid(t))
                {
                    throw new FormulaFormatException("Invalid token. : " + t);
                }
                if (t == "(")
                {
                    lpCount++;
                }
                else if (t == ")")
                {
                    rpCount++;
                }
                if (rpCount > lpCount)
                {
                    throw new FormulaFormatException("Too many closing parentheses.");
                }
                CompareToPrev(previousValue, t);
                previousValue = t;
            }

            if (lpCount != rpCount)
            {
                throw new FormulaFormatException("Total number of opening parentheses does not match total number of closing parentheses.");
            }

            if (GetTokens(formula).Last() == "(" || GetTokens(formula).Last() == "+" || GetTokens(formula).Last() == "-" || GetTokens(formula).Last() == "*" || GetTokens(formula).Last() == "/")
            {
                throw new FormulaFormatException("Last token must be a number, variable, or closing parentheses.");
            }
            if (GetTokens(formula).First() == ")" || GetTokens(formula).First() == "+" || GetTokens(formula).First() == "-" || GetTokens(formula).First() == "*" || GetTokens(formula).First() == "/")
            {
                throw new FormulaFormatException("First token must be a number, variable, or opening parentheses.");
            }
            thisFormula = formula;
        }

        /// <summary>
        /// Compares the previous token to the current token to make sure it can follow the previous.
        /// Throws exceptions.
        /// Examples that throw: 2++2, 2 2
        /// </summary>
        public void CompareToPrev(String prev, String current)
        {
            //Check if the previous token was an operator or an opening parentheses, and if it is followed by an operator or a closing parentheses throw exception.
            if ((prev == "+" || prev == "-" || prev == "*" || prev == "/" || prev == "(") && (current == "+" || current == "-" || current == "*" || current == "/" || current == ")"))
            {
                throw new FormulaFormatException("You cannot have " + prev + " followed by " + current + ".");
            }
            //Check if the previous token was not an operator or opening parentheses, and if it is follwed by something other than an 
            if ((prev != "+" && prev != "-" && prev != "*" && prev != "/" && prev != "(") && (current != "+" && current != "-" && current != "*" && current != "/" && current != ")") && prev != "")
            {
                throw new FormulaFormatException("You cannot have " + prev + " followed by " + current + ".");
            }
        }

        /// <summary>
        /// Checks each token that is passed to it to make sure the token is valid.
        /// Returns true if the token is valid
        /// Valid tokens are (,),+,-,*,/, numbers, and variables.
        /// </summary>
        public bool IsValid(String s)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern);
            int x;
            double y;
            if (Regex.IsMatch(s, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (Regex.IsMatch(s, doublePattern, RegexOptions.IgnorePatternWhitespace)) // Make sure any doubles are positive
                {
                    if (double.TryParse(s, out y))
                    {
                        if (y < 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else if (int.TryParse(s, out x))// Make sure any integers are positive
            {
                if (x >= 0)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            Stack<String> operators = new Stack<String>();
            Stack<double> values = new Stack<double>();
            double d;
            foreach (String t in GetTokens(thisFormula))
            {
                //Doubles
                if (double.TryParse(t, out d))
                {
                    if (values.Count != 0)
                    {
                        if (operators.Peek() == "*" || operators.Peek() == "/")
                        {
                            values.Push(Result(operators.Pop(), values.Pop(), d));
                        }
                        else
                        {
                            values.Push(d);
                        }
                    }
                    else
                    {
                        values.Push(d);
                    }
                }

                //Operators
                else if (t == "+" || t == "-")
                {
                    if (operators.Count != 0)
                    {
                        if (operators.Peek() == "+" || operators.Peek() == "-")
                        {
                            values.Push(Result(operators.Pop(), values.Pop(), values.Pop()));
                        }
                    }
                    operators.Push(t);
                }
                else if (t == "*" || t == "/")
                {
                    operators.Push(t);
                }
                else if (t == "(")
                {
                    operators.Push(t);
                }
                else if (t == ")")
                {
                    if (operators.Peek() == "+" || operators.Peek() == "-")
                    {
                        values.Push(Result(operators.Pop(), values.Pop(), values.Pop()));
                    }
                    operators.Pop();
                    if (operators.Count != 0)
                    {
                        if (operators.Peek() == "*" || operators.Peek() == "/")
                        {
                            values.Push(Result(operators.Pop(), values.Pop(), values.Pop()));
                        }
                    }
                }

                //If it hasn't been anything else it must be a variable
                else
                {
                    try
                    {
                        d = lookup(t);
                    }catch(UndefinedVariableException)
                    {
                        throw new FormulaEvaluationException("No variables have values.");
                    }
                    if (values.Count != 0)
                    {
                        if (operators.Peek() == "*" || operators.Peek() == "/")
                        {
                            values.Push(Result(operators.Pop(), values.Pop(), d));
                        }
                        else
                        {
                            values.Push(d);
                        }
                    }
                    else
                    {
                        values.Push(d);
                    }
                }
            }

            //Out of tokens
            if (operators.Count == 0)
            {
                return values.Pop();
            }
            else
            {
                return Result(operators.Pop(), values.Pop(), values.Pop());
            }
        }


        /// <summary>
        /// Given an operator and two doubles returns the result of the operation
        /// </summary>
        public double Result(String s, double d1, double d2)
        {
            switch (s)
            {
                case "*": return d1 * d2;
                case "/":
                    //Check to make sure we aren't dividing by zero
                    if(d2 == 0)
                    {
                        throw new FormulaEvaluationException("Can't divide by 0.");
                    }
                    return d1 / d2;
                case "+": return d1 + d2;
                case "-": return d1 - d2;
                default: throw new FormulaFormatException("Invalid operator");
            }
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
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
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
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
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
