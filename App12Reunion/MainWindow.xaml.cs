using Microsoft.UI.Xaml;
using SimpleGeolocation;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using CPPClassLibrary;
using RuntimeComponentCpp;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App12Reunion
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            txtVersion.Text = GetAppVersion();

            var b = new SimpleMath();
            var res = b.add(1, 3);
            txtMath.Text = $"Result from RuntimeComponentCpp.SimpleMath.add. 1 + 3 = {res}"; // RuntimeComponentCpp is a Windows Runtime Component exposed through a C# Projection
            txtMath.Text += $"\nHello from CPPClassLibrary.SimpleMathCpp: {SimpleMathCpp.add(2, 2)}"; // CPPClassLibrary is a C++ .NET 5.0 Class Library. No WinRT and No projection needed.


        }

        public string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            return "Version " + string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private async void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            LookUpAddress loc = new LookUpAddress();
            var strLoc =  await loc.GetCurrentLocationOneShot();
            var res = $"\nLatitude, Longitude: {strLoc}";
            txtLocation.Text = res;
            var msg = new MessageDialog(res);
            msg.Title = "Your location";
            msg.Initialize();
            await msg.ShowAsync();
        }
    }
}
