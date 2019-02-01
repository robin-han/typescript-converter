using GrapeCity.DataVisualization.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace GrapeCity.DataVisualization.UnitTest.TypeScript
{
    [TestClass]
    public class ArrayTest : TestBase
    {
        [TestMethod]
        public void NormalTest()
        {
            Array<Number> arr = new Array<Number>() { 0, 1, 2, 3 };
            for (Number i = 0; i < 4; i++)
            {
                Assert.AreEqual(i, arr[i]);
            }
        }

        [TestMethod]
        public void NullUndefinedTest()
        {
            Array<String> a = undefined;
            Array<String> b = undefined;
            Assert.IsTrue(a == b);

            a = null;
            b = undefined;
            Assert.IsTrue(a == b);

            a = undefined;
            b = null;
            Assert.IsTrue(a == b);

            a = null;
            b = null;
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void ConcatTest()
        {
            Array<Number> newArray;
            Array<Number> arr = new Array<Number>() { 1, 2 };
            newArray = arr.concat();
            Assert.AreEqual(1, newArray[0]);
            Assert.AreEqual(2, newArray[1]);

            newArray = arr.concat(3, 4);
            Assert.AreEqual(3, newArray[2]);
            Assert.AreEqual(4, newArray[3]);

            Array<Number> arr2 = new Array<Number>() { 5, 6 };
            newArray = arr.concat(arr2);
            Assert.AreEqual(5, newArray[2]);
            Assert.AreEqual(6, newArray[3]);
        }

        [TestMethod]
        public void EveryTest()
        {
            Array<Number> arr = new Array<Number>() { 1, 2, 3 };
            Assert.IsTrue(arr.every((item) => item > 0));
        }

        [TestMethod]
        public void FilterTest()
        {
            Array<Number> arr = new Array<Number>() { 1, 2, 3 };
            Array<Number> items = arr.filter(item => item > 2);
            Assert.IsTrue(items.length == 1);
            Assert.AreEqual(3, items[0]);
        }

        [TestMethod]
        public void FindTest()
        {
            Array<Number> arr = new Array<Number>() { 1, 2, 3 };

            Number find = arr.find(item => item > 2);
            Assert.AreEqual(3, find);

            find = arr.find(item => item > 3);
            Assert.AreEqual(null, find);
        }
    }
}
