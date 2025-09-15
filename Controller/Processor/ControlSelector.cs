using Controller.Switching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Processor
{
    public class ControlSelector
    {
        public bool s;
        public bool[] R0 = new bool[16];
        public bool a0;
        public bool d0;
        public bool ca0;
        public bool j0;

        public bool[] R1 = new bool[16];
        public bool a1;
        public bool d1;
        public bool ca1;
        public bool j1;


        public bool[] R = new bool[16];
        public bool a;
        public bool d;
        public bool ca;
        public bool j;

        public int proceed()
        {
            int val = 0;
            Selector_16 selector_16 = new Selector_16();
            Selector select_a = new Selector();
            Selector select_d = new Selector();
            Selector select_ca = new Selector();
            Selector select_j = new Selector();


            selector_16.s = s;
            selector_16.D0 = R0;
            selector_16.D1 = R1;
            val += selector_16.proceed();
            R = selector_16.output;


            select_a.s = s;
            select_a.d0 = a0;
            select_a.d1 = a1;
            val += select_a.proceed();
            a = select_a.outout;

            select_d.s = s;
            select_d.d0 = d0;
            select_d.d1 = d1;
            val += select_d.proceed();
            d = select_d.outout;


            select_ca.s = s;
            select_ca.d0 = ca0;
            select_ca.d1 = ca1;
            val += select_ca.proceed();
            ca = select_ca.outout;

            select_j.s = s;
            select_j.d0 = j0;
            select_j.d1 = j1;
            val += select_j.proceed();
            j = select_j.outout;

            return val;
        }
        
    }
}
