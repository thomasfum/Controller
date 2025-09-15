using Controller.Processor;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Processor
{
    [TestClass]
    public class UnitTest_ControlSelector
    {

        void test(bool s, int R1, bool a1, bool d1, bool ca1, bool j1, int R0, bool a0, bool d0, bool ca0, bool j0, int expected_R, bool expected_a, bool expected_d, bool expected_ca, bool expected_j)
        {
            ControlSelector cs = new ControlSelector();
            cs.s = s;
            cs.R1 = Tools.int2BoolArray(R1);
            cs.a1 = a1;
            cs.d1 = d1;
            cs.ca1 = ca1;
            cs.j1 = j1;
            cs.R0 = Tools.int2BoolArray(R0);
            cs.a0 = a0;
            cs.d0 = d0;
            cs.ca0 = ca0;
            cs.j0 = j0;
            int val = cs.proceed();

            Assert.AreEqual(480, val);
            Assert.AreEqual(expected_R, Tools.boolArray2Int(cs.R));
            Assert.AreEqual(expected_a, cs.a);
            Assert.AreEqual(expected_d, cs.d);
            Assert.AreEqual(expected_ca, cs.ca);
            Assert.AreEqual(expected_j, cs.j);

        }
        [TestMethod]
        public void TestMethod1()
        {
            test(false, 42, false, true, false, true, -256, true, false, true, false, -256, true, false, true, false);
        }
        public void TestMethod2()
        {
            test(true, 42, false, true, false, true, -256, true, false, true, false, 42, false, true, false, true);
        }
    }
}
