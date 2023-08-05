using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Microsoft.UI.Xaml.Media.Animation;
using Keybind.Front;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Composition.SystemBackdrops;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace Keybind
{
    public partial class App : Application
    {
        private static MainWindow m_window;
        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow
            {
                Title = "Keybind"
            };
            m_window.Activate();
            try
            {
                Services.CollectionManagement.LoadListFromDisk();
            } catch (FileNotFoundException)
            {
                Services.CollectionManagement.GenerateServiceCollection();
            }
        }

        public static MainWindow GetMainWindow()
        {
            return m_window;
        }
    }
}
