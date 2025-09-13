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
        public int proceed()
        {

            int val = 0;
            Or or = new Or();
            or.a = a;
            or.b = b;
            val += or.proceed();

     
            Nand nand = new Nand();
            nand.a = a;
            nand.b = b;
            val += nand.proceed();


            And and = new And();
            and.a = or.output;
            and.b = nand.output;
            val += and.proceed();


            output = and.output;
            return val;
        }

    }
}
