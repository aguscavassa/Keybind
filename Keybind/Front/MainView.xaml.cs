using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Keybind.Front
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {
        public static List<Service> currentUserServicesList;
        public MainView()
        {
            this.InitializeComponent();

            currentUserServicesList = new List<Service>(new Service[3]
            {
                new Service("Facebook", "1", "Social Media"),
                new Service("Random Website", "2"),
                new Service("Reddit", "3", "Social Media")
            });
        }

        public class Service
        {
            public string Tag { get; set; }
            public string ServiceName { get; set; }
            public string Password { get; set; }

            public Service(string name, string password, string tag = "None")
            {
                this.Tag = tag;
                this.ServiceName = name;
                this.Password = password;
            }
        }
    }
}
