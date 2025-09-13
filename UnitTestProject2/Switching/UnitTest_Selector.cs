using Controller.Switching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Switching
{
    [TestClass]
    public class UnitTest_Selector
    {

        void test(bool s, bool d1, bool d0,bool expected)
        {
            Selector select= new Selector();
            select.s=s;
            select.d1=d1;
            select.d0=d0;
            int val=select.proceed();
            Assert.AreEqual(expected, select.s);
            Assert.AreEqual(24,val);
        }
        [TestMethod]
        public void TestMethod_0_0_0()
        {
            test(false, false, false, false);
        }
        [TestMethod]
        public void TestMethod_0_1_0()
        {
            test(false, true, false, false);
        }
        [TestMethod]
        public void TestMethod_0_0_1()
        {
            test(false, false, true, true);
        }
        [TestMethod]
        public void TestMethod_0_1_1()
        {
            test(false, true, true, true);
        }



        [TestMethod]
        public void TestMethod_1_0_0()
        {
            test(true, false, false, false);
        }
        [TestMethod]
        public void TestMethod_1_1_0()
        {
            test(true, true, false, true);
        }
        [TestMethod]
        public void TestMethod_1_0_1()
        {
            test(true, false, true, false);
        }
        [TestMethod]
        public void TestMethod_1_1_1()
        {
            test(true, true, true, true);
        }


    }
}
