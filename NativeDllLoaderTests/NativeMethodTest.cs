using Microsoft.VisualStudio.TestTools.UnitTesting;
using NativeDllLoader;
using System;
using System.Runtime.InteropServices;

namespace NativeDllLoaderTests.Windows {
    [TestClass]
    public class NativeMethodTest {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint type);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int InvalidMessageBox(IntPtr hWnd, string lpText, string lpCaption);

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

            int ret = (int)method.Invoke(IntPtr.Zero, "Text", "Caption", 0u);

            Console.WriteLine(ret);
        }

        [TestMethod]
        public void InvalidMessageBoxInvokeTest() {
            NativeDll dll = new("user32.dll");

            NativeMethod<InvalidMessageBox> method = new(dll, "MessageBoxA");

            int ret = (int)method.Invoke(IntPtr.Zero, "Text", "Caption");

            Console.WriteLine(ret);
        }

         [TestMethod()]
        public void NotFoundTest() {
            Assert.ThrowsException<MissingMethodException>(() => {
                NativeDll dll = new("user32.dll");

                NativeMethod<MessageBox> method = new(dll, "MessageBoxB");
            });
        }
    }
}
