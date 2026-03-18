using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreEngine
{
    public class MathParser
    {
        public List<string> Tokenize(string expression)
        {
            List<string> tokens = new List<string>();
            string buffer = "";
            foreach (char c in expression)
            {
                if (char.IsDigit(c) || "abcdefABCDEFxX".Contains(c))
                {
                    buffer += c;
                }
                else if ("+-*/()^".Contains(c))
                {
                    if (c == '-' && buffer == "")
                    {
                        if (tokens.Count == 0 || "+-*/(^".Contains(tokens.Last()))
                        {
                            buffer += c;
                            continue;
                        }
                    }
                    if (buffer != "")
                    {
                        tokens.Add(buffer);
                        buffer = "";
                    }
                    tokens.Add(c.ToString());
                }
            }
            if (buffer != "")
            {
                tokens.Add(buffer);
            }
            return tokens;
        }

        private double ParseNumber(string token)
        {
            bool isNegative = false;
            if (token.StartsWith("-"))
            {
                isNegative = true;
                token = token.Substring(1);
            }

            double result;

            if (token.StartsWith("0x") || token.StartsWith("0X"))
            {
                string hexPart = token.Substring(2);

                result = Convert.ToInt64(hexPart, 16);
            }
            else
            {
                result = double.Parse(token);
            }

            return isNegative ? -result : result;
        }

        private double ExecuteMath(double left, double right, string op)
        {
            double result = -1;
            switch (op)
            {
                case "+":
                    result = left + right;
                    break;
                case "-":
                    result = left - right;
                    break;
                case "*":
                    result = left * right;
                    break;
                case "/":
                    if (right == 0) throw new DivideByZeroException("Divide by 0");
                    result = left / right;
                    break;
                case "%":
                    result = left % right;
                    break;
                case "^":
                    result = Math.Pow(left, right);
                    break;
            }
            return result;
        }

        public double Evaluate(List<string> tokens)
        {
            while (tokens.Contains("("))
            {
                int openIndex = tokens.LastIndexOf("(");

                int closeIndex = tokens.IndexOf(")", openIndex);

                //要素数計算
                int count = closeIndex - openIndex - 1;

                List<string> innerTokens = tokens.GetRange(openIndex + 1, count);

                double innerResult = Evaluate(innerTokens);

                int removeCount = closeIndex - openIndex + 1;
                tokens.RemoveRange(openIndex, removeCount);

                tokens.Insert(openIndex, innerResult.ToString());
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "^")
                {
                    double left = ParseNumber(tokens[i - 1]);
                    double right = ParseNumber(tokens[i + 1]);

                    double result = ExecuteMath(left, right, "^");

                    tokens.RemoveRange(i - 1, 3);
                    tokens.Insert(i - 1, result.ToString());
                    i--;
                }
            }

                for (int i = 0; i < tokens.Count; i++)
            {
                string current = tokens[i];

                if (current == "*" || current == "/" || current == "%")
                {
                    double left = ParseNumber(tokens[i - 1]);
                    double right = ParseNumber(tokens[i + 1]);

                    double result = ExecuteMath(left, right, current);

                    tokens.RemoveRange(i - 1, 3);
                    tokens.Insert(i - 1, result.ToString());

                    i--;
                }
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                string current = tokens[i];

                if (current == "+" || current == "-")
                {
                    double left = ParseNumber(tokens[i - 1]);
                    double right = ParseNumber(tokens[i + 1]);

                    double result = ExecuteMath(left, right, current);

                    tokens.RemoveRange(i - 1, 3);
                    tokens.Insert(i - 1, result.ToString());

                    i--;
                }
            }
            return ParseNumber(tokens[0]);
        }
    }
}
