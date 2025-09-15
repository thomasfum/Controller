using Controller;
using Controller.Processor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Processor
{
    [TestClass]
    public class UnitTest_ALUInstruction
    {

        void test(int I, int A, int D, int CA, int expected_R, bool expected_a, bool expected_d, bool expected_ca, bool expected_j)
        {
            ALUInstruction alui = new ALUInstruction();
            alui.I = Tools.int2BoolArray(I);
            alui.A = Tools.int2BoolArray(A);
            alui.D = Tools.int2BoolArray(D);
            alui.CA = Tools.int2BoolArray(CA);
            int val=alui.proceed();

            Assert.AreEqual(6474, val);
            Assert.AreEqual(expected_R, Tools.boolArray2Int(alui.R));
            Assert.AreEqual(expected_a, alui.a);
            Assert.AreEqual(expected_d, alui.d);
            Assert.AreEqual(expected_ca, alui.ca);
            Assert.AreEqual(expected_j, alui.j);


        }
        [TestMethod]
        public void TestMethod1()//TODO explain
        {
            test(-6768, 7, 9, 13, 1, false, true, false, false);
        }
        [TestMethod]
        public void TestMethod2()//TODO explain
        {
            test(-8168, 6, 5, 0, 4, false, true, true, false);
        }
        [TestMethod]
        public void TestMethod3()//TODO explain
        {
            test(-2909, 0, 0, 42, 42, true, false, false, true);
        }
        [TestMethod]
        public void TestMethod4()//TODO explain
        {
            test(-2906, 0, 0, 42, 42, true, false, false, false);
        }

        [TestMethod]
        public void TestMethod5()//TODO explain
        {
            test(-6304, 42, 1, 2, 41, true, false, false, false);
        }
        [TestMethod]
        public void TestMethod6()//TODO explain
        {
            test(-6302, 1, 2, -1, 0, true, false, false, true);
        }
    }
}
