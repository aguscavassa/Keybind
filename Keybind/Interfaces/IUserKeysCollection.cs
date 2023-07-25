using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keybind.Interfaces
{
    public interface IUserKeysCollection
    {
        static string uuid;
        static Dictionary<string, Dictionary<string, string>> taggedCollection;
        public void AddTagCollection(string tag);
        public bool RemoveTagCollection(string tag);
        public Dictionary<string, string> GetTagCollection(string tag);
    }
}
