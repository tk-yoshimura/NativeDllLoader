using System;
using System.Runtime.InteropServices;

namespace NativeDllLoader {
    static class WindowsKernel {
        private const string dllname = "kernel32.dll";

        [DllImport(dllname, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string libname);

        [DllImport(dllname, SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(dllname, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = false, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
    }
}
