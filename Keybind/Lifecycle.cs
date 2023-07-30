using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Cryptography;
using Keybind.Front;
using System.Collections.Generic;

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
}

    
