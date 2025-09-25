using Controller;
using Controller.Switching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Switching
{
    [TestClass]
    public class UnitTest_Switch_2_4
    {

        void test(bool a1, bool a0, int expected_d)
        {
            Switch_2_4 demux = new Switch_2_4();
            demux.a0 = a0;
            demux.a1 = a1;
            demux.d = true;
            int val = demux.proceed();
            Assert.AreEqual(expected_d, Tools.boolArray2Int(demux.c));
            Assert.AreEqual(15, val);
        }
        [TestMethod]
        public void TestMethod1_0_0_1()
        {
            test(false, false, 1);
        }
        [TestMethod]
        public void TestMethod1_0_1_2()
        {
            test(false, true, 2);
        }
        [TestMethod]
        public void TestMethod_1_0_3()
        {
            test(true, false, 4);
        }
        [TestMethod]
        public void TestMethod_1_1_8()
        {
            test(true, true, 8);
        }
    }
}
