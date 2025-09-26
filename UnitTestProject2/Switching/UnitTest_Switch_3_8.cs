using Controller.Switching;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Switching
{
    [TestClass]
    public class UnitTest_Switch_3_8
    {

        void test(bool a2, bool a1, bool a0, int expected_d)
        {
            Switch_3_8 demux = new Switch_3_8();
            demux.a0 = a0;
            demux.a1 = a1;
            demux.a2 = a2;
            demux.d = true;
            int val = demux.proceed();
            Assert.AreEqual(expected_d, Tools.boolArray2Int(demux.c));
            Assert.AreEqual(35, val);
        }

        [TestMethod]
        public void TestMethod_0_0_0_1()
        {
            test(false, false, false, 1);
        }
        [TestMethod]
        public void TestMethod_0_0_1_2()
        {
            test(false, false, true, 2);
        }

        [TestMethod]
        public void TestMethod_0_1_0_4()
        {
            test(false, true, false, 4);
        }
        [TestMethod]
        public void TestMethod_0_1_1_8()
        {
            test(false, true, true, 8);
        }
        [TestMethod]
        public void TestMethod_1_0_0_16()
        {
            test(true, false, false, 16);
        }
        [TestMethod]
        public void TestMethod_1_0_1_32()
        {
            test(true, false, true, 32);
        }
        [TestMethod]
        public void TestMethod_1_1_0_64()
        {
            test(true, true, false, 64);
        }
        
        [TestMethod]
        public void TestMethod_1_1_1_128()
        {
            test(true, true, true, 128);
        }
    }
}
