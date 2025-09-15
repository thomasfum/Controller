using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Memory
{
    public class SR_Latch
    {
        public bool s;
        public bool r;
        public bool output;
        Nand nand1 = new Nand();
        Nand nand2 = new Nand();
        public int proceed()
        {
            int val = 0;
      

            nand1.a = s;
            nand1.b= nand2.output;
            val += nand1.proceed();

            nand2.a = nand1.output;
            nand2.b = r;
     

            val += nand2.proceed();
            output=nand2.output;
            return val;
        }
    }
}
