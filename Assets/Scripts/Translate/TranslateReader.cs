using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Assets.Scripts.Logging;
using UnityEngine;

namespace Assets.Scripts.Translate {

    public class TranslateAttribute : Attribute {

        public string key { get; set; }

        public TranslateAttribute(string keyForTranslate) {
            key = keyForTranslate;
        }

    }

    public class TranslateReader {

        private static Dictionary<Lang, Dictionary<string, string>> translations = new Dictionary<Lang, Dictionary<string, string>> { };

        public static Dictionary<string, string> additionalData = new Dictionary<string, string>();

        public static string GetTranslate(string key, Dictionary<string, string> vals = null)
            => GetTranslate(StaticData.current.lang, key, vals);

        public static string GetTranslate(string key, IEnumerable<Placeholder> placeholders)
            => GetTranslate(StaticData.current.lang, key, placeholders.ToDictionary());

        public static string GetTranslate(string key, params Placeholder[] placeholders)
            => GetTranslate(key, placeholders.ToDictionary());

        private readonly static Regex regex = new Regex("%\\[([^\\[\\]]+)\\]");

        public static string GetTranslate(Lang lang, string key, Dictionary<string, string> vals = null) {

            if (key == string.Empty)
                return string.Empty;

            if (!translations.ContainsKey(lang))
                translations.Add(lang, ReadTranslateFile(lang.ToString()));

            if (!translations[lang].ContainsKey(key)) {

#if UNITY_EDITOR
                throw new Exception($"There no translation with key - {key}, lang - {lang}");
#else
                
                TagLogger<TranslateReader>.Error($"There no translation with key - {key}, lang - {lang}");
                return key;
#endif

            }

            var tr = translations[lang][key];

            if (vals != null) {

                var matches = regex.Matches(tr);

                foreach (Match m in matches) {

                    if (m.Success && vals.TryGetValue(m.Groups[1].Value, out var res)) {
                        tr = tr.Replace(m.Groups[0].Value, res);
                    }

                }

            }

            return tr;

        }

        public static void RefreshTranslate() => translations = new Dictionary<Lang, Dictionary<string, string>> { };

        private static Dictionary<string, string> ReadTranslateFile(string lang) {

            var jsonData = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "translation", $"{lang}.json"));

            var data = JsonUtility.FromJson<TranslateModel>(jsonData);

            var result = new Dictionary<string, string>();

            foreach (var translation in data.translations)
                result.Add(translation.key, translation.value);

            return result;

        }

        public enum Lang {
            ru, en
        }
    }

}
