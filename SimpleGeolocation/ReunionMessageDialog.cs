using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using Windows.UI.Popups;
using WinRT;

namespace SimpleGeolocation
{
    public static class ReunionMessageDialog 
    {

        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }
        public static void Initialize(this MessageDialog md, IntPtr hWndArg)
        {
            IInitializeWithWindow initializeWithWindowWrapper = md.As<IInitializeWithWindow>();
            IntPtr hwnd;
            if (hWndArg == IntPtr.Zero)
                hwnd = PInvoke.User32.GetActiveWindow();
            else
                hwnd = hWndArg;

            initializeWithWindowWrapper.Initialize(hwnd);
        }
        public static void Initialize(this MessageDialog md, Window window=null)
        {
            IInitializeWithWindow initializeWithWindowWrapper = md.As<IInitializeWithWindow>();
            IntPtr hwnd;
            if (window == null)
                hwnd = PInvoke.User32.GetActiveWindow();
            else
                hwnd = window.As<IWindowNative>().WindowHandle; ;
            initializeWithWindowWrapper.Initialize(hwnd);
        }
    }
}
