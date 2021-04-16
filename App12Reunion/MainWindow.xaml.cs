using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using Windows.ApplicationModel;
using Windows.Services.Store;
using Windows.UI.Popups;
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
            txtVersion.Text = GetAppVersion();
        }
        public StoreContext m_StoreContext { get; private set; }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
            DoInAppPurchase();
        }

        public async void DoInAppPurchase()
        {
            // Init m_StoreContext in Window_Activated
            var res = await ClassLibrary1.Class1.DoPurchase("9NWMTMH9PNGB");
            textBlock.Text = res;

            var b = new RuntimeComponentCpp.SimpleMath();
            var res1 = b.add(1, 3);

            MessageDialog msg = new MessageDialog($"Answer is {res1}");
            IInitializeWithWindow initializeWithWindowWrapper = msg.As<IInitializeWithWindow>();
            IntPtr hwnd = PInvoke.User32.GetActiveWindow();
            initializeWithWindowWrapper.Initialize(hwnd);
            await msg.ShowAsync();

        }

        bool bActivated = false;
        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            IntPtr hwnd = PInvoke.User32.GetActiveWindow();
            if ( (hwnd == (IntPtr)0) || (bActivated) )
            {
                return;
            }
            ClassLibrary1.Class1.InitializeInAppPurchase(hwnd);
            //m_StoreContext = StoreContext.GetDefault();
            //IInitializeWithWindow initializeWithWindowWrapper = ((object)m_StoreContext).As<IInitializeWithWindow>();
            //initializeWithWindowWrapper.Initialize(hwnd);
            //bActivated = true;
        }

        public string GetAppVersion()
        {

            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return "Version " + string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

        }
    }
}
