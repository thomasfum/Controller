using Controller.LogicGates;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.LogicGates
{
    [TestClass]
    public class UnitTest_Xor_16
    {
        void test(int a, int b, int expected)
        {
            Xor_16 xor = new Xor_16();
            xor.a = Tools.int2BoolArray(a);
            xor.b = Tools.int2BoolArray(b);
            int val = xor.proceed();
            Assert.AreEqual(expected, Tools.boolArray2Int(xor.s));
            Assert.AreEqual(96, val);
        }
        [TestMethod]
        public void TestMethod_4_8()
        {
            test(4, 8, 12);
        }
        [TestMethod]
        public void TestMethod_4_4()
        {
            test(4, 4, 0);
        }
        [TestMethod]
        public void TestMethod_4_6()
        {
            test(4, 6, 2);
        }
    }
}
