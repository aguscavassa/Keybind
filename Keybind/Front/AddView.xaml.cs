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
using Windows.UI;
using System.Web;
using Windows.Security.Cryptography.Core;

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
            TagList = Services.CollectionManagement.GetTagList();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.GetMainWindow().NavigateDefault(typeof(MainView), null);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceNameField.Text == "" || UserField.Text == "" || PasswordField.Password == "")
            {
                ToolTipService.SetToolTip(AcceptButton, "One (or more) of the required fields are empty.");
                AcceptSymbol.Symbol = Symbol.Important;
                return;
            }

            if (TagField.Text == "")
            {
                Services.CollectionManagement.Add(new Services.Service(ServiceNameField.Text, UserField.Text, PasswordField.Password));
            } else
            {
                Services.CollectionManagement.Add(new Services.Service(ServiceNameField.Text, UserField.Text, PasswordField.Password, TagField.Text));
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
                    itemList.Add("No tags found.");
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
