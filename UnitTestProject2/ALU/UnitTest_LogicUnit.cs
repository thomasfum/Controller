using Controller.Switching;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Controller.ALU;

namespace UnitTestProject2.ALU
{
    [TestClass]
    public class UnitTest_LogicUnit
    {

        void test(bool op1, bool op0,int X, int Y, int expected)
        {
            LogicUnit lu = new LogicUnit();
            lu.X = Tools.int2BoolArray(X);
            lu.Y = Tools.int2BoolArray(Y);
            lu.op0 = op0;
            lu.op1 = op1;
            int val = lu.proceed();
            Assert.AreEqual(expected, Tools.boolArray2Int(lu.output));
            Assert.AreEqual(576, val);
        }
        [TestMethod]
        public void TestMethod_And()
        {
            test(false, false, 15, 14, 14);
        }

        [TestMethod]
        public void TestMethod_Or()
        {
            test(false, true, 15, 14, 15);
        }
        [TestMethod]
        public void TestMethod_Xor()
        {
            test(true, false, 7, 2, 5);
        }

        [TestMethod]
        public void TestMethod_Inver()
        {
            test(true, true, 2, 0, 65536-3);
        }
    }
}
