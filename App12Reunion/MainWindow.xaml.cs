using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using Windows.Services.Store;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App12Reunion
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    [ComImport]
    [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithWindow
    {
        void Initialize(IntPtr hwnd);
    }
    
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }
        public StoreContext m_StoreContext { get; private set; }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
            DoInAppPurchase();
        }

        public async void DoInAppPurchase()
        {
            if (m_StoreContext == null)
            {
                m_StoreContext = StoreContext.GetDefault();
            }
            var result = await m_StoreContext.RequestPurchaseAsync("9NWMTMH9PNGB");
            StoreConsumableResult res;
            switch (result.Status)
            {
                case StorePurchaseStatus.Succeeded:
                    textBlock.Text = $"Store purchase succeeded: {result.Status}";
                    break;
                case StorePurchaseStatus.AlreadyPurchased:
                    {
                        res = await m_StoreContext.ReportConsumableFulfillmentAsync("9NWMTMH9PNGB", 1, Guid.NewGuid());
                        textBlock.Text = $"Store purchase failed: {result.Status}";
                        textBlock.Text += $"\nStore Fulfillment result: {res.Status}";
                    }
                    break;
                default:
                    {
                        textBlock.Text = $"Store purchase failed: {result.Status}";
                    }
                    break;
            }


        }

        bool bActivated = false;
        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            IntPtr hwnd = PInvoke.User32.GetActiveWindow();
            if ( (hwnd == (IntPtr)0) || (bActivated) )
            {
                return;
            }
            m_StoreContext = StoreContext.GetDefault();
            IInitializeWithWindow initializeWithWindowWrapper = ((object)m_StoreContext).As<IInitializeWithWindow>();
            initializeWithWindowWrapper.Initialize(hwnd);
            bActivated = true;
        }
    }
}
