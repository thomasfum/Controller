using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.LogicGates
{

    public class And_16
    {
        public bool[] a = new bool[16];
        public bool[] b = new bool[16];
        public bool[] s = new bool[16];
        And []and=new And [16];
        public And_16()
        {
            for (int i = 0; i < 16; i++)
                and[i] = new And();
        }
        public int proceed()
        {
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                and[i].a = a[i];
                and[i].b = b[i];
                val += and[i].proceed();
                s[i] = and[i].output;
            }
            return val;
        }
    }
}
