using Controller;
using Controller.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Memory
{
    [TestClass]
    public class UnitTest_Rom
    {
        [TestMethod]
        public void TestMethod1()
        {
            Rom rom = new Rom(256);
            rom.InitRegister(Tools.int2BoolArray(0), Tools.int2BoolArray(33));
            rom.InitRegister(Tools.int2BoolArray(1), Tools.int2BoolArray(44));
            
            rom.ad = Tools.int2BoolArray(0);
            rom.cl = true;
            rom.proceed();
            rom.cl = false;
            rom.proceed();
            Assert.AreEqual(33, Tools.boolArray2Int(rom.output));

            rom.ad = Tools.int2BoolArray(1);
            rom.cl = true;
            rom.proceed();
            rom.cl = false;
            rom.proceed();
            Assert.AreEqual(44, Tools.boolArray2Int(rom.output));


        }
    }
}
