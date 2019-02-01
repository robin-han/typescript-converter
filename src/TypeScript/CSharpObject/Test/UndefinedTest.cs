using GrapeCity.DataVisualization.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.DataVisualization.UnitTest.TypeScript
{
    [TestClass]
    public class UndefinedTest: TestBase
    {
        [TestMethod]
        public void NormalTest()
        {
            Undefined un = Undefined.Value;

            Assert.IsFalse(un);
            Assert.IsTrue(un == null);
            Assert.IsFalse(un != null);
            Assert.IsTrue(undefined == un);
            Assert.IsFalse(undefined != un);
        }
    }
}
