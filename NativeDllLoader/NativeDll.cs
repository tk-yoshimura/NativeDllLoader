using System;
using System.Runtime.InteropServices;

namespace NativeDllLoader {
    public class NativeDll : IDisposable {
        internal IntPtr Handle { private set; get; } = IntPtr.Zero;

        public string Name { private set; get; } = string.Empty;

        public NativeDll(string dllname) {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                throw new NotImplementedException();
            }

            dllname = dllname.Replace('/', '\\');

            this.Handle = WindowsKernel.LoadLibrary(dllname);

            if (!IsValid) {
                throw new DllNotFoundException(dllname);
            }

            this.Name = dllname;
        }

        public bool IsValid => Handle != IntPtr.Zero;

        public static bool Exists(string libname) {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                throw new NotImplementedException();
            }

            IntPtr handle = WindowsKernel.LoadLibrary(libname);

            if (handle == IntPtr.Zero) {
                return false;
            }

            WindowsKernel.FreeLibrary(handle);

            return true;
        }

        public void Dispose() {
            if (Handle != IntPtr.Zero) {
                WindowsKernel.FreeLibrary(Handle);
                Handle = IntPtr.Zero;
                Name = string.Empty;
            }

            GC.SuppressFinalize(this);
        }

        ~NativeDll() {
            Dispose();
        }
    }
}
