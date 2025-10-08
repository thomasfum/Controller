using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Or
    {
        public bool a;
        public bool b;
        public bool output;
        Invert inv1 = new Invert();
        Invert inv2 = new Invert();
        Nand nand = new Nand();
        public int proceed()
        {
            int val = 0;
            inv1.input = a;
            val += inv1.proceed();
            inv2.input = b;
            val += inv2.proceed();
            nand.a = inv1.output; 
            nand.b = inv2.output;
            val += nand.proceed();
            output = nand.output;
            return val;
        }
    }
}
