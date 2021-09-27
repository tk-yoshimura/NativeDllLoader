using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativeDllLoader;
using System;
using System.Runtime.InteropServices;

namespace NativeDllLoaderTests.Windows {
    [TestClass]
    public class NativeMethodTest {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint type);

        [TestMethod()]
        public void ExistsTest() {
            NativeDll dll = new("user32.dll");

            Assert.IsTrue(NativeMethod<MessageBox>.Exists(dll, "MessageBoxA"));
            Assert.IsFalse(NativeMethod<MessageBox>.Exists(dll, "MessageBoxB"));
        }

        [TestMethod]
        public void MessageBoxInvokeTest() {
            NativeDll dll = new("user32.dll");

            NativeMethod<MessageBox> method = new(dll, "MessageBoxA");

            int ret = (int)method.AsDelegate().Invoke(IntPtr.Zero, "Text", "Caption", 0u);

            Console.WriteLine(ret);
        }

        [TestMethod()]
        public void NotFoundTest() {
            Assert.ThrowsException<EntryPointNotFoundException>(() => {
                NativeDll dll = new("user32.dll");

                NativeMethod<MessageBox> method = new(dll, "MessageBoxB");
            });
        }

        [TestMethod]
        public void DisposedTest() {
            NativeDll dll = new("user32.dll");

            NativeMethod<MessageBox> method = new(dll, "MessageBoxA");

            dll.Dispose();

            Assert.ThrowsException<InvalidOperationException>(() => {
                method.AsDelegate().Invoke(IntPtr.Zero, "Text", "Caption", 0u);
            });
        }
    }
}
