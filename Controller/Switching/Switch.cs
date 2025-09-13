using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Switching
{
    public class Switch
    {
        public bool s;
        public bool d;
        public bool c0;
        public bool c1;
        public int proceed()
        {
            int val = 0;
            Invert inv = new Invert();
            inv.input = s;
            val += inv.proceed();

            And and1 = new And();
            And and2 = new And();
            and1.a = s;
            and1.b = d;
            val += and1.proceed();

            and2.a = inv.output;
            and2.b = d;
            val += and2.proceed();

            c1 = and1.output;
            c0 = and2.output;

            return val;
        }
    }
}
