using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class And
    {

        public bool a;
        public bool b;
        public bool output;
        Nand nand = new Nand();
        Invert invert = new Invert();
        

        public int proceed()
        {
            int val = 0;
            nand.a = a; nand.b=b;
            val+=nand.proceed();
            invert.input = nand.output;
            val += invert.proceed();
            output =invert.output;
            return val;
        }
    }
}
