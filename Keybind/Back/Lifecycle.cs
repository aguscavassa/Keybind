using System;
using System.IO;
using System.Security.Cryptography;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;

namespace Keybind.Back;

public static class Lifecycle
{
    private static ResourceManager resManager;
    private static string userDir;
    private static string userUuid;
    public static string UserDir { get => userDir; set => userDir = value; }

    public static string UserUuid { get => userUuid; set => userUuid = value; }

    public static string AssetsFolder = AppDomain.CurrentDomain.BaseDirectory + @"\AppX\Assets\";

    public static string[] ImageAssets = new string[]
    {
        AssetsFolder + "github-mark-white.png",
        AssetsFolder + "Square44x44Logo.targetsize-256.png"
    };
    

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
            CollectionManagement.LoadListFromDisk();
        }
        catch (FileNotFoundException)
        {
            CollectionManagement.GenerateServiceCollection();
        }
        SettingsManagement.GetSettings();
        Console.WriteLine(ImageAssets[0]);
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
            if (child.GetWindowItem<T>(type, name) != null)
            {
                result = child.GetWindowItem<T>(type, name);
                break;
            }
        }
        return result;
    }

    public static string GetLocalizedString(string value)
    {
        return resManager.MainResourceMap.GetSubtree("Resources").GetValue(value).ValueAsString;
    }

    public static void SetLanguage(int activeLanguage)
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture = SettingsManagement.Languages[activeLanguage];
        //Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = SettingsManagement.Languages[activeLanguage].Name;
        Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
        Windows.ApplicationModel.Resources.Core.ResourceManager.Current.DefaultContext.Reset();
    }

    
}


