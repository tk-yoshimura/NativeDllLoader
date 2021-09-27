using System;
using System.Runtime.InteropServices;

namespace NativeDllLoader {
    public class NativeMethod<Delegate> where Delegate : System.Delegate {
        private readonly Delegate function;

        internal NativeDll Assembly { private set; get; }
        internal IntPtr Handle { private set; get; } = IntPtr.Zero;

        public NativeMethod(NativeDll assembly, string funcname) {
            if (!assembly.IsValid) {
                throw new ArgumentException(
                    "The specified assembly has not been loaded.",
                    nameof(assembly)
                );
            }

            this.Assembly = assembly;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                throw new NotImplementedException();
            }

            this.Handle = WindowsKernel.GetProcAddress(assembly.Handle, funcname);

            if (this.Handle == IntPtr.Zero) {
                throw new MissingMethodException($"{assembly.Name} - {funcname}");
            }

            this.function = Marshal.GetDelegateForFunctionPointer<Delegate>(Handle);
        }

        public bool IsValid => Assembly.IsValid && Handle != IntPtr.Zero;

        public static bool Exists(NativeDll dll, string funcname) {
            if (!dll.IsValid) {
                throw new ArgumentException(
                    "The specified assembly has not been loaded.",
                    nameof(dll)
                );
            }

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                throw new NotImplementedException();
            }

            IntPtr handle = WindowsKernel.GetProcAddress(dll.Handle, funcname);

            return handle != IntPtr.Zero;
        }

        public object Invoke(params object[] args) {
            if (!IsValid) {
                throw new InvalidOperationException();
            }

            return function.DynamicInvoke(args.Length > 0 ? args : null);
        }
    }
}
