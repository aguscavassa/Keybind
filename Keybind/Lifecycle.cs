using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Cryptography;


namespace Keybind;

public static class Lifecycle
{
    private static string userDir;
    private static string userUuid;
    public static string UserDir { get => userDir; set => userDir = value; }

    public static string UserUuid { get => userUuid; set => userUuid = value; }

    public static void Init()
    {
        UserDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Data";
        if (File.Exists(UserDir + "\\user.dat"))
        {
            userUuid = File.ReadAllText(UserDir + "\\user.dat");
        } else
        {
            userUuid = Guid.NewGuid().ToString();
            File.WriteAllText(UserDir + "\\user.dat", userUuid);
        }
    }

    public static void SaveUserPasswords(UserKeysCollection userPasswords)
    {
        string jsonSerialized = JsonSerializer.Serialize(userPasswords);
        byte[] buffer = Encoding.UTF8.GetBytes(jsonSerialized);
        byte[] protectedBuffer = ProtectedData.Protect(buffer, null, DataProtectionScope.CurrentUser);
        string protectedText = Convert.ToBase64String(protectedBuffer);
        try
        {
            File.WriteAllText($"{UserDir}\\{UserUuid}.dat", protectedText);
        } catch
        {
            throw;
        }
    }

    public static UserKeysCollection LoadUserPasswords()
    {
        try
        {
            string protectedText = File.ReadAllText($"{UserDir}\\{UserUuid}.dat");
            byte[] protectedBuffer = Convert.FromBase64String(protectedText);
            byte[] buffer = ProtectedData.Unprotect(protectedBuffer, null, DataProtectionScope.CurrentUser);
            return JsonSerializer.Deserialize<UserKeysCollection>(Encoding.UTF8.GetString(buffer));
        } catch
        {
            throw;
        }
    }

    public static UserKeysCollection GenerateUserKeysCollection(string uuid)
    {
        return new UserKeysCollection(uuid);
    }
}
