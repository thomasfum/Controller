using Controller.Arithmetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class SixteenBitsAdder
    {
        public bool[] a = new bool[16];
        public bool[] b = new bool[16];
        public bool[] s = new bool[16];
        public bool c;
        public bool cOut;
        public void proceed()
        {
            bool lastC = c;
            for (int i = 0; i < 16; i++) 
            {
                FullAdder add = new FullAdder();
                add.a = a[i];
                add.b = b[i];
                add.c = lastC;
                add.proceed();
                s[i] = add.l;
                lastC = add.h;
            }
            cOut = lastC;
        }
    }
}
