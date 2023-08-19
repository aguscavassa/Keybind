using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using CommunityToolkit.WinUI.UI.Controls;
using System.Collections.ObjectModel;
using Keybind.Back;

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
        public static ObservableCollection<Service> currentServicesCollection;
        public MainView()
        {
            this.InitializeComponent();

            MainViewPage = this;
        }

        public void SearchChanged(AutoSuggestBox searchBox)
        {

            MainDataGrid.ItemsSource = CollectionManagement.ServiceCollection.FindAll(item => item.Name.Contains(searchBox.Text, StringComparison.InvariantCultureIgnoreCase) || item.User.Contains(searchBox.Text, StringComparison.InvariantCultureIgnoreCase) || item.Tag.Contains(searchBox.Text, StringComparison.InvariantCultureIgnoreCase));
        }

        private void MainDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            IOrderedEnumerable<Service> CollectionHelper;
            if (currentServicesCollection == null)
            {
                currentServicesCollection = new ObservableCollection<Service>();
            }
            currentServicesCollection.Clear();
            switch (e.Column.Tag.ToString())
            {
                case "Name":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        CollectionHelper = CollectionManagement.ServiceCollection.OrderBy(x => x.Name);
                        foreach (Service service in CollectionHelper)
                        {
                            currentServicesCollection.Add(service);
                        }
                        MainDataGrid.ItemsSource = currentServicesCollection;
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        CollectionHelper = CollectionManagement.ServiceCollection.OrderByDescending(x => x.Name);
                        foreach (Service service in CollectionHelper)
                        {
                            currentServicesCollection.Add(service);
                        }
                        MainDataGrid.ItemsSource = currentServicesCollection;
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
                case "Tag":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        CollectionHelper = CollectionManagement.ServiceCollection.OrderBy(x => x.Tag);
                        foreach (Service service in CollectionHelper)
                        {
                            currentServicesCollection.Add(service);
                        }
                        MainDataGrid.ItemsSource = currentServicesCollection;
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        CollectionHelper = CollectionManagement.ServiceCollection.OrderByDescending(x => x.Tag);
                        foreach (Service service in CollectionHelper)
                        {
                            currentServicesCollection.Add(service);
                        }
                        MainDataGrid.ItemsSource = currentServicesCollection;
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
                case "User Name":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        CollectionHelper = CollectionManagement.ServiceCollection.OrderBy(x => x.User);
                        foreach (Service service in CollectionHelper)
                        {
                            currentServicesCollection.Add(service);
                        }
                        MainDataGrid.ItemsSource = currentServicesCollection;
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        CollectionHelper = CollectionManagement.ServiceCollection.OrderByDescending(x => x.User);
                        foreach (Service service in CollectionHelper)
                        {
                            currentServicesCollection.Add(service);
                        }
                        MainDataGrid.ItemsSource = currentServicesCollection;
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
                case "Password":
                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                    {
                        CollectionHelper = CollectionManagement.ServiceCollection.OrderBy(x => x.Password);
                        foreach (Service service in CollectionHelper)
                        {
                            currentServicesCollection.Add(service);
                        }
                        MainDataGrid.ItemsSource = currentServicesCollection;
                        e.Column.SortDirection = DataGridSortDirection.Ascending;
                    }
                    else
                    {
                        CollectionHelper = CollectionManagement.ServiceCollection.OrderByDescending(x => x.Password);
                        foreach (Service service in CollectionHelper)
                        {
                            currentServicesCollection.Add(service);
                        }
                        MainDataGrid.ItemsSource = currentServicesCollection;
                        e.Column.SortDirection = DataGridSortDirection.Descending;
                    }
                    break;
                default:
                    break;
            }
            CollectionHelper = null;

            foreach (var col in MainDataGrid.Columns)
            {
                if (e.Column.Tag.ToString() != col.Tag.ToString())
                {
                    col.SortDirection = null;
                }
            }
        }

        public static void RefreshGrid()
        {
            MainView.MainViewPage.MainDataGrid.ItemsSource = null;
            MainView.MainViewPage.MainDataGrid.ItemsSource = CollectionManagement.ServiceCollection;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var normalFlyout = Resources["Context"] as MenuFlyout;
            var normalTarget = normalFlyout.Target;
            MenuFlyoutItem item = (MenuFlyoutItem)sender;
            TextBlock cellText = (TextBlock)((Grid)normalTarget).Children[0];

            if (string.Equals(item.Tag.ToString(), "copy"))
            {
                Lifecycle.CopyToClipboard(cellText.Text);
            }
        }

        private void PasswordMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            Service selectedService = (Service)MainDataGrid.SelectedItem;
            var passwordFlyout = Resources["PasswordContext"] as MenuFlyout;
            var passwordTarget = passwordFlyout.Target;
            MenuFlyoutItem item = (MenuFlyoutItem)sender;

            TextBlock passwordText = (TextBlock)((Grid)passwordTarget).Children[0];
            if (string.Equals(item.Tag.ToString(), "show"))
            {
                if (passwordText.Text == selectedService.MaskedPassword)
                {
                    passwordText.Text = selectedService.Password;
                    return;
                }
                if (passwordText.Text == selectedService.Password)
                {
                    passwordText.Text = selectedService.MaskedPassword;
                    item.Text = Lifecycle.GetLocalizedString("ShowPassFlyoutExtra");
                }
            }

            if (string.Equals(item.Tag.ToString(), "copy"))
            {
                Lifecycle.CopyToClipboard(selectedService.Password);
            }
        }

        private void MenuFlyout_Opening(object sender, object e)
        {
            MenuFlyout context = sender as MenuFlyout;
            if (MainDataGrid.SelectedItem == null)
            {
                context.Hide();
            }
        }

        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null)
            {
                // This part does not work. Trying to figure out why.
                Button deleteButton = Lifecycle.GetWindowItem<Button>(App.GetMainWindow().Content, typeof(Button), "DeleteButton");
                if (deleteButton.IsEnabled)
                {
                    deleteButton.IsEnabled = false;
                }
                deleteButton.IsEnabled = true;
                // No one knows. This next part does work.
                Button editButton = Lifecycle.GetWindowItem<Button>(App.GetMainWindow().Content, typeof(Button), "EditButton");
                editButton.IsEnabled = true;
            }
        }
    }
}
