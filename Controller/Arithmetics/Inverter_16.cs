using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    public class Inverter_16
    {
        public bool[] a = new bool[16];
        public bool[] s = new bool[16];
       
        public void proceed()
        {
            for (int i = 0; i < 16; i++)
            {
                Invert inv = new Invert();
                inv.input = a[i];
                inv.proceed();
                s[i] = inv.output;
            }
            
        }
    }
}
