using GrapeCity.DataVisualization.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.DataVisualization.UnitTest.TypeScript
{
    [TestClass]
    public class DynamicObjectTest : TestBase
    {
        [TestMethod]
        public void NormalTest()
        {
            dynamic obj = new DynamicObject() {
                { "ABC", 100},
                { "ABD", 200}
            };

            Assert.AreEqual(100, obj["ABC"]);
            Assert.AreEqual(200, obj.ABD);
            Assert.AreEqual(undefined, obj.DD);

            Assert.IsTrue(obj);
        }

    }
}
