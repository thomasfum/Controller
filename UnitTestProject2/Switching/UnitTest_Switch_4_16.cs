using Controller.Switching;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Switching
{
    [TestClass]
    public class UnitTest_Switch_4_16
    {

        void test(bool a3, bool a2, bool a1, bool a0, int expected_d)
        {
            Switch_4_16 demux = new Switch_4_16();
            demux.a0 = a0;
            demux.a1 = a1;
            demux.a2 = a2;
            demux.a3 = a3;
            demux.d = true;
            int val = demux.proceed();
            Assert.AreEqual(expected_d, Tools.boolArray2Int(demux.c));
            Assert.AreEqual(75, val);
        }

        [TestMethod]
        public void TestMethod_0_1()
        {
            test(false,false, false, false, 1);
        }
        [TestMethod]
        public void TestMethod_1_2()
        {
            test(false, false, false, true, 2);
        }
        [TestMethod]
        public void TestMethod_2_4()
        {
            test(false, false, true, false, 4);
        }
        [TestMethod]
        public void TestMethod_3_8()
        {
            test(false, false, true, true, 8);
        }
        [TestMethod]
        public void TestMethod_4_16()
        {
            test(false, true, false, false, 16);
        }
        [TestMethod]
        public void TestMethod_5_32()
        {
            test(false, true, false, true, 32);
        }
        [TestMethod]
        public void TestMethod_6_64()
        {
            test(false, true, true, false, 64);
        }
        [TestMethod]
        public void TestMethod_7_128()
        {
            test(false, true, true, true, 128);
        }
        [TestMethod]
        public void TestMethod_8_256()
        {
            test(true, false, false, false, 256);
        }
        [TestMethod]
        public void TestMethod_9_512()
        {
            test(true, false, false, true, 512);
        }
        [TestMethod]
        public void TestMethod_10_1024()
        {
            test(true, false, true, false, 1024);
        }
        [TestMethod]
        public void TestMethod_11_2048()
        {
            test(true, false, true, true, 2048);
        }
        [TestMethod]
        public void TestMethod_12_4096()
        {
            test(true, true, false, false, 4096);
        }
        [TestMethod]
        public void TestMethod_13_8192()
        {
            test(true, true, false, true, 8192);
        }
        [TestMethod]
        public void TestMethod_14_16384()
        {
            test(true, true, true, false, 16384);
        }
        [TestMethod]
        public void TestMethod_15_32768()
        {
            test(true, true, true, true, 32768);
        }
    }
}
