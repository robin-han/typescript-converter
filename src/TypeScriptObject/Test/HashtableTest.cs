using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeScript.CSharp.Tests
{
    [TestClass]
    public class HashtableTest : TestBase
    {
        [TestMethod]
        public void NormalTest()
        {
            Hashtable<String, Number> dic = new Hashtable<String, Number>() {
                { "ABC", 100},
                { "ABD", 200}
            };

            Assert.AreEqual(100, dic["ABC"]);
            Assert.AreEqual(200, dic["ABD"]);
        }

        [TestMethod]
        public void NullUndefinedTest()
        {
            Hashtable<String, Number> a = undefined;
            Hashtable<String, Number> b = undefined;
            Assert.IsTrue(a == b);

            a = null;
            b = undefined;
            Assert.IsTrue(a == b);
        }

        [TestMethod] 
        public void KeyTest()
        {
            Hashtable<Number, String> dic = new Hashtable<Number, String>();
            Number key = 0;
            dic[key] = "ABC";
          
            Assert.AreEqual<String>("ABC", dic[key]);
            Assert.AreEqual<String>("ABC", dic[0]);

            key++;
            Assert.AreEqual<String>("ABC", dic[0]);
        }
    }
}
