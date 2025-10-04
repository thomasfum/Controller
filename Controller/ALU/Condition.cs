using Controller.Arithmetics;
using Controller.LogicGates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.ALU
{
    public class Condition
    {

        public bool lt;
        public bool eq;
        public bool gt;
        public bool[] X = new bool[16];
        public bool output;

        Equal0_16 eq0 = new Equal0_16();
        Less0_16 lt0 = new Less0_16();
        Invert invEq0 = new Invert();
        Invert invLt0 = new Invert();
        And gtAnd1 = new And();
        And gtAnd2 = new And();
        And ltAnd = new And();
        And eqAnd = new And();
        Or Or1 = new Or();
        Or Or2 = new Or();

        public int proceed()
        {
            int val = 0;
        
            eq0.a = X;
            val+=eq0.proceed();
            lt0.a = X;
            val += lt0.proceed();

            invEq0.input= eq0.s;
            val += invEq0.proceed();

            invLt0.input = lt0.s;
            val += invLt0.proceed();

            gtAnd1.a = invEq0.output;
            gtAnd1.b = invLt0.output;
            val += gtAnd1.proceed();

            gtAnd2.a = gtAnd1.output;
            gtAnd2.b = gt;
            val += gtAnd2.proceed();

            ltAnd.a= lt0.s;
            ltAnd.b = lt;
            val += ltAnd.proceed();

            eqAnd.a= eq0.s;
            eqAnd.b = eq;
            val += eqAnd.proceed();

            Or1.a = gtAnd2.output;
            Or1.b= ltAnd.output;
            val += Or1.proceed();

            Or2.a = Or1.output;
            Or2.b = eqAnd.output;
            val += Or2.proceed();

            output = Or2.output;
            return val;
        }
    }
}
