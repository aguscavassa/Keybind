using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Keybind.Back;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Keybind.Front
{
    // TODO: Rewrite this class and the EditView class to minimize repetition using user controls.
    public sealed partial class AddView : Page
    {
        private List<string> TagList;
        public AddView()
        {
            this.InitializeComponent();
            TagList = CollectionManagement.GetTagList();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.GetMainWindow().NavigateDefault(typeof(MainView), null);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceNameField.Text == "" || UserField.Text == "" || PasswordField.Password == "")
            {
                AcceptTooltipText.Text = Lifecycle.GetLocalizedString("MissingFieldsError");
                AcceptSymbol.Symbol = Symbol.Important;
                return;
            }

            if (TagField.Text == "")
            {
                CollectionManagement.Add(new Service(ServiceNameField.Text, UserField.Text, PasswordField.Password));
                MainView.RefreshGrid();
            } else
            {
                CollectionManagement.Add(new Service(ServiceNameField.Text, UserField.Text, PasswordField.Password, TagField.Text));
                MainView.RefreshGrid();
            }

            App.GetMainWindow().NavigateDefault(typeof(MainView), null);
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

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordField.Password = Lifecycle.CreateRandomString();
        }
    }
}
