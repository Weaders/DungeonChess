using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Character;

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

        public static IEnumerable<Placeholder> GetPlaceholdersFromAttrs(this object obj, CharacterData from) {

            var pls = obj.GetType().GetMethods().Select(p => {
                var key = p.GetCustomAttribute<PlaceholderAttribute>()?.Key;
                return key != null ? new Placeholder(key, p.Invoke(obj, new[] { from }).ToString()) : null;
            });

            return pls.Union(obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Select(p => {
                var key = p.GetCustomAttribute<PlaceholderAttribute>()?.Key;
                return key != null ? new Placeholder(key, p.GetValue(obj).ToString()) : null;
            })).Where(kv => kv != null);

        }

        public static Dictionary<string, string> ToDictionary(this IEnumerable<Placeholder> placeholders)
            => placeholders.ToDictionary(x => x.key, x => x.value);

    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class PlaceholderAttribute : Attribute { 
        
        public string Key { get; set; }

        public PlaceholderAttribute(string key) {
            Key = key;
        }

    }

}
