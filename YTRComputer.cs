using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class YTRComputer
    {
        /*
          // example 
          string result = new YTRComputer("(2+5)^2+(10*2)");
          Console.Writeline(result);
         
          // output
          result = 69
         
         */
        public string Compute(string arg)
        {
            while (arg.Contains(" "))
                arg = arg.Replace(" ", "");

            if (checkValidationExp(arg) == false)
                return null;//invalid Exp

            if (arg.Contains("("))
            {
                arg = computeBrackets(arg);
                if (arg == null)
                    return null;
            }

            if (arg.Contains("^"))
            {
                string tmp = arg;
                // 2+3^2*4^2
                while (tmp.Contains("^"))
                {
                    char[] operators = new char[] { '+', '-', '*', '/', '%', '^' };

                    string[] pieces = Regex.Split(tmp, @"(?<=[+*/%^-])");

                    int idxExp = -1;
                    for (int i = 0; i < pieces.Length; i++)
                    {
                        if (pieces[i].Contains("^"))
                        {
                            idxExp = i;
                            break;
                        }
                    }

                    string computeThis = pieces[idxExp] + pieces[idxExp + 1];
                    string computed = computeSimplestExp(computeThis);

                    string originExpStatement = pieces[idxExp] + pieces[idxExp + 1];
                    if (pieces.Length > idxExp + 2)
                        originExpStatement = originExpStatement.Substring(0, originExpStatement.Length - 1);

                    tmp = tmp.Replace(originExpStatement, computed);

                }

                arg = tmp;
            }
            try
            {
                return new DataTable().Compute(arg, "").ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private string computeBrackets(string statement)
        {
            int nOpenBracket = statement.Count(f => f == '(');
            int nCloseBracket = statement.Count(f => f == ')');

            if (nCloseBracket != nOpenBracket)
                return null;//invalid bracket

            string newStatement = statement;
            while (newStatement.Contains("(") == true)
            {
                int openParentheseIdx = newStatement.LastIndexOf("(");
                string tmp = newStatement.Substring(openParentheseIdx);

                string computeThis = tmp.Substring(1, tmp.IndexOf(")") - 1);

                string result = Compute(computeThis);

                newStatement = newStatement.Replace("(" + computeThis + ")", result);
            }
            return newStatement;
        }
        private bool checkValidationExp(string arg)
        {
            if (arg.IndexOf("^") == 0 || arg.LastIndexOf("^") == arg.Length - 1)
                return false;

            return true;
        }
        private string computeSimplestExp(string simplestExpOperation)
        {
            string[] arr = simplestExpOperation.Split('^');

            string result = "";

            for (int i = 0; i < arr.Length; i++)
                result += "(" + arr[0] + ")" + "*";

            result = result.Substring(0, result.Length - 1);
            string value = new DataTable().Compute(result, "").ToString();
            return value;
        }
        
    }
}
