using Controller;
using Controller.ALU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.ALU
{
    [TestClass]
    public class UnitTest_Conition
    {
        void test(bool lt, bool eq, bool gt, int X, bool expected)
        {
            Condition Cond = new Condition();
            Cond.lt = lt;
            Cond.eq = eq;
            Cond.gt = gt;
            Cond.X = Tools.int2BoolArray(X);
            int val = Cond.proceed();
            Assert.AreEqual(expected, Cond.output);
            Assert.AreEqual(186, val);
        }
        [TestMethod]
        public void TestMethod_Never0()
        {
            test(false,false, false, 0,false);
        }
        [TestMethod]
        public void TestMethod_Never1()
        {
            test(false, false, false, 1, false);
        }
        [TestMethod]
        public void TestMethod_Never2()
        {
            test(false, false, false, 2, false);
        }
        [TestMethod]
        public void TestMethod_neverM1()
        {
            test(false, false, false, -1, false);
        }
        [TestMethod]
        public void TestMethod_Always0()
        {
            test(true, true, true, 0, true);
        }
        [TestMethod]
        public void TestMethod_Always1()
        {
            test(true, true, true, 1, true);
        }
        [TestMethod]
        public void TestMethod_Always2()
        {
            test(true, true, true, 2, true);
        }
        [TestMethod]
        public void TestMethod_AlwaysM1()
        {
            test(true, true, true, -1, true);
        }

        [TestMethod]
        public void TestMethod_gt0()
        {
            test(false, false, true, 0, false);
        }

        [TestMethod]
        public void TestMethod_gt1()
        {
            test(false, false, true, 1, true);
        }



        [TestMethod]
        public void TestMethod_lt0()
        {
            test(true, false, false, 0, false);
        }

        [TestMethod]
        public void TestMethod_lt1()
        {
            test(true, false, false, -1, true);
        }

        //----------------------
        [TestMethod]
        public void TestMethod_gt_eq0()
        {
            test(false, true, true, 0, true);
        }

        [TestMethod]
        public void TestMethod_gt_eq1()
        {
            test(false, true, true, 1, true);
        }


        [TestMethod]
        public void TestMethod_lt_eq0()
        {
            test(true, true, false, 0, true);
        }

        [TestMethod]
        public void TestMethod_lt_eq1()
        {
            test(true, true, false, -1, true);
        }

        [TestMethod]
        public void TestMethod_eq0()
        {
            test(false, true, false, 0, true);
        }
        [TestMethod]
        public void TestMethod_eq1()
        {
            test(false, true, false, 1, false);
        }

    }
}
