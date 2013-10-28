using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algebra;

namespace Calculator
{
    //the program uses the method of postfix notation combined with two stacks

    class Calc<elem, obj>
        where obj : IAlgebrable<obj,elem>, new()
        where elem : INumber<elem>, new()
    {
        private List<string> objects = new List<string>();
        private List<string> numbers = new List<string>();

        Dictionary<string, obj> objNames = new Dictionary<string, obj>();
        Dictionary<string, elem> numNames = new Dictionary<string, elem>();


        //transforms input string expession into string in postfix notation
        private string StrToPost (string str) 
        {
            Stack<char> stackChr = new Stack<char>();
            StringBuilder postfix = new StringBuilder();
            stackChr.Push('0');
            int i = 0;


            while (i < str.Length)
            {

                if (Char.IsLetter(str[i]))
                {
                    postfix = postfix.Append(str[i]);
                }

                if (IsOperator(str[i]))
                {
                    postfix = postfix.Append(' ');
                    while ((stackChr.Peek() != '0') && (GetPriority(stackChr.Peek()) <= GetPriority(str[i])))
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
                        if (stackChr.Peek() == '0')
                            throw new Exception("Wrong brackets!");
                        postfix.Append(stackChr.Pop());
                    }
                    stackChr.Pop();
                }

                //if (!Char.IsNumber(str[i]) && !IsOperator(str[i]) && IsBracket(str[i]) == 0 && !(str[i] == ' ')) 
                  //  throw new Exception("Illegal symbol!");
                i++;
            }

            while (stackChr.Peek() != '0')
            {
                if (stackChr.Peek() == '(')
                    throw new Exception("Wrong brackets!");
                postfix = postfix.Append(' ');
                postfix.Append(stackChr.Pop());
            }

            return postfix.ToString();
        }




        //looks for objects in the input string and matches them with values
        private void ObjectParcer(string str)
        {
            StringBuilder tmp = new StringBuilder();
            int i = 0;

            while (i < str.Length)
            {
                if (Char.IsLetter(str[i]) && Char.IsUpper(str[i]))
                {
                    while ((i < str.Length) && Char.IsUpper(str[i]))
                    {
                        tmp = tmp.Append(str[i]);
                        i++;
                    }

                    if (!objects.Contains(tmp.ToString()))
                        objects.Add(tmp.ToString());
                    tmp.Clear();
                }

                if (i<str.Length && Char.IsLetter(str[i]) && Char.IsLower(str[i]))
                {
                    while ((i < str.Length) && Char.IsLower(str[i]))
                    {
                        tmp = tmp.Append(str[i]);
                        i++;
                    }

                    if (!numbers.Contains(tmp.ToString()))
                        numbers.Add(tmp.ToString());
                    tmp.Clear();
                }
                i++;
            }

        }




        //Calculates the value of the expression written in postfix notation
        private String PostfixCalc(string str)
        {
            Stack<String> stackDbl = new Stack<String>();
            StringBuilder number = new StringBuilder();
            StringBuilder tmp1 = new StringBuilder();
            StringBuilder tmp2 = new StringBuilder();

            int i = 0;
            
            while (i < str.Length)
            {

                if (Char.IsLetter(str[i]) || Char.IsNumber(str[i]))
                {
                    number.Append(str[i]);
                }

                if (str[i] == ' ' && number.ToString() != "")
                {
                    stackDbl.Push(number.ToString());
                    number.Clear();
                }

                if (IsOperator(str[i]))
                {
                    if (stackDbl.Count < 2)
                        throw new Exception("Illegal input!");

                    
                    tmp1.Insert(0, stackDbl.Pop());
                    tmp2.Insert(0, stackDbl.Pop());
                    

                    if (Char.IsLower(tmp1[0]) && Char.IsLower(tmp2[0])) {
                        elem res = new elem();
                        res = CalculateNum(tmp1.ToString(), tmp2.ToString(), str[i]);
                        numNames.Add("a" + i, res);
                        stackDbl.Push("a" + i);
                    }

                    if (Char.IsUpper(tmp1[0]) && Char.IsUpper(tmp2[0]))
                    {
                        obj res = new obj();
                        res = ObjCalculate(tmp1.ToString(), tmp2.ToString(), str[i]);
                        objNames.Add("A" + i, res);
                        stackDbl.Push("A" + i);
                    }

                    if (Char.IsLower(tmp1[0]) && Char.IsUpper(tmp2[0]))
                    {
                        obj res = new obj();
                        res = MulCalculate(tmp2.ToString(), tmp1.ToString(), str[i]);
                        objNames.Add("A" + i, res);
                        stackDbl.Push("A" + i);
                    }

                    if (Char.IsLower(tmp2[0]) && Char.IsUpper(tmp1[0]))
                    {
                        obj res = new obj();
                        res = MulCalculate(tmp1.ToString(), tmp2.ToString(), str[i]);
                        objNames.Add("A" + i, res);
                        stackDbl.Push("A" + i);
                    }
                    tmp1.Clear();
                    tmp2.Clear();
                }

                i++;
            }

            if (stackDbl.Count > 2) 
                throw new Exception("Illegal input!");

            //it means that we have a number in the input string
            if (stackDbl.Count < 1)
                stackDbl.Push(number.ToString());

            return stackDbl.Pop();

        }



        //Calculates the value of the input string
        public void Calculator(string str)
        {
            List<string> vars = new List<string>();
            ObjectParcer(str);

            if (objects.Count != 0)
            {
                for (int i = 0; i < objects.Count(); i++)
                {
                    Console.WriteLine("Insert the object " + objects[i] + ":");
                    obj tmp = new obj();
                    tmp.insert();
                    objNames.Add(objects[i], tmp);
                }
            }

            if (numbers.Count != 0)
            {
                for (int i = 0; i < numbers.Count(); i++)
                {
                    Console.WriteLine("Insert the number " + numbers[i] + ":");
                    elem tmp = new elem();
                    tmp.insert();
                    numNames.Add(numbers[i], tmp);
                }
            }

            //str = StrToPost(str);
            str = PostfixCalc(StrToPost(str));

            if (Char.IsLower(str[0]))
                numNames[str].printer();
            else
                objNames[str].printer();
        }



        //returns the result of the operator
        //two objects
        private obj ObjCalculate (string a, string b, char oper)
        {
            switch (oper)
            {
                case '+':
                    {
                        return (obj)(objNames[a].Sum(objNames[b]));
                    }
                case '-':
                    {
                        return (obj)(objNames[b].Sub(objNames[a]));
                    }
                case '*':
                    {
                        return (obj)(objNames[a].Mul(objNames[b]));
                    }
            }
            return default(obj);
        }

        //two elements
        private elem CalculateNum(String a, String b, char oper)
        {
            switch (oper)
            {
                case '+':
                    {
                        return (elem)(numNames[a].Sum(numNames[b]));
                    }
                case '-':
                    {
                        return (elem)(numNames[a].Sub(numNames[b]));
                    }
                case '*':
                    {
                        return (elem)(numNames[a].Mul(numNames[b]));
                    }
            }
            return default(elem);
        }

        ////a number and an object
        private obj MulCalculate(string a, string b, char oper)
        {
            return (obj)(objNames[a].Mul(numNames[b]));
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



        
    }
}
