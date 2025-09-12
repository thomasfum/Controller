using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class UnitTestOr
    {

        void test(bool a, bool b, bool expected)
        {
            Or or = new Or();
            or.a = a;
            or.b = b;
            or.proceed();
            Assert.AreEqual(expected, or.output);
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
            test(true, true, true);
        }

    }
}
