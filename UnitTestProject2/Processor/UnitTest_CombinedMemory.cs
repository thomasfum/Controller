using Controller.Memory;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Controller.Processor;

namespace UnitTestProject2.Processor
{
    [TestClass]
    public class UnitTest_CombinedMemory
    {
        [TestMethod]
        public void TestMethod1()
        {
            CombinedMemory mem = new CombinedMemory(64);

            mem.X = Tools.int2BoolArray(5);
            mem.a = true;
            mem.d = true;
            mem.ca = false;
            mem.cl = false;
            int val = mem.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(mem.A));
            Assert.AreEqual(0, Tools.boolArray2Int(mem.D));
            Assert.AreEqual(0, Tools.boolArray2Int(mem.CA));
            Assert.AreEqual(25561, val);


            mem.X = Tools.int2BoolArray(5);
            mem.a = true;
            mem.d = true;
            mem.ca = false;
            mem.cl = true;
            mem.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(mem.A));
            Assert.AreEqual(0, Tools.boolArray2Int(mem.D));
            Assert.AreEqual(0, Tools.boolArray2Int(mem.CA));

            mem.X = Tools.int2BoolArray(5);
            mem.a = true;
            mem.d = true;
            mem.ca = false;
            mem.cl = false;
            mem.proceed();
                Assert.AreEqual(5, Tools.boolArray2Int(mem.A));
                Assert.AreEqual(5, Tools.boolArray2Int(mem.D));
            Assert.AreEqual(0, Tools.boolArray2Int(mem.CA));

            mem.X = Tools.int2BoolArray(55);
            mem.a = false;
            mem.d = false;
            mem.ca = true;
            mem.cl = true;
            mem.proceed();
            Assert.AreEqual(5, Tools.boolArray2Int(mem.A));
            Assert.AreEqual(5, Tools.boolArray2Int(mem.D));
            Assert.AreEqual(0, Tools.boolArray2Int(mem.CA));


            mem.X = Tools.int2BoolArray(0);
            mem.a = false;
            mem.d = false;
            mem.ca = true;
            mem.cl = false;
            mem.proceed();
            Assert.AreEqual(5, Tools.boolArray2Int(mem.A));
            Assert.AreEqual(5, Tools.boolArray2Int(mem.D));
            Assert.AreEqual(55, Tools.boolArray2Int(mem.CA));

        }


    }
}
