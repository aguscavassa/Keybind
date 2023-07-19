using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Microsoft.UI.Xaml.Media.Animation;
using Keybind.Front;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Composition.SystemBackdrops;

namespace Keybind
{
    public partial class App : Application
    {
        private Window m_window;
        internal Frame rootFrame;
        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Title = "Keybind";
            m_window.Activate();
            Lifecycle.Init();
        }
    }
}
