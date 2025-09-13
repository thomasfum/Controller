using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Controller;

namespace UnitTestProject2.LogicGates
{
    [TestClass]
    public class UnitTest_Relay
    {

        void test(bool _default, bool cmd, bool _input,bool expected)
        {
            Relay relay1 = new Relay(_default);//default off
            relay1.input  = _input;
            relay1.c = cmd;
            int val=relay1.proceed();
            Assert.AreEqual(expected, relay1.output);
            Assert.AreEqual(1,val);
        }
        //defaultOff
        [TestMethod]
        public void TestMethod_defaultOff_OnTrue()
        {
            test(true,true,true,true);
        }
        [TestMethod]
        public void TestMethod_defaultOff_OffTrue()
        {
            test(true,  false, true, false);
        }
        [TestMethod]
        public void TestMethod_defaultOff_OnFalse()
        {
            test(true, true, false, false);
        }
        [TestMethod]
        public void TestMethode_defaultOff_OffFalse()
        {
            test(true, false, false, false);
        }
        //defaultOn
        [TestMethod]
        public void TestMethod_defaultOn_OnTrue()
        {
            test(false, true, true, false);
        }
        [TestMethod]
        public void TestMethod_defaultOn_OffTrue()
        {
            test(false, false, true, true);
        }
        [TestMethod]
        public void TestMethod_defaultOn_OnFalse()
        {
            test(false, true, false, false);
        }
        [TestMethod]
        public void TestMethode_defaultOn_OffFalse()
        {
            test(false, false, false, false);
        }

    }
}
