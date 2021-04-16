using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClassLibrary1
{
    public class Class1
    {
        private static StoreContext _storeContext;

        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }
        public static bool InitializeInAppPurchase(IntPtr hwnd)
        {
            _storeContext = StoreContext.GetDefault();
            IInitializeWithWindow initializeWithWindowWrapper = ((object)_storeContext).As<IInitializeWithWindow>();
            initializeWithWindowWrapper.Initialize(hwnd);
            return true;
        }
        public static async Task<string> DoPurchase(string StoreId)
        {
            var result = await _storeContext.RequestPurchaseAsync(StoreId);
            string strResult;
            switch (result.Status)
            {
                case StorePurchaseStatus.Succeeded:
                    strResult = $"Store purchase succeeded: {result.Status}";
                    break;
                default:
                    {
                        strResult = $"Store purchase failed: {result.Status}";
                    }
                    break;
            }
            return strResult;
        }

        public static async Task<string> purchaseDurable(string StoreId)
        {
            Debug.WriteLine("StoreContext.GetDefault...");
            if (_storeContext == null)
            {
                throw (new Exception("Store context not initialized"));
            }

            var result = await _storeContext.RequestPurchaseAsync(StoreId);
            if (result.ExtendedError != null)
            {
                throw new Exception(result.ExtendedError.Message);
            }
            return $"{result.Status}";
        }
    }
}
