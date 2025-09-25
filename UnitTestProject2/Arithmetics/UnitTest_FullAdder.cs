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
    public class UnitTest_FullAdder
    {

        void test(bool a, bool b, bool c, bool expected_h, bool expected_l)
        {
            FullAdder fullAdder = new FullAdder();
            fullAdder.a = a;
            fullAdder.b = b;
            fullAdder.c = c;
            int val=fullAdder.proceed();
            Assert.AreEqual(expected_l, fullAdder.l);
            Assert.AreEqual(expected_h, fullAdder.h);
            Assert.AreEqual(19, val);
            
        }
        [TestMethod]
        public void TestMethod_0_0_0()
        {
            test(false, false, false , false, false);
        }
        [TestMethod]
        public void TestMethod_0_0_1()
        {
            test(false,false, true, false, true);
        }
        [TestMethod]
        public void TestMethod_0_1_0()
        {
            test(false, true, false, false, true);
        }
        [TestMethod]
        public void TestMethod_0_1_1()
        {
            test(false, true, true, true, false);
        }
        [TestMethod]
        public void TestMethod_1_0_0()
        {
            test(true, false, false, false, true);
        }
        public void TestMethod_1_0_1()
        {
            test(true, false, true, true, false);
        }
        public void TestMethod_1_1_0()
        {
            test(true, true, false, true, false);
        }

        public void TestMethod_1_1_1()
        {
            test(true, true, true, true, true);
        }

    }
}
