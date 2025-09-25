using Controller.Processor;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Processor
{
    [TestClass]
    public class UnitTest_ControlUnit
    {

        void test(int I, int A, int D, int CA,  int expected_R, bool expected_a, bool expected_d, bool expected_ca, bool expected_j)
        {
            ControlUnit cu = new ControlUnit();
         
            cu.I = Tools.int2BoolArray(I);
            cu.A = Tools.int2BoolArray(A);
            cu.D = Tools.int2BoolArray(D);
            cu.CA = Tools.int2BoolArray(CA);
            int val = cu.proceed();

            Assert.AreEqual(2318, val);
            Assert.AreEqual(expected_R, Tools.boolArray2Int(cu.R));
            Assert.AreEqual(expected_a, cu.a);
            Assert.AreEqual(expected_d, cu.d);
            Assert.AreEqual(expected_ca, cu.ca);
            Assert.AreEqual(expected_j, cu.j);

        }
        [TestMethod]
        public void TestMethod1()//TODO explain
        {
            test(1234, 7, 9, 13, 1234, true, false, false, false);
        }
        [TestMethod]
        public void TestMethod2()//TODO explain
        {
            test(-6768, 7, 9, 13, 1, false, true, false, false);
        }
        [TestMethod]
        public void TestMethod3()//TODO explain
        {
            test(-8168, 6, 5, 0, 4, false, true, true, false);
        }
        [TestMethod]
        public void TestMethod4()//TODO explain
        {
            test(-2909, 0, 0, 42, 42, true, false, false, true);
        }
        [TestMethod]
        public void TestMethod5()//TODO explain
        {
            test(-6304, 42, 1, 2, 41, true, false, false, false);
        }
    }
}
