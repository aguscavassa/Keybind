using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybind
{
    public class PassCollection
    {
        private Dictionary<string, string> passwordCollection;
        public Dictionary<string, string> PasswordCollection
        {
            get { return passwordCollection; }
            set { passwordCollection = value; }
        }

        public PassCollection()
        {
            PasswordCollection = new Dictionary<string, string>();
        }

        public PassCollection(Dictionary<string, string> pairs)
        {
            PasswordCollection ??= new Dictionary<string, string>();
            PasswordCollection.Clear();
            PasswordCollection = pairs;
        }

        public void AddToCollection(string key, string value)
        {
            PasswordCollection.Add(key, value);
        }

        public void RemoveFromCollection(string key)
        {
            PasswordCollection.Remove(key);
        }

        public string GetPassword(string key)
        {
            return PasswordCollection[key];
        }
    }

    public class UserPasswords
    {
        private Dictionary<string, PassCollection> taggedCollection;

        public Dictionary<string, PassCollection> TaggedCollection {
            get { return taggedCollection; }
            set { taggedCollection = value; }
        }

        public UserPasswords(string tag)
        {
            TaggedCollection = new Dictionary<string, PassCollection>
            {
                { tag, new PassCollection() }
            };
        }
    }
}
