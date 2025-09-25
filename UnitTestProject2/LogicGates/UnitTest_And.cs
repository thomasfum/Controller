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
    public class UnitTestAnd
    {

        void test(bool a, bool b, bool expected)
        {
            And and= new And();
            and.a = a;
            and.b = b;
            int val=and.proceed();
            Assert.AreEqual(expected, and.output);
            Assert.AreEqual(2,val);
        }
        [TestMethod]
        public void TestMethod_0_0()
        {
            test(false, false, false);
        }
        [TestMethod]
        public void TestMethod_0_1()
        {
            test(false, true, false);
        }
        [TestMethod]
        public void TestMethod_1_0()
        {
            test(true, false, false);
        }
        [TestMethod]
        public void TestMethod_1_1()
        {
            test(true, true, true);
        }

    }
}
