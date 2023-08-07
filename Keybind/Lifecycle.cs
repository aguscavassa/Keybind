using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Cryptography;
using Keybind.Front;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.VisualBasic;

namespace Keybind;

public static class Lifecycle
{
    private static ResourceManager resManager;
    private static string userDir;
    private static string userUuid;
    public static string UserDir { get => userDir; set => userDir = value; }

    public static string UserUuid { get => userUuid; set => userUuid = value; }

    public static void Init()
    {
        resManager = new ResourceManager();
        UserDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Data";
        Directory.CreateDirectory(UserDir);
        if (File.Exists(UserDir + "\\user.dat"))
        {
            userUuid = File.ReadAllText(UserDir + "\\user.dat");
        }
        else
        {
            userUuid = Guid.NewGuid().ToString();
            File.WriteAllText(UserDir + "\\user.dat", userUuid);
        }
        try
        {
            Services.CollectionManagement.LoadListFromDisk();

        } catch (FileNotFoundException)
        {
            Services.CollectionManagement.GenerateServiceCollection();
        }
    }

    public static string CreateRandomString(int length = 12)
    {
        byte[] bytes = new byte[length];
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }

    public static void CopyToClipboard(string str)
    {
        var package = new DataPackage();
        package.SetText(str);
        Clipboard.SetContent(package);
    }

    public static T GetWindowItem<T>(this UIElement parent, Type type, string name) where T : FrameworkElement
    {
        if (parent == null) return null;

        if (parent.GetType() == type && ((T)parent).Name == name)
        {
            return (T)parent;
        }
        T result = null;
        int j = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < j; i++)
        {
            UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);
            if (GetWindowItem<T>(child, type, name) != null)
            {
                result = GetWindowItem<T>(child, type, name);
                break;
            }
        }
        return result;
    }

    public static string GetLocalizedString(string value)
    {
        return resManager.MainResourceMap.GetSubtree("Resources").GetValue(value).ValueAsString;
    }
}

    
