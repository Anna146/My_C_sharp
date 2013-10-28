using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator;

namespace Algebra
{
    class Complex: IEquatable<Complex>, INumber<Complex>
    {
        private double im;
        private double re;

        public Complex()
        {
            im = default(double);
            re = default(double);
        }

        public Complex(double Im, double Re)
        {
            im = Im;
            re = Re;
        }

        public Complex(Complex clone)
        {
            im = clone.im;
            re = clone.re;
        }

        public Complex(double number)
        {
            re = number;
            im = 0;
        }

        public static implicit operator Complex(double num)
        {
            return new Complex() {re = num, im = 0};
        }

        public double accRe
        {
            get
            {
                return re;
            }
            set
            {
                re = value;
            }
        }

        public double accIm
        {
            get
            {
                return im;
            }
            set
            {
                im = value;
            }
        }

        // you should insert first real part then enter then imaginary
        public Complex insert()
        {
            re = Convert.ToDouble(Console.ReadLine());
            im = Convert.ToDouble(Console.ReadLine());
            return this;
        }


        public static Complex operator +(Complex one, Complex two) 
        {
            return new Complex() { re = one.re + two.re, im = one.im + two.im };
        }

        public static Complex operator -(Complex one, Complex two)
        {
            return new Complex()
            {
                re = one.re - two.re,
                im = one.im - two.im
            };
        }

        public static Complex operator *(Complex one, Complex two)
        {
            return new Complex()
            {
                re = one.re * two.re - one.im * two.im,
                im = one.im * two.re + one.re * two.im
            };
        }

        public static Complex operator /(Complex one, Complex two)
        {
            return new Complex()
            {
                re = (one.re * two.re + one.im * two.im) / (two.re * two.re + two.im * two.im),
                im = (one.im * two.re - one.re * two.im) / (two.re * two.re + two.im * two.im)
            };
        }

        public static bool operator==(Complex a, Complex b)
        {
            return ((a.re == b.re) && (a.re == b.re));
        }

        public static bool operator!=(Complex a, Complex b)
        {
            return ((a.re != b.re) || (a.re != b.re));
        }
        
        

        public void printer()
        {
            Console.WriteLine(re + " + " + im + "i");
        }

        public override String ToString()
        {
            return re + " + " + im + "i";
        }


        public bool Equals(Complex other)
        {
            return (re == other.re) && (im == other.im);
        }

        public Complex Sum(INumber<Complex> other)
        {
            return this + (Complex)other;
        }

        public Complex Sub(INumber<Complex> other)
        {
            return this - (Complex)other;
        }

        public Complex Mul(INumber<Complex> other)
        {
            return this * (Complex)other;
        }

        public Complex Neg()
        {
            Complex tmp = new Complex();
            tmp.re = -re;
            tmp.im = -im;
            return tmp;
        }

        void setDoubleValue(Double val)
        {
            re = val;
            im = 0;
        }

        public Complex Div(Complex other)
        {
            return this / (Complex)other;
        }
    }
}
