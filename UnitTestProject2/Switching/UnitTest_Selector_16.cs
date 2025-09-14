using Controller.LogicGates;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Controller.Switching;

namespace UnitTestProject2.Switching
{
    [TestClass]
    public class UnitTest_Selector_16
    {
        [TestMethod]
        public void TestMethod_0()
        {

            Selector_16 select = new Selector_16();
            select.a = Tools.int2BoolArray(15);
            select.b = Tools.int2BoolArray(14);
            select.s = false;
            int val = select.proceed();
            Assert.AreEqual(Tools.boolArray2Int(select.a), Tools.boolArray2Int(select.output));
            Assert.AreEqual(384, val);
        }
        [TestMethod]
        public void TestMethod_1()
        {

            Selector_16 select = new Selector_16();
            select.a = Tools.int2BoolArray(15);
            select.b = Tools.int2BoolArray(14);
            select.s = true;
            int val = select.proceed();
            Assert.AreEqual(Tools.boolArray2Int(select.b), Tools.boolArray2Int(select.output));
            Assert.AreEqual(384, val);
        }
    }
}
