using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Xor
    {
        public bool a;
        public bool b;
        public bool output;
        Or or = new Or();
        Nand nand = new Nand();
        And and = new And();
        public int proceed()
        {
            int val = 0;
            or.a = a;
            or.b = b;
            val += or.proceed();
            nand.a = a;
            nand.b = b;
            val += nand.proceed();
            and.a = or.output;
            and.b = nand.output;
            val += and.proceed();
            output = and.output;
            return val;
        }

    }
}
