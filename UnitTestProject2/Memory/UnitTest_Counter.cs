using Controller;
using Controller.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Memory
{
    [TestClass]
    public class UnitTest_Counter
    {
        [TestMethod]
        public void TestMethod1()
        {
            Counter_16 counter= new Counter_16();
            counter.st = true;
            counter.cl = false;
            counter.X = Tools.int2BoolArray(33);
            int val= counter.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(counter.output));
            Assert.AreEqual(2400, val);


            counter.st = true;
            counter.cl = true;
            counter.X = Tools.int2BoolArray(33);
            counter.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(counter.output));

            counter.st = false;
            counter.cl = false;
            counter.X = Tools.int2BoolArray(0);
            counter.proceed();
            Assert.AreEqual(33, Tools.boolArray2Int(counter.output));

            counter.st = false;
            counter.cl = true;
            counter.X = Tools.int2BoolArray(0);
            counter.proceed();
            Assert.AreEqual(33, Tools.boolArray2Int(counter.output));

            counter.st = false;
            counter.cl = false;
            counter.X = Tools.int2BoolArray(0);
            counter.proceed();
            Assert.AreEqual(34, Tools.boolArray2Int(counter.output));

            counter.st = false;
            counter.cl = true;
            counter.X = Tools.int2BoolArray(0);
            counter.proceed();
            Assert.AreEqual(34, Tools.boolArray2Int(counter.output));

            counter.st = false;
            counter.cl = false;
            counter.X = Tools.int2BoolArray(0);
            counter.proceed();
            Assert.AreEqual(35, Tools.boolArray2Int(counter.output));
        }
    }
}
