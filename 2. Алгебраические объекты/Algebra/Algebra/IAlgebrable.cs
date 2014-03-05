using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algebra
{
    interface IAlgebrable <Q, elem>
    {
        void insert();

        Q Sum(Q other);

        Q Sub(Q other);

        Q Mul(Q other);

        Q Mul(elem other);

        Q Div(elem other);

        void printer();
    }
}
