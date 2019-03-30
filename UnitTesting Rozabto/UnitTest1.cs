using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.Model;

namespace UnitTesting_Rozabto
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_Album()
        {
            var album = new Album("Namee");
            Assert.AreEqual(album.Name, "Namee");
        }
    }
}
