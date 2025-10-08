using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Switching
{
    public class Switch_3_8
    {
        public bool a0;
        public bool a1;
        public bool a2;
        public bool d;
        public bool[] c = new bool[8];

        Switch_2_4 switch_1And2 = new Switch_2_4();
        Switch_1_2 switch_3_1 = new Switch_1_2();
        Switch_1_2 switch_3_2 = new Switch_1_2();
        Switch_1_2 switch_3_3 = new Switch_1_2();
        Switch_1_2 switch_3_4 = new Switch_1_2();

        public int proceed()
        {
            int val = 0;
            
            switch_1And2.a0 = a1;
            switch_1And2.a1 = a2;
            switch_1And2.d = d;
            val+=switch_1And2.proceed();
           
            switch_3_1.a = a0;
            switch_3_1.d = switch_1And2.c[0];
            val += switch_3_1.proceed();
            c[0]= switch_3_1.c0;
            c[1] = switch_3_1.c1;

            switch_3_2.a = a0;
            switch_3_2.d = switch_1And2.c[1];
            val += switch_3_2.proceed();
            c[2] = switch_3_2.c0;
            c[3] = switch_3_2.c1;

            switch_3_3.a = a0;
            switch_3_3.d = switch_1And2.c[2];
            val += switch_3_3.proceed();
            c[4] = switch_3_3.c0;
            c[5] = switch_3_3.c1;

            switch_3_4.a = a0;
            switch_3_4.d = switch_1And2.c[3];
            val += switch_3_4.proceed();
            c[6] = switch_3_4.c0;
            c[7] = switch_3_4.c1;

            return val;
        }
    }
}
