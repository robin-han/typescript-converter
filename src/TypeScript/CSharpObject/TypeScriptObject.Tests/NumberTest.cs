﻿using GrapeCity.DataVisualization.TypeScript;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GrapeCity.DataVisualization.UnitTest.TypeScript
{
    [TestClass]
    public class NumberTest : TestBase
    {
        [TestMethod]
        public void NormalTest()
        {
            Number a = 10;
            Number b = 20;

            Assert.AreEqual<Number>(30, a + b);
            Assert.AreEqual<Number>(-10, a - b);
            Assert.AreEqual<Number>(200, a * b);
            Assert.AreEqual<Number>(0.5, a / b);

            Assert.IsTrue(10 == a);
            Assert.IsTrue(b == 20);
            Assert.IsFalse(10 != a);
            Assert.IsFalse(b != 20);
        }

        [TestMethod]
        public void NullUndefinedTest()
        {
            Number a = undefined;
            Number b = undefined;
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

            a = null;
            b = 0;
            Assert.IsFalse(a == b);

            a = 0;
            b = null;
            Assert.IsFalse(a == b);

            a = undefined;
            b = 0;
            Assert.IsFalse(a == b);

            a = 0;
            b = undefined;
            Assert.IsFalse(a == b);

            a = NaN;
            b = NaN;
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void BooleanTest()
        {
            Number a = undefined;
            Assert.IsFalse(a);

            a = null;
            Assert.IsFalse(a);

            a = 0;
            Assert.IsFalse(a);

            a = 1;
            Assert.IsTrue(a);

            a = -1;
            Assert.IsTrue(a);
        }
    }
}
