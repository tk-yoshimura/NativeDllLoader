using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativeDllLoader;
using System;

namespace NativeDllLoaderTests.Windows {
    [TestClass()]
    public class NativeDllTests {
        [TestMethod()]
        public void ExistsTest() {
            Assert.IsTrue(NativeDll.Exists("user32.dll"));
            Assert.IsFalse(NativeDll.Exists("user33.dll"));
        }

        [TestMethod()]
        public void DisposeTest() {
            NativeDll dll = new("user32.dll");

            Assert.IsTrue(dll.IsValid);

            dll.Dispose();

            Assert.IsFalse(dll.IsValid);
        }

        [TestMethod()]
        public void NotFoundTest() {
            Assert.ThrowsException<DllNotFoundException>(() => {
                NativeDll dll = new("user33.dll");
            });
        }
    }
}