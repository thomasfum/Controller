using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Switching
{
    public class Selector
    {
        public bool s;
        public bool d0;
        public bool d1;
        public bool outout;

        And and1 = new And();
        And and2 = new And();
        Invert inv = new Invert();
        Or or = new Or();

        public int proceed()
        {
            int val = 0;
          
            and1.a = s;
            and1.b = d1;
            val+=and1.proceed();

            inv.input = s;
            val+=inv.proceed();

            and2.a = inv.output;
            and2.b = d0;
            val += and2.proceed();
            or.a = and1.output;
            or.b = and2.output;
            val += or.proceed();

            outout=or.output;

            return val;
        }

    }
}
