using Microsoft.UI.Xaml;
using System.IO;
using Keybind.Back;

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
                SettingsManagement.GetSettings();
                CollectionManagement.LoadListFromDisk();
            } catch (FileNotFoundException)
            {
                CollectionManagement.GenerateServiceCollection();
            }
        }

        public static MainWindow GetMainWindow()
        {
            return m_window;
        }


    }
}
