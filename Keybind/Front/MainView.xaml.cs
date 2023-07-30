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
using System.Diagnostics;
using Keybind.Services;
using CommunityToolkit.WinUI.UI.Controls;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Keybind.Front
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {
        public static MainView MainViewPage;
        public MainView()
        {
            this.InitializeComponent();

            MainViewPage = this;
        }

        private void MainDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        public void SearchChanged(AutoSuggestBox searchBox)
        {
            MainDataGrid.ItemsSource = new ObservableCollection<Service>(from item in CollectionManagement.ServiceCollection
                                                                         where item.Name.Contains(searchBox.Text) || item.User.Contains(searchBox.Text) || item.Tag.Contains(searchBox.Text)
                                                                         select item);
        }
    }
}
