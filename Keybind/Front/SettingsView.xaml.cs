using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Keybind.Back;

namespace Keybind.Front
{
    public sealed partial class SettingsView : Page
    {
        public SettingsView()
        {
            this.InitializeComponent();
            SettingsLanguageDropdown.SelectedItem = SettingsManagement.Languages[SettingsManagement.CurrentSettings.ActiveLanguage];
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsManagement.SetSettings(new SettingsManagement.Settings(SettingsLanguageDropdown.SelectedIndex, (bool)SettingsMicaCheckbox.IsChecked));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            App.GetMainWindow().NavigateDefault(typeof(MainView), null);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            TextBlock contentText = new TextBlock();
            contentText.TextWrapping = TextWrapping.WrapWholeWords;
            contentText.Text = string.Format(string.Concat(Lifecycle.GetLocalizedString("DeleteAllDialogueContentFirst"), "\n", Lifecycle.GetLocalizedString("DeleteAllDialogueContentSecond")));

            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = Lifecycle.GetLocalizedString("DeleteAllDialogTitle");
            dialog.PrimaryButtonText = Lifecycle.GetLocalizedString("AcceptButtonGeneral");
            dialog.CloseButtonText = Lifecycle.GetLocalizedString("CancelButtonGeneral");
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.Content = contentText;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                CollectionManagement.Purge();
                MainView.RefreshGrid();
                App.GetMainWindow().NavigateDefault(typeof(MainView), null);
            }
        }
    }
}
