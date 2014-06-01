using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra
{
    class Real: IEquatable<Real>, INumber<Real>, IComparable<Real>
    {
        private double number;

        public Real()
        {
            number = default(double);
        }

        public Real(double num)
        {
            number = num;
        }

        public Real(Real clone)
        {
            number = clone.number;
        }


        public static implicit operator Real(double num)
        {
            return new Real() {number = num};
        }

        public double accNum
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

       
        public Real insert()
        {
            number = Convert.ToDouble(Console.ReadLine());
            return this;
        }


        public static Real operator +(Real one, Real two) 
        {
            return new Real() { number = one.number + two.number };
        }

        public void setDoubleValue(Double val)
        {
            this.number = val;
        }

        public static Real operator -(Real one, Real two)
        {
            return new Real()
            {
                number = one.number - two.number
            };
        }

        public static Real operator *(Real one, Real two)
        {
            return new Real()
            {
                number = one.number * two.number
            };
        }


        public static Real operator /(Real one, Real two)
        {
            return new Real()
            {
                number = one.number / two.number
            };
        }

        public Real Div(Real one, Real two)
        {
            return new Real()
            {
                number = one.number / two.number
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator==(Real a, Real b)
        {
            return (a.number == b.number);
        }

        public static bool operator!=(Real a, Real b)
        {
            return (a.number != b.number);
        } 

        public void printer()
        {
            Console.WriteLine(number);
        }

        public override String ToString()
        {
            return number.ToString();
        }

        public static bool operator <(Real one, Real two)
        {
            return one.number < two.number;
        }

        public static bool operator >(Real one, Real two)
        {
            return !(one.number < two.number);
        }

        public bool Equals(Real other)
        {
            return number == other.number;
        }

        public override bool Equals(object other)
        {
            return true;
        }

        public Real Sum(INumber<Real> other)
        {
            return new Real(this + (Real)other);
        }

        public Real Div(INumber<Real> other)
        {
            return new Real(this / (Real)other);
        }

        public Real Sub(INumber<Real> other)
        {
            return this - (Real)other;
        }

        public Real Mul(INumber<Real> other)
        {
            return this * (Real)other;
        }

        public Real Neg()
        {
            Real tmp = new Real();
            tmp.number = -number;
            return tmp;
        }


        public Real Div(Real other)
        {
            return this / (Real)other;
        }


        public int CompareTo(Real other)
        {
            if (this < other)
                return 1;
            else
                return -1;
        }
    }
}
