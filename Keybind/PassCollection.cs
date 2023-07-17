using Keybind.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybind
{
    public class UserKeysCollection : IUserKeysCollection
    {
        public string Uuid;
        private Dictionary<string, Dictionary<string, string>> taggedKeys;

        public Dictionary<string, Dictionary<string, string>> TaggedKeys { get { return taggedKeys; } }

        public UserKeysCollection(string uuid)
        {
            Uuid = uuid;
        }

        public void AddTagCollection(string tag)
        {
            TaggedKeys.Add(tag, new Dictionary<string, string>());
        }

        public bool RemoveTagCollection(string tag)
        {
            return TaggedKeys.Remove(tag);
        }

        public Dictionary<string, string> GetTagCollection(string tag)
        {
            return TaggedKeys[tag];
        }
    }
}
