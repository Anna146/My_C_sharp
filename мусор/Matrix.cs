using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Algebra
{
    class Matrix<Q>:IAlgebrable<Matrix<Q>, Q>, IEquatable<Matrix<Q>> where Q: INumber<Q>, new()
    {

        class Line<T> : IAlgebrable<Line<T>, T>, IEquatable<Line<T>> where T : INumber<T>, new()
        {
            private int size;
            private T[] numbers;

            public Line(int n, T[] arr) {
                size = n;
                numbers = new T[size];
                for (int i=0; i< size; i++)
                {
                    numbers[i] = arr[i];
                }
            }

            public Line(Line<T> l2)
            {
                size = l2.size;
                numbers = new T[size];
                for (int i = 0; i < size; i++)
                {
                    numbers[i] = l2.numbers[i];
                }
            }

            public Line(int n)
            {
                size = n;
                numbers = new T[size];
                for (int i = 0; i < size; i++)
                {
                    numbers[i] = default(T);
                }
            }

            public Line()
            {
                size = 0;
            }

            public T[] toArr() 
            {
                return numbers;
            }

            public T[] accNum
            {
                get
                {
                    return numbers;
                }
                set
                {
                    numbers = value;
                }
            }

            public void insert()
            {
                for (int i = 0; i < size; i++)
                {
                    numbers[i] = new T();
                    numbers[i].insert(); 
                }
            }

            public static Line<T> operator+(Line<T> a, Line<T> b) 
            {
                Line<T> tmp = new Line<T>(a.size);
                for (int i = 0; i < a.size; i++)
                {
                    tmp.accNum[i] = (T)((a.accNum[i]).Sum(b.accNum[i]));
                }
                return tmp;
            }

            public static Line<T> operator -(Line<T> a, Line<T> b)
            {
                Line<T> tmp = new Line<T>(a.size);
                object tmpObj1;
                object tmpObj2;
                for (int i = 0; i < a.size; i++)
                {
                    tmp.accNum[i] = (T)((a.accNum[i]).Sub(b.accNum[i]));
                }
                return tmp;
            }

            public static Line<T> operator *(Line<T> a, Line<T> b)
            {
                Line<T> tmp = new Line<T>(a.size);
                object tmpObj1;
                object tmpObj2;
                for (int i = 0; i < a.size; i++)
                {
                    tmp.accNum[i] = (T)((a.accNum[i]).Mul(b.accNum[i]));
                }
                return tmp;
            }

            public bool Equals(Line<T> other)
            {
                throw new NotImplementedException();
            }

            public Line<T> Sum(Line<T> other)
            {
                return this + other;
            }

            public Line<T> Sub(Line<T> other)
            {
                return this - other;
            }

            public Line<T> Mul(Line<T> other)
            {
                return this * other;
            }

            public Line<T> Mul(T numb)
            {
                Line<T> tmp = new Line<T>(size, numbers);
                for (int i = 0; i < size; i++)
                    tmp.numbers[i] = (T)tmp.numbers[i].Mul(numb);
                return tmp;
            }

            public void printer()
            {
            }
        }

        private int lineNumb;
        private int columnNumb;
        private Line<Q>[] lines;

        //getter/setter
        public int accLineDim
        {
            get
            {
                return lineNumb;
            }
            set
            {
                lineNumb = value;
            }
        }

        public int accColDim
        {
            get
            {
                return columnNumb;
            }
            set
            {
                columnNumb = value;
            }
        }



        //implements IEquitable
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



        //constructors
        // s1 - number of lines, s2 - number of columns
        public Matrix(int s1, int s2)
        {
            lineNumb = s1;
            columnNumb = s2;

            lines = new Line<Q>[s1];
            for (int i = 0; i < s1; i++)
            {
                lines[i] = new Line<Q>(s2);
            }
        }

        public Matrix()
        {
            lineNumb = 0;
            columnNumb = 0;

        }

        //copy constructor
        public Matrix(Matrix<Q> m2)
        {
            lineNumb = m2.lineNumb;
            columnNumb = m2.columnNumb;

            lines = new Line<Q>[columnNumb];
            for (int i = 0; i < lineNumb; i++)
            {
                lines[i] = new Line<Q>(columnNumb);
            }
        }


        // s1 - number of lines, s2 - number of columns
        public Matrix(int s1, int s2, Q[][] linesArr)
        {
            lineNumb = s1;
            columnNumb = s2;

            lines = new Line<Q>[s1];
            for (int i = 0; i < s2; i++)
            {
                lines[i] = new Line<Q>(s1, linesArr[i]);
            }
        }

        //transforms matrix to 2-dimensional array
        public Q[][] toArr()
        {
            Q[][] arr = new Q[lineNumb][];
            for (int i = 0; i < lineNumb; i++)
            {
                arr[i] = lines[i].accNum;
            }
            return arr;
        }

        //getter/setter
        public Q[][] accLines
        {
            get
            {
                return toArr();
            }
            set
            {
                for (int i = 0; i < lineNumb; i++)
                {
                    lines[i].accNum = value[i];
                }
            }
        }

        //allows to inser values into the matrix
        public void insert()
        {
            Console.WriteLine("Insert number of lines: ");
            lineNumb = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Insert number of columns: ");
            columnNumb = Convert.ToInt32(Console.ReadLine());

            lines = new Line<Q>[lineNumb];
            for (int i = 0; i < lineNumb; i++)
            {
                lines[i] = new Line<Q>(columnNumb);
                Console.WriteLine("Line "+i + ": ");
                lines[i].insert();
            }

            this.printer();
        }

        //arifmetic operators
        public static Matrix<Q> operator +(Matrix<Q> one, Matrix<Q> two) 
        {
            Matrix<Q> tmp = new Matrix<Q>(one.lineNumb, one.columnNumb);
            for (int i = 0; i < one.lineNumb; i++)
            {
                for (int j = 0; j < one.columnNumb; j++)
                {
                    tmp.accLines[i][j] = (Q)(one.accLines[i][j].Sum(two.accLines[i][j]));
                }
            }
            return tmp;
        }

        public static Matrix<Q> operator -(Matrix<Q> one, Matrix<Q> two)
        {
            Matrix<Q> tmp = new Matrix<Q>(one.lineNumb, one.columnNumb);
            for (int i = 0; i < one.lineNumb; i++)
            {
                for (int j = 0; j < one.columnNumb; j++)
                {
                    tmp.accLines[i][j] = (Q)(one.accLines[i][j].Sub(two.accLines[i][j]));
                }
            }
            return tmp;
        }
        
        public static Matrix<Q> operator *(Matrix<Q> one, Matrix<Q> two)
        {
            Matrix<Q> tmp = new Matrix<Q>(one.columnNumb, two.lineNumb);
		    for (int i=0; i<one.lineNumb; i++) 
			    for (int j=0; j<one.columnNumb; j++) {
				    tmp.accLines[i][j] = default(Q);
				for (int k=0; k<one.columnNumb; k++) 
					tmp.accLines[i][j] = (Q)(one.accLines[i][k].Mul(two.accLines[k][j].Sum(tmp.accLines[i][j])));
			}
		return tmp;
        }
        
        //comparators
        public static bool operator==(Matrix<Q> a, Matrix<Q> b)
        {
            if (a.columnNumb != b.columnNumb || a.lineNumb != b.lineNumb) return false;
            for (int i = 0; i < a.lineNumb; i++)
            {
                for (int j = 0; j < a.columnNumb; j++)
                {
                    if (!a.accLines[i][j].Equals(b.accLines[i][j])) return false;
                }
            }
            return true;
        }

        public static bool operator!=(Matrix<Q> a, Matrix<Q> b)
        {
            return !(a == b);
        }

        //prints matrix
        public void printer()
        {
            for (int i = 0; i < lineNumb; i++)
            {
                for (int j = 0; j < columnNumb; j++)
                {
                    Console.Write(accLines[i][j]+" ");
                }
                Console.WriteLine();
            }
        }
        
        public override String ToString()
        {
            StringBuilder tmp = new StringBuilder();
            for (int i = 0; i < lineNumb; i++)
            {
                for (int j = 0; j < columnNumb; j++)
                {
                    tmp = tmp.Append((accLines[i][j]).ToString() + " ");
                }
                tmp = tmp.Append("\n");
            }
            return tmp.ToString();
        }

        
        //Implements IAlgebrable
        public Matrix<Q> Sum(Matrix<Q> other)
        {
            return this + other;
        }

        public Matrix<Q> Sub(Matrix<Q> other)
        {
            return this - other;
        }

        public Matrix<Q> Mul(Matrix<Q> other)
        {
            return this * other;
        }

        public Matrix<Q> Mul(Q numb)
        {
            Matrix<Q> tmp = new Matrix<Q>(lineNumb, columnNumb);
            for (int i = 0; i < lineNumb; i++)
            {
                for (int j = 0; j < columnNumb; j++)
                {
                    tmp.accLines[i][j] = (Q)(accLines[i][j].Sum(numb));
                }
            }
            return tmp;
        }

        public Matrix<Q> Div(Q numb)
        {
            Matrix<Q> tmp = new Matrix<Q>(lineNumb, columnNumb);
            for (int i = 0; i < lineNumb; i++)
            {
                for (int j = 0; j < columnNumb; j++)
                {
                    tmp.accLines[i][j] = (Q)(accLines[i][j].Div(numb));
                }
            }
            return tmp;
        }

        //Implements IEquatable
        public bool Equals(Matrix<Q> other)
        {
            return this == other;
        }

        //Matrix features
        public Q trace()
        {
            Q tmp = new Q();

            for (int i = 0; i < columnNumb; i++)
                tmp = tmp.Sum(accLines[i][i]);

            return tmp;
        }

        public Q det() 
        {
            Q tmp = new Q();

		    if (columnNumb != 1) {
		        int pow = -1;
		        for (int i=0; i < lineNumb; i++) {
			        pow *= (-1);
			        Matrix<Q> mChild = new Matrix<Q>(lineNumb, columnNumb); 
			    for (int j=1; j < lineNumb; j++) {
				    for (int k=0, l=0; k < lineNumb; k++, l++) {
					    if (k == i) 
                            k++; 
                        mChild.accLines[j-1][l] = this.accLines[j][k];
				}
			}
            Q powQ = new Q();
            powQ.setDoubleValue(pow);
			tmp = tmp.Sum((powQ.Mul(accLines[0][i])).Mul(mChild.det()));
		}
		}
		else return accLines[0][0];
		return tmp;
        }
    }
}
