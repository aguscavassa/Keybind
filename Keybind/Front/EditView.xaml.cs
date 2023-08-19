using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Keybind.Back;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Keybind.Front
{
    // TODO: Rewrite this class and the AddView class to minimize repetition using user controls.
    public sealed partial class EditView : Page
    {
        public static EditView EditViewPage;
        private List<string> TagList;
        private Service selectedService;
        public EditView()
        {
            this.InitializeComponent();
            TagList = CollectionManagement.GetTagList();
            EditViewPage = this;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedService == null)
            {
                AcceptTooltipText.Text = Lifecycle.GetLocalizedString("NoSelectionError");
                AcceptSymbol.Symbol = Symbol.Important;
                return;
            }
            if (ServiceNameField.Text == "" || UserField.Text == "" || PasswordField.Password == "")
            {
                AcceptTooltipText.Text = Lifecycle.GetLocalizedString("MissingFieldsError");
                AcceptSymbol.Symbol = Symbol.Important;
                return;
            }
            
            CollectionManagement.Modify(selectedService, new Service(ServiceNameField.Text, UserField.Text, PasswordField.Password));
            MainView.RefreshGrid();

            App.GetMainWindow().NavigateDefault(typeof(MainView), null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.GetMainWindow().NavigateDefault(typeof(MainView), null);
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordField.Password = Lifecycle.CreateRandomString();
        }

        private void TagField_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var itemList = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var item in TagList)
                {
                    var found = splitText.All((key) =>
                    {
                        return item.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        itemList.Add(item);
                    }
                }
                if (itemList.Count == 0)
                {
                    itemList.Add(Lifecycle.GetLocalizedString("NoTagsInfo"));
                }
                sender.ItemsSource = itemList;
            }
        }

        public void EnteredEdit()
        {
            Service t = (Service)MainView.MainViewPage.MainDataGrid.SelectedItem;
            if (t == null)
            {
                return;
            }
            TagField.Text = t.Tag;
            ServiceNameField.Text = t.Name;
            UserField.Text = t.User;
            PasswordField.Password = t.Password;
            selectedService = t;
        }
    }
}
