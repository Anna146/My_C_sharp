using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra
{
    interface INumber<T>
    {
        T insert();

        T Sum(INumber<T> other);

        T Sub(INumber<T> other);

        T Mul(INumber<T> other);

        T Div(INumber<T> other);

        T Neg();

        void setDoubleValue(Double val);

        bool Equals(T other);

        void printer();
    }
}
