using Keybind.Front;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Keybind.Services
{
    public class Service : IEquatable<Service>
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string MaskedPassword { get; set; }


        public Service(string Name, string User, string Password, string Tag = "None")
        {
            Regex mask = new(".");
            this.Name = Name;
            this.User = User;
            this.Password = Password;
            this.Tag = Tag;
            MaskedPassword = mask.Replace(Password, "*");
        }
        public override string ToString()
        {
            return $"{Name} " + $"{Tag}" + $"{User} " + $"{Password}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not Service) return false;
            Service other = obj as Service;
            if (other == null) { return false; }
            else return Equals(other);
        }

        public bool Equals(Service other)
        {
            if (other == null) return false;
            return (this.Name.Equals(other.Name) && this.User.Equals(other.User));
        }

    }


    public static class CollectionManagement
    {
        public static List<Service> ServiceCollection { get; set; }

        public static bool GenerateServiceCollection()
        {
            if (ServiceCollection == null)
            {
                ServiceCollection = new List<Service>();
                return true;
            }
            return false;
        }

        public static void Add(Service service)
        {
            if (!ServiceCollection.Contains(service))
            {
                ServiceCollection.Add(service);
            }
        }

        public static void Modify(Service service, Service newService)
        {
            Service helper = ServiceCollection.Find(x => (x.Name == service.Name) && (x.User == service.User) && (x.Password == service.Password));
            helper.User = newService.User;
            helper.Password = newService.Password;
            helper.Name = newService.Name;
        }

        public static bool Delete(Service service)
        {
            if (ServiceCollection.Contains(service))
            {
                ServiceCollection.Remove(service);
                return true;
            }
            else return false;

        }

        public static bool SaveListToDisk()
        {
            string jsonSerialized = JsonSerializer.Serialize(ServiceCollection);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonSerialized);
            byte[] protectedBuffer = ProtectedData.Protect(buffer, null, DataProtectionScope.CurrentUser);
            string protectedText = Convert.ToBase64String(protectedBuffer);
            try
            {
                File.WriteAllText($"{Lifecycle.UserDir}\\{Lifecycle.UserUuid}.dat", protectedText);
                return true;
            }
            catch
            {
                throw;
            }
        }
        public static List<Service> LoadListFromDisk()
        {
            string protectedText = File.ReadAllText($"{Lifecycle.UserDir}\\{Lifecycle.UserUuid}.dat");
            byte[] protectedBuffer = Convert.FromBase64String(protectedText);
            byte[] buffer = ProtectedData.Unprotect(protectedBuffer, null, DataProtectionScope.CurrentUser);
            ServiceCollection = JsonSerializer.Deserialize<List<Service>>(buffer);
            return ServiceCollection;
        }

        public static List<string> GetTagList()
        {
            List<string> list = new List<string>();
            foreach (Service service in ServiceCollection)
            {
                if (!list.Contains(service.Tag)) list.Add(service.Tag);
            }
            return list;
        }
    }
}
