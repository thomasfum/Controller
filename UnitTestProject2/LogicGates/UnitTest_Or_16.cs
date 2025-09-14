using Controller.LogicGates;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.LogicGates
{
    [TestClass]
    public class UnitTest_Or_16
    {

        void test(int a, int b, int expected)
        {
            Or_16 or = new Or_16();
            or.a = Tools.int2BoolArray(a);
            or.b = Tools.int2BoolArray(b);
            int val = or.proceed();
            Assert.AreEqual(expected, Tools.boolArray2Int(or.s));
            Assert.AreEqual(144, val);
        }
        [TestMethod]
        public void TestMethod_4_8()
        {
            test(4, 8, 12);
        }
        [TestMethod]
        public void TestMethod_4_4()
        {
            test(4, 4, 4);
        }
        [TestMethod]
        public void TestMethod_4_6()
        {
            test(4, 6, 6);
        }
    }
}
