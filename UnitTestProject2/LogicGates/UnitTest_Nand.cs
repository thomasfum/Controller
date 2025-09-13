using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2.LogicGates
{
    [TestClass]
    public class UnitTest_Nand
    {

        void test( bool a, bool b, bool expected)
        {
            Nand nand1 = new Nand();
            nand1.a = a;
            nand1.b = b;
            int val = nand1.proceed();
            Assert.AreEqual(expected, nand1.output);
            Assert.AreEqual(3, val);
        }

        [TestMethod]
        public void TestMethod_0_0()
        {
            test(false, false, true);
        }
        [TestMethod]
        public void TestMethod_0_1()
        {
            test(false, true, true);
        }
        [TestMethod]
        public void TestMethod_1_0()
        {
            test(true, false, true);
        }
        [TestMethod]
        public void TestMethod_1_1()
        {
            test(true, true, false);
        }

    }
}
