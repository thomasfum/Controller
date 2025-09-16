using Controller.Memory;
using Controller;
using Controller.Processor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Processor
{
    [TestClass]
    public class UnitTest_Computer
    {
        [TestMethod]
        public void TestMethod_Dincrement()
        {
            Computer myComputer;
            myComputer = new Computer(256, 256);
            myComputer.setRom(0, 32912);//Set the D register to 0
            myComputer.setRom(1, 2);    //Set the A register to 2
            myComputer.setRom(2, 34064);// Add 1 to the D register
            myComputer.setRom(3, 32775);//Jump unconditionally to adress A =>2
            int val=myComputer.proceed();
            Assert.AreEqual(0, myComputer.getD());
            Assert.AreEqual( 13098, val);
            myComputer.proceed();
            Assert.AreEqual(0, myComputer.getD());
            myComputer.proceed();
            Assert.AreEqual(0, myComputer.getD());
            myComputer.proceed();
            Assert.AreEqual(1, myComputer.getD());
            myComputer.proceed();
            Assert.AreEqual(1, myComputer.getD());
            myComputer.proceed();
            Assert.AreEqual(2, myComputer.getD());
            myComputer.proceed();
            Assert.AreEqual(2, myComputer.getD());
            myComputer.proceed();
            Assert.AreEqual(3, myComputer.getD());

        }
    }
}
