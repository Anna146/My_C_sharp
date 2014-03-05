using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator;

namespace Algebra
{
    class main
    {
        public static void Main()
        {
            try
            {/*
                Complex one = new Complex()
                {
                    im = 5,
                    re = 2
                };
                Complex two = new Complex()
                {
                    im = 1,
                    re = 7
                };
                Calc<double, Complex> ca = new Calc<double, Complex>();
                one = ca.Calculator("a+B");
                one.printer();
                int a = 9;*/

                Matrix<Complex> a1 = new Matrix<Complex>(2,2);
                Matrix<Complex> a2 = new Matrix<Complex>(2, 2);
                a1.accLines[0][0] = new Complex(0, 1);
                a1.accLines[0][1] = new Complex(1, 1);
                a1.accLines[1][0] = new Complex(0, 0);
                a1.accLines[1][1] = new Complex(0, 1);
                a2.accLines[0][0] = new Complex(0, 1);
                a2.accLines[1][1] = new Complex(1, 1);
                a2.accLines[0][1] = new Complex(0, 0);
                a2.accLines[1][0] = new Complex(1, 0);

                Polynom<Real> b1 = new Polynom<Real>(3);
                Polynom<Real> b2 = new Polynom<Real>(2);

                b1.accKoef[0] = new Real(1);
                b1.accKoef[1] = new Real(0);
                b1.accKoef[2] = new Real(32);
                b2.accKoef[0] = new Real(3);
                b2.accKoef[1] = new Real(2);

                
                (b1*b2).printer();
                //Calc<Real,Matrix<Real>> cl = new Calc<Real,Matrix<Real>>();
                Calc<Real, Polynom<Real>> cl = new Calc<Real, Polynom<Real>>();
                cl.Calculator("a*B-C");
                int a = 5;
            }
           catch
            {

            }
        }
    }
}
