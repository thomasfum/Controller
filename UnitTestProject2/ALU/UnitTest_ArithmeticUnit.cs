using Controller;
using Controller.ALU;
using Controller.Arithmetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.ALU
{
    [TestClass]
    public class UnitTest_ArithmeticUnit
    {

        void test(bool op1, bool op0, int X, int Y, int expected)
        {
            ArithmeticUnit au = new ArithmeticUnit();
            au.op0 = op0;
            au.op1 = op1;
            au.X = Tools.int2BoolArray(X);
            au.Y = Tools.int2BoolArray(Y);
            int val = au.proceed();
            Assert.AreEqual(expected, Tools.boolArray2Int(au.output));
            Assert.AreEqual(2640, val);
        }
        [TestMethod]
        public void TestMethod_add_5_8()
        {
            test(false, false, 5, 8, 13);

        }
        [TestMethod]
        public void TestMethod_sub_8_5()
        {
            test(true, false, 8, 5, 3);
        }
        [TestMethod]
        public void TestMethod_5_inc()
        {
            test(false, true, 5, 0, 6);
        }
        [TestMethod]
        public void TestMethod_5_dec()
        {
            test(true, true, 5, 0, 4);
        }
    }
}
