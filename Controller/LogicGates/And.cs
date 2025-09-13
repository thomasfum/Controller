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

        public int proceed()
        {
            int val = 0;
            Nand nand= new Nand();
            nand.a = a; nand.b=b;
            val+=nand.proceed();
            Invert invert= new Invert();
            invert.input = nand.output;
            val += invert.proceed();
            output =invert.output;
            return val;
        }
    }
}
