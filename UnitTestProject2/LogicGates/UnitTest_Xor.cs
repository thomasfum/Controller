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
    public class UnitTest_Xor
    {

        void test(bool a, bool b, bool expected)
        {
            Xor xor = new Xor();
            xor.a = a;
            xor.b = b;
            int val = xor.proceed();
            Assert.AreEqual(expected, xor.output);
            Assert.AreEqual(6, val);
        }
        [TestMethod]
        public void TestMethod_0_0()
        {
            test(false, false, false);
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
