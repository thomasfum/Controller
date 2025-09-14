using Controller.Arithmetics;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Controller.LogicGates;

namespace UnitTestProject2.LogicGates
{
    [TestClass]
    public class UnitTest_And_16
    {
        void test(int a, int b, int expected)
        {
            And_16 and = new And_16();
            and.a = Tools.int2BoolArray(a);
            and.b = Tools.int2BoolArray(b);
            int val = and.proceed();
            Assert.AreEqual(expected, Tools.boolArray2Int(and.s));
            Assert.AreEqual(96, val);
        }
        [TestMethod]
        public void TestMethod_4_8()
        {
            test(4, 8, 0);
        }
        [TestMethod]
        public void TestMethod_4_4()
        {
            test(4, 4, 4);
        }
        [TestMethod]
        public void TestMethod_4_6()
        {
            test(4, 6, 4);
        }
    }
}
