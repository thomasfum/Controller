using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2.Arithmetics
{
    [TestClass]
    public class UnitTest_Adder_2
    {

        [TestMethod]
        public void TestMethod_2_2_1()
        {
            Adder_2 twoBitsAdder = new Adder_2();
            twoBitsAdder.a0 = false;
            twoBitsAdder.a1 = true;//a=2
            twoBitsAdder.b0 = false;
            twoBitsAdder.b1 = true;//b=2
            twoBitsAdder.c = true;//c=1
            int val=twoBitsAdder.proceed();//=>5
            Assert.AreEqual(true, twoBitsAdder.cOut);
            Assert.AreEqual(false, twoBitsAdder.s1);
            Assert.AreEqual(true, twoBitsAdder.s0);
            Assert.AreEqual(114, val);

        }
        [TestMethod]
        public void TestMethod_1_1_0()
        {
            Adder_2 twoBitsAdder = new Adder_2();
            twoBitsAdder.a0 = true;
            twoBitsAdder.a1 = false;//a=1
            twoBitsAdder.b0 = true;
            twoBitsAdder.b1 = false;//b=1
            twoBitsAdder.c = false;//c=0
            int val = twoBitsAdder.proceed();//=>2
            Assert.AreEqual(false, twoBitsAdder.cOut);
            Assert.AreEqual(true, twoBitsAdder.s1);
            Assert.AreEqual(false, twoBitsAdder.s0);
            Assert.AreEqual(114, val);

        }

    }
}
