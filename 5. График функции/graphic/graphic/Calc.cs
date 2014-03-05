using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    //the program uses the method of postfix notation combined with two stacks

    class Calc
    {
        //transforms input string expession into string in postfix notation
        private string StrToPost (string str) 
        {
            Stack<char> stackChr = new Stack<char>();
            StringBuilder postfix = new StringBuilder();
            stackChr.Push('b');
            int i = 0;


            while (i < str.Length)
            {

                if (Char.IsNumber(str[i]) || (str[i] == '.'))
                {
                    postfix = postfix.Append(str[i]);
                }

                if (IsOperator(str[i]))
                {
                    postfix = postfix.Append(' ');
                    while ((stackChr.Peek() != 'b') && (GetPriority(stackChr.Peek()) <= GetPriority(str[i])))
                    {
                        postfix.Append(stackChr.Pop());
                        postfix = postfix.Append(' ');
                    }
                    stackChr.Push(str[i]);
                }

                if (IsBracket(str[i]) == 1)
                {
                    stackChr.Push(str[i]);
                }

                if (IsBracket(str[i]) == 2)
                {
                    while (IsBracket(stackChr.Peek()) != 1)
                    {
                        if (stackChr.Peek() == 'b')
                            throw new Exception("Wrong brackets!");
                        postfix.Append(stackChr.Pop());
                    }
                    stackChr.Pop();
                }

                if (!Char.IsNumber(str[i]) && !IsOperator(str[i]) && IsBracket(str[i]) == 0 && !(str[i] == ' ') && !(str[i] == '.')) 
                    throw new Exception("Illegal symbol!");
                i++;
            }

            while (stackChr.Peek() != 'b')
            {
                if (stackChr.Peek() == '(')
                    throw new Exception("Wrong brackets!");
                postfix = postfix.Append(' ');
                postfix.Append(stackChr.Pop());
            }

            return postfix.ToString();
        }


        //looks for variants in the input string and matches them with values
        private List<string> VarParcer(string str)
        {
            StringBuilder tmp = new StringBuilder();
            int i = 0;
            List<string> vars = new List<string>();

            while (i < str.Length)
            {
                if (Char.IsLetter(str[i]))
                {
                    while ((i < str.Length) && Char.IsLetter(str[i]))
                    {
                        tmp = tmp.Append(str[i]);
                        i++;
                    }

                    if (!vars.Contains(tmp.ToString()))
                        vars.Add(tmp.ToString());
                    tmp.Clear();
                }
                i++;
            }

            return vars;
        }


        //finds function calls in the string and Calculates them. These include: sin, cos, ln
        private string FunctionParcer(string str, double x)
        {
            StringBuilder tmp = new StringBuilder();

            while (str.Contains("cos("))
            {
                int start = str.IndexOf("cos(");
                int i = start + 4;

                while (str[i] != ')')
                {
                    tmp = tmp.Append(str[i]);
                    i++;
                }

                double val = Math.Cos(Calculator(tmp.ToString(),x));
                str = str.Remove(start, i - start + 1);
                str = str.Insert(start, val.ToString());
                tmp.Clear();
            }

            while (str.Contains("sin("))
            {
                int start = str.IndexOf("sin(");
                int i = start + 4;

                while (str[i] != ')')
                {
                    tmp = tmp.Append(str[i]);
                    i++;
                }

                double val = Math.Sin(Calculator(tmp.ToString(), x));
                str = str.Remove(start, i - start + 1);
                str = str.Insert(start, val.ToString());
                tmp.Clear();
            }

            while (str.Contains("ln("))
            {
                int start = str.IndexOf("ln(");
                int i = start + 4;

                while (str[i] != ')')
                {
                    tmp = tmp.Append(str[i]);
                    i++;
                }

                double val = Math.Log(Calculator(tmp.ToString(), x));
                str = str.Remove(start, i - start + 1);
                str = str.Insert(start, val.ToString());
                tmp.Clear();
            }

            return str;
        }


        //Calculates the value of the expression written in postfix notation
        private double PostfixCalc(string str)
        {
            Stack<double> stackDbl = new Stack<double>();
            StringBuilder number = new StringBuilder();
            int i=0;

            while (i < str.Length)
            {

                if (Char.IsNumber(str[i]) || (str[i] == '.'))
                {
                    number.Append(str[i]);
                }

                if ((str[i] == ' ' || IsOperator(str[i])) && number.ToString() != "")
                {
                    try
                    {
                        stackDbl.Push(double.Parse(number.ToString().Replace(".", ",")));
                        number.Clear();
                    }
                    catch (Exception a)
                    {
                    }
                }

                if (IsOperator(str[i]))
                {
                    if (stackDbl.Count < 2)
                        throw new Exception("Illegal input!");
                    stackDbl.Push(Calculate(stackDbl.Pop(), stackDbl.Pop(), str[i]));
                }

                i++;
            }

            if (stackDbl.Count > 2) 
                throw new Exception("Illegal input!");
            //it means that we have a number in the input string
            if (stackDbl.Count < 1)
                stackDbl.Push(double.Parse(number.ToString().Replace(".", ",")));
            return stackDbl.Pop();
        }



        //Calculates the value of the input string
        public double Calculator(string str, double x)
        {
            List<string> vars = new List<string>();
            str = FunctionParcer(str, x);
            vars = VarParcer(str);
            String tmp = x.ToString("f");
            int a = tmp.IndexOf("E");
            if (a > 0)
                tmp = tmp.Remove(a);
            if (x < 0)
                tmp = "(0" + tmp + ")";


            tmp = tmp.Replace(",", ".");
            str = str.Replace(",", ".");
            if (vars.Count == 0)
            {
                str = negCheck(str);
                return PostfixCalc(StrToPost(str));
            }
            else
            {
                str = str.Replace(vars[0], tmp);
                str = negCheck(str);
                return PostfixCalc(StrToPost(str));
            }
            
        }

        //if we have a kind of (-1)*5
        string negCheck(string str)
        {
            if (str[0] == '-')
                str = '0' + str;
            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == '-' && str[i - 1] == '(')
                    str = str.Substring(0, i) + '0' + str.Substring(i, str.Length - i);
            }
            return str;
        }

        //returns the result of the operator
        private double Calculate(double a, double b, char oper)
        {
            switch (oper)
            {
                case '*': return a * b;
                case '/':
                    {
                        if (a == 0) {
                            if (b > 0)
                                a = double.PositiveInfinity;
                            else
                                a = double.NegativeInfinity;
                            }
                            //throw new Exception("Division by zero!");
                        return b / a;
                    }
                case '+': return a + b;
                case '-': return b - a;
                case '^': return Math.Pow(b,a);
            }
            return 0;
        }


        //returns the priority of the operator
        private int GetPriority(char oper)
        {
            switch (oper)
            {
                case '*': return 1;
                case '/': return 1;
                case '+': return 2;
                case '-': return 2;
                case '^': return 0;
            }
            return 3;
        }


        //checks whether the input char is a bracket: 1-opening bracket 2-closing bracket
        private int IsBracket(char ch)
        {
            if (ch == '(') 
                return 1;
            else 
            {
                if (ch == ')') 
                    return 2;
                else 
                    return 0;
            }
        }


        //checks whether char is an operator
        private bool IsOperator(char p)
        {
            if (p == '*' || p =='/' || p =='+' || p =='-' || p =='^')
                return true;
            else 
                return false;
        }




        static void main(string[] args)
        {
            try
            {
                String inpStr = Console.ReadLine();
                //string inpStr = "3+cos(15-3*5)*a/(1-5)^b";
                Calc c = new Calc();
                //Console.WriteLine(c.Calculator(inpStr).ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
