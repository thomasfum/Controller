using Controller;
using Controller.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Memory
{
    [TestClass]
    public class UnitTest_Register_16
    {
        [TestMethod]
        public void TestMethod1()
        {
            Register_16 register_16 = new Register_16();
            register_16.X = Tools.int2BoolArray(33);
            register_16.st = false;
            register_16.cl = false;
            int val = register_16.proceed();
            Assert.AreEqual(0,Tools. boolArray2Int(register_16.output));
            Assert.AreEqual(368, val);

            register_16.X = Tools.int2BoolArray(33);
            register_16.st = true;
            register_16.cl = false;
            register_16.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(register_16.output));


            register_16.X = Tools.int2BoolArray(33);
            register_16.st = true;
            register_16.cl = true;
            register_16.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(register_16.output));

            register_16.X = Tools.int2BoolArray(33);
            register_16.st = false;
            register_16.cl = false;
            register_16.proceed();
            Assert.AreEqual(33, Tools.boolArray2Int(register_16.output));

            register_16.X = Tools.int2BoolArray(22);
            register_16.st = true;
            register_16.cl = true;
            register_16.proceed();
            Assert.AreEqual(33, Tools.boolArray2Int(register_16.output));


            register_16.X = Tools.int2BoolArray(11);
            register_16.st = false;
            register_16.cl = false;
            register_16.proceed();
            Assert.AreEqual(22, Tools.boolArray2Int(register_16.output));



        }
    }
}
