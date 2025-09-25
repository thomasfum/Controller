using Controller;
using Controller.Arithmetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2.Arithmetics
{
    [TestClass]
    public class UnitTest_HalfAdder
    {
        void test(bool a, bool b, bool expected_h, bool expected_l)
        {
            HalfAdder halfAdder = new HalfAdder();
            halfAdder.a = a;
            halfAdder.b = b;
            int val=halfAdder.proceed();
            Assert.AreEqual(expected_l, halfAdder.l);
            Assert.AreEqual(expected_h, halfAdder.h);
            Assert.AreEqual(8, val);
        }
        [TestMethod]
        public void TestMethod_0_0()
        {
            test(false, false, false,false);
        }
        [TestMethod]
        public void TestMethod_0_1()
        {
            test(false, true, false,true);
        }
        [TestMethod]
        public void TestMethod_1_0()
        {
            test(true, false, false, true);
        }
        [TestMethod]
        public void TestMethod_1_1()
        {
            test(true, true, true, false);
        }
    }
}
