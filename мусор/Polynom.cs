using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra
{
    class Polynom<Q>: IAlgebrable<Polynom<Q>, Q>, IEquatable<Polynom<Q>> where Q : INumber<Q>, new()
    {
            //it's not actually a polynom degree but rather deg = polynom degree + 1
            private int deg;
            //the array starts from the 0 degree to deg-1
            private Q[] koef;

            //getter/setter
            public int accDeg
            {
                get
                {
                    return deg;
                }
                set
                {
                    deg = value;
                }
            }


            //geter/setter
            public Q[] accKoef
            {
                get
                {
                    return koef;
                }
                set
                {
                    for (int i = 0; i < deg; i++)
                    {
                        koef[i] = value[i];
                    }
                }
            }

            //constructors
            //makes polynom with n degree
            public Polynom(int n)
            {
                deg = n;

                koef = new Q[n];
                for (int i = 0; i < n; i++)
                {
                    koef[i] = new Q();
                }
            }

            public Polynom()
            {
                deg=0;
            }
            
            //copy constructor
            public Polynom(Polynom<Q> p2)
            {
                deg = p2.deg;

                koef = new Q[deg];
                for (int i = 0; i < deg; i++)
                {
                    koef[i] = new Q();
                }
            }
            
            //makes polynom with koefArr koefficients
            public Polynom(int n, Q[] koefArr)
            {
                deg = n;

                koef = new Q[n];
                for (int i = 0; i < n; i++)
                {
                    koef[i] = new Q();
                }
            }




            //returns an array of koefficients
            public Q[] toArr()
            {
                Q[] arr = new Q[deg];
                for (int i = 0; i < deg; i++)
                {
                    arr[i] = koef[i];
                }
                return arr;
            }


            //allows to insert values from console
            public void insert()
            {
                Console.WriteLine("Insert the degree: ");
                deg = Convert.ToInt32(Console.ReadLine());

                koef = new Q[deg];
                for (int i = 0; i < deg; i++)
                {
                    koef[i] = new Q();
                    Console.WriteLine("Insert koef for x^" + i + " : ");
                    koef[i].insert();
                }

                this.printer();
            }

            //arifmetic operations
            public static Polynom<Q> operator +(Polynom<Q> one, Polynom<Q> two)
            {
                int resDeg = (one.deg > two.deg) ? one.deg : two.deg;
                int smallDeg = (one.deg < two.deg) ? one.deg : two.deg;

                Polynom<Q> tmp = new Polynom<Q>(resDeg);

                for (int i = 0; i < smallDeg; i++)
                    {
                        tmp.koef[i] = (Q)(one.koef[i].Sum(two.koef[i]));
                    }

                if (resDeg == one.deg) 
                {
                    for (int i = smallDeg; i< resDeg; i++)
                        tmp.koef[i] = one.koef[i];
                }
                else 
                {
                    for (int i = smallDeg; i< resDeg; i++)
                        tmp.koef[i] = two.koef[i];
                }
                return tmp;
            }

            public static Polynom<Q> operator -(Polynom<Q> one, Polynom<Q> two)
            {
                int resDeg = (one.deg > two.deg) ? one.deg : two.deg;
                int smallDeg = (one.deg < two.deg) ? one.deg : two.deg;

                Polynom<Q> tmp = new Polynom<Q>(resDeg);

                for (int i = 0; i < smallDeg; i++)
                    {
                        tmp.koef[i] = (Q)(one.koef[i].Sub(two.koef[i]));
                    }

                if (resDeg == one.deg) 
                {
                    for (int i = smallDeg; i< resDeg; i++)
                        tmp.koef[i] = one.koef[i];
                }
                else 
                {
                    for (int i = smallDeg; i< resDeg; i++)
                        tmp.koef[i] = two.koef[i].Neg();
                }
                return tmp;
            }


            public static Polynom<Q> operator *(Polynom<Q> one, Polynom<Q> two)
            {
                Polynom<Q> tmp = new Polynom<Q>(one.deg + two.deg - 1);

                for (int i=0; i<one.deg; i++) 
                {
                    for (int j=0; j<two.deg; j++)
                    {
                        tmp.koef[i+j] = tmp.koef[i+j].Sum(one.koef[i].Mul(two.koef[j]));
                    }
                }

                return tmp;
            }

            //comarators
            public static bool operator ==(Polynom<Q> a, Polynom<Q> b)
            {
                if (a.deg != b.deg || a.deg != b.deg)
                    return false;
                for (int i = 0; i < a.deg; i++)
                {
                    for (int j = 0; j < a.deg; j++)
                    {
                        if (!a.koef[i].Equals(b.koef[i]))
                            return false;
                    }
                }
                return true;
            }

            public static bool operator !=(Polynom<Q> a, Polynom<Q> b)
            {
                return !(a == b);
            }

            //implements IEquatable
            public override bool Equals(object obj)
            {
                Polynom<Q> tmp = (Polynom<Q>)obj;

                return this == tmp;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            
            //prints polynom
            public void printer()
            {
                for (int j = 0; j < deg; j++)
                    {
                        Console.Write(koef[j] + "*x^" + j);
                        if (j < deg - 1)
                            Console.Write("+");
                    }
                Console.WriteLine();
            }

            public override String ToString()
            {
                StringBuilder tmp = new StringBuilder();
                for (int i = 0; i < deg; i++)
                {
                    tmp = tmp.Append((koef[i]).ToString() + " ");
                }
                return tmp.ToString();
            }

            //implements INumber
            public Polynom<Q> Sum(Polynom<Q> other)
            {
                return this + other;
            }

            public Polynom<Q> Sub(Polynom<Q> other)
            {
                return this - other;
            }

            public Polynom<Q> Mul(Polynom<Q> other)
            {
                return this * other;
            }

            public Polynom<Q> Mul(Q numb)
            {
                Polynom<Q> tmp = new Polynom<Q>(deg);
                for (int i = 0; i < deg; i++)
                {
                    tmp.koef[i] = (Q)(koef[i].Mul(numb));
                }
                return tmp;
            }

            public Polynom<Q> Div(Q numb)
            {
                Polynom<Q> tmp = new Polynom<Q>(deg);
                for (int i = 0; i < deg; i++)
                {
                    tmp.koef[i] = (Q)(koef[i].Div(numb));
                }
                return tmp;
            }

            public bool Equals(Polynom<Q> other)
            {
                return this == other;
            }
        }
    }

