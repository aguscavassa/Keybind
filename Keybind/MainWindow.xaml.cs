using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using Keybind.Front;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using Windows.UI;
using System.Runtime.InteropServices;
using Keybind.Back;

namespace Keybind
{
    public sealed partial class MainWindow : Window
    {
        [DllImport("Shcore.dll", SetLastError = true)]
        internal static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);
        private readonly AppWindow _mainAppWindow;

        public MainWindow()
        {
            this.InitializeComponent();
            Lifecycle.Init();
            SystemBackdrop = new MicaBackdrop()
            { Kind = MicaKind.BaseAlt };
            _mainAppWindow = GetAppWindowForCurrentWindow();
            _mainAppWindow.TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            _mainAppWindow.Resize(new Windows.Graphics.SizeInt32(1280, 720));
            _mainAppWindow.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(128,58,58,58);

            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                var customTitleBar = _mainAppWindow.TitleBar;
                customTitleBar.ExtendsContentIntoTitleBar = true;
                TitleBar.Loaded += TitleBarLoaded;
                TitleBar.SizeChanged += TitleBarSizeChanged;
            } else
            {
                TitleBar.Visibility = Visibility.Collapsed;
            }
            NavigateDefault(typeof(MainView), null);
            _mainAppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            _mainAppWindow.Destroying += (AppWindow u, object o) =>
            {
                CollectionManagement.ServiceCollection = MainView.MainViewPage.MainDataGrid.ItemsSource.Cast<Service>().ToList();
                CollectionManagement.SaveListToDisk();
            };
        }

        private void TitleBarSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (AppWindowTitleBar.IsCustomizationSupported() && _mainAppWindow.TitleBar.ExtendsContentIntoTitleBar) SetDragRegion(_mainAppWindow);
        }

        private void TitleBarLoaded(object sender, RoutedEventArgs e)
        {
            if (AppWindowTitleBar.IsCustomizationSupported()) SetDragRegion(_mainAppWindow);
        }

        private void SetDragRegion(AppWindow _mainWindow)
        {
            double scale = GetScaleAdjustment();

            RightPaddingCol.Width = new GridLength(_mainWindow.TitleBar.RightInset / scale);
            LeftPaddingCol.Width = new GridLength(_mainWindow.TitleBar.LeftInset / scale);

            PaddingCol.Width = new GridLength(_mainWindow.TitleBar.RightInset - (IconCol.ActualWidth + NameCol.ActualWidth) / scale);

            List<Windows.Graphics.RectInt32> dragRectsList = new();

            Windows.Graphics.RectInt32 dragRectL;
            dragRectL.X = (int)((LeftPaddingCol.ActualWidth) * scale);
            dragRectL.Y = 0;
            dragRectL.Height = (int)(TitleBar.ActualHeight * scale);
            dragRectL.Width = (int)((IconCol.ActualWidth
                                    + NameCol.ActualWidth
                                    + LeftDraggingCol.ActualWidth
                                    + PaddingCol.ActualWidth) * scale);
            dragRectsList.Add(dragRectL);

            Windows.Graphics.RectInt32 dragRectR;
            dragRectR.X = (int)((LeftPaddingCol.ActualWidth
                                + IconCol.ActualWidth
                                + NameCol.ActualWidth
                                + ContentCol.ActualWidth
                                + PaddingCol.ActualWidth
                                + LeftDraggingCol.ActualWidth) * scale);
            dragRectR.Y = 0;
            dragRectR.Height = (int)(TitleBar.ActualHeight * scale);
            dragRectR.Width = (int)((RightDraggingCol.ActualWidth) * scale);
            dragRectsList.Add(dragRectR);

            Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();

            _mainWindow.TitleBar.SetDragRectangles(dragRects);
        }

        internal enum Monitor_DPI_Type : int
        {
            MDT_Effective_DPI = 0,
            MDT_Angular_DPI = 1,
            MDT_Raw_DPI = 2,
            MDT_Default = MDT_Effective_DPI
        }

        private double GetScaleAdjustment()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
            IntPtr hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

            int result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out uint dpiX, out uint _);

            if (result != 0)
            {
                throw new Exception("Could not get DPI for monitor.");
            }

            uint scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
            return scaleFactorPercent / 100.0;
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        public void NavigateDefault(Type page, object param)
        {
            if (page != MainFrame.CurrentSourcePageType)
            {
                MainFrame.Navigate(page, param, new DrillInNavigationTransitionInfo());
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateDefault(typeof(AddView), null);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateDefault(typeof(SettingsView), null);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateDefault(typeof(MainView), null);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateDefault(typeof(EditView), null);
            EditView.EditViewPage.EnteredEdit();
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            MainView.MainViewPage.SearchChanged(sender);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            CollectionManagement.Delete((Service)MainView.MainViewPage.MainDataGrid.SelectedItem);
            MainView.RefreshGrid();
        }

        // Somehow, the DeleteButton does not get enabled in the MainDataGrid_SelectionChanged method, so this is a bandaid fix until I can find a solution.
        private void EditButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.OldValue == false)
            {
                DeleteButton.IsEnabled = true;
            }
        }
    }
}
