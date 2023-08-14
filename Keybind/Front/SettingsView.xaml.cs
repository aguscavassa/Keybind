using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Keybind.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Keybind.Front
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page
    {
        private string[] Languages = new string[]
        {
            Lifecycle.GetLocalizedString("Settings-EnglishLanguage"),
            Lifecycle.GetLocalizedString("Settings-SpanishLanguage")
        };

        private bool UseMicaEffectsSetting = false;
        private bool ShowPasswordsSetting = false;
        private string ActiveLanguage = Lifecycle.GetLocalizedString("Settings-EnglishLanguage");
        public SettingsView()
        {
            this.InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.GetMainWindow().NavigateDefault(typeof(MainView), null);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            CollectionManagement.Purge();
            MainView.RefreshGrid();
        }
    }
}
