using Microsoft.UI.Composition.SystemBackdrops;
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

namespace Keybind
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Title = "Keybind";
            this.InitializeComponent();
            SystemBackdrop = new MicaBackdrop()
            { Kind = MicaKind.BaseAlt };
            Life.Init();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            UserPasswords uP = new UserPasswords("userMedia");
            Life.SaveUserPasswords(uP);
            myButton.Content = uP.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
