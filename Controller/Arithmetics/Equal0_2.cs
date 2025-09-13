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
        public int proceed()
        {
            int val = 0;
            Invert inv1= new Invert();
            inv1.input = a0;
            val+=inv1.proceed();

            Invert inv2 = new Invert();
            inv2.input = a1;
            val += inv2.proceed();

            And and= new And();
            and.a = inv1.output;
            and.b = inv2.output;
            val += and.proceed();
            s = and.output;
            return val;
        }
    }
}
