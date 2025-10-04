using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Switching
{
    public class Switch_4_16
    {

        public bool a0;
        public bool a1;
        public bool a2;
        public bool a3;
        public bool d;
        public bool[] c = new bool[16];
        Switch_3_8 switch_1_2And3 = new Switch_3_8();
        Switch_1_2 switch_4_1 = new Switch_1_2();
        Switch_1_2 switch_4_2 = new Switch_1_2();
        Switch_1_2 switch_4_3 = new Switch_1_2();
        Switch_1_2 switch_4_4 = new Switch_1_2();

        Switch_1_2 switch_4_5 = new Switch_1_2();
        Switch_1_2 switch_4_6 = new Switch_1_2();
        Switch_1_2 switch_4_7 = new Switch_1_2();
        Switch_1_2 switch_4_8 = new Switch_1_2();

        public int proceed()
        {
            int val = 0;

           
            switch_1_2And3.a0 = a1;
            switch_1_2And3.a1 = a2;
            switch_1_2And3.a2 = a3;
            switch_1_2And3.d = d;
            val += switch_1_2And3.proceed();

            switch_4_1.a = a0;
            switch_4_1.d = switch_1_2And3.c[0];
            val += switch_4_1.proceed();
            c[0] = switch_4_1.c0;
            c[1] = switch_4_1.c1;

            switch_4_2.a = a0;
            switch_4_2.d = switch_1_2And3.c[1];
            val += switch_4_2.proceed();
            c[2] = switch_4_2.c0;
            c[3] = switch_4_2.c1;

            switch_4_3.a = a0;
            switch_4_3.d = switch_1_2And3.c[2];
            val += switch_4_3.proceed();
            c[4] = switch_4_3.c0;
            c[5] = switch_4_3.c1;

            switch_4_4.a = a0;
            switch_4_4.d = switch_1_2And3.c[3];
            val += switch_4_4.proceed();
            c[6] = switch_4_4.c0;
            c[7] = switch_4_4.c1;

            switch_4_5.a = a0;
            switch_4_5.d = switch_1_2And3.c[4];
            val += switch_4_5.proceed();
            c[8] = switch_4_5.c0;
            c[9] = switch_4_5.c1;

            switch_4_6.a = a0;
            switch_4_6.d = switch_1_2And3.c[5];
            val += switch_4_6.proceed();
            c[10] = switch_4_6.c0;
            c[11] = switch_4_6.c1;

            switch_4_7.a = a0;
            switch_4_7.d = switch_1_2And3.c[6];
            val += switch_4_7.proceed();
            c[12] = switch_4_7.c0;
            c[13] = switch_4_7.c1;

            switch_4_8.a = a0;
            switch_4_8.d = switch_1_2And3.c[7];
            val += switch_4_8.proceed();
            c[14] = switch_4_8.c0;
            c[15] = switch_4_8.c1;

            return val;
        }
    }
}
