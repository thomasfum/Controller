using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    public class Equal0_2
    {
        public bool a0;
        public bool a1;
        public bool s;
        Invert inv1 = new Invert();
        Invert inv2 = new Invert();
        And and = new And();
        public int proceed()
        {
            int val = 0;
            inv1.input = a0;
            val+=inv1.proceed();
            inv2.input = a1;
            val += inv2.proceed();
            and.a = inv1.output;
            and.b = inv2.output;
            val += and.proceed();
            s = and.output;
            return val;
        }
    }
}
