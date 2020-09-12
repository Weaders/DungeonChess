using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Translate {

    [Serializable]
    public class Placeholder {

        public string key;
        public string value;

        public Placeholder() { }

        public Placeholder(string k, string v) {
            key = k;
            value = v;
        }

        public Placeholder(string k, object v) : this(k, v.ToString()) { }

        public Placeholder((string, string) kv) {
            (key, value) = kv;
        }

    }

    public static class PlaceholderHelper {

        public static Dictionary<string, string> ToDictionary(this IEnumerable<Placeholder> placeholders)
            => placeholders.ToDictionary(x => x.key, x => x.value);

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PlacecholderAttribute : Attribute { 
        
        public string Key { get; set; }

        public PlacecholderAttribute(string key) {
            Key = key;
        }

    }

}
