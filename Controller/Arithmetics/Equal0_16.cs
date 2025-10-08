using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    public class Equal0_16
    {
        public bool[] a = new bool[16];
        public bool s;
        Equal0_2 eq1 = new Equal0_2();
        Equal0_2 eq2 = new Equal0_2();
        Equal0_2 eq3 = new Equal0_2();
        Equal0_2 eq4 = new Equal0_2();
        Equal0_2 eq5 = new Equal0_2();
        Equal0_2 eq6 = new Equal0_2();
        Equal0_2 eq7 = new Equal0_2();
        Equal0_2 eq8 = new Equal0_2();
        And and1 = new And();
        And and2 = new And();
        And and3 = new And();
        And and4 = new And();

        And and5 = new And();
        And and6 = new And();

        And and7 = new And();
        public int proceed()
        {
            int val=0;
           
            eq1.a0 = a[0];
            eq1.a1 = a[1];
            eq2.a0 = a[2];
            eq2.a1 = a[3];
            eq3.a0 = a[4];
            eq3.a1 = a[5];
            eq4.a0 = a[6];
            eq4.a1 = a[7];
            eq5.a0 = a[8];
            eq5.a1 = a[9];
            eq6.a0 = a[10];
            eq6.a1 = a[11];
            eq7.a0 = a[12];
            eq7.a1 = a[13];
            eq8.a0 = a[14];
            eq8.a1 = a[15];
            val+=eq1.proceed();
            val += eq2.proceed();
            val += eq3.proceed();
            val += eq4.proceed();
            val += eq5.proceed();
            val += eq6.proceed();
            val += eq7.proceed();
            val += eq8.proceed();
           
            and1.a = eq1.s;
            and1.b = eq2.s;
            and2.a = eq3.s;
            and2.b = eq4.s;
            and3.a = eq5.s;
            and3.b = eq6.s;
            and4.a = eq7.s;
            and4.b = eq8.s;
            val += and1.proceed();
            val += and2.proceed();
            val += and3.proceed();
            val += and4.proceed();

           

            and5.a = and1.output;
            and5.b = and2.output;
            and6.a = and3.output;
            and6.b = and4.output;
            val += and5.proceed();
            val += and6.proceed();

          
            and7.a = and5.output;
            and7.b = and6.output;
            val += and7.proceed();
            s = and7.output;
            return val;
        }
    }
}
