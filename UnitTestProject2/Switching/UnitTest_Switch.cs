using Controller.Switching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Switching
{
    [TestClass]
    public class UnitTest_Switch
    {


        void test(bool s, bool d,  bool expected_c1, bool expected_c0)
        {
            Switch switc = new Switch();
            switc.s = s;
            switc.d = d;
            int val = switc.proceed();
            Assert.AreEqual(expected_c0, switc.c0);
            Assert.AreEqual(expected_c1, switc.c1);
            Assert.AreEqual(15, val);
        }
        [TestMethod]
        public void TestMethod_0_0()
        {
            test(false, false, false, false);
        }
        [TestMethod]
        public void TestMethod_0_1()
        {
            test(false, true, false, true);
        }
        [TestMethod]
        public void TestMethod_1_0()
        {
            test(true, false, false, false);
        }
        [TestMethod]
        public void TestMethod_1_1()
        {
            test(true, true, true, false);
        }
    }
}
