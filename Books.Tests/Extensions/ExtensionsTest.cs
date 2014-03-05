using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace Books.Tests.Extensions
{
    [TestClass]
    public class ExtensionsTest
    {
        [TestMethod]
        public void TestExtensions()
        {
            string target = "1090";

            int destination = target.To<int>();
            Assert.AreEqual(1090, destination);
            
            target = null;
            var ndestination = target.ToNullable<int?>();
            Assert.AreEqual(null, ndestination);
        }
    }
}
