using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SaltyTank
{
    public static class Localization
    {
        public static SystemLanguage language;
        public static Locale locale;

        public static void Init()
        {
            language = SystemLanguage.English;
            string lang = language.ToString();
            TextAsset dataDefault = Resources.Load<TextAsset>("Locales/English");
            TextAsset langData = Resources.Load<TextAsset>("Locales/" + lang);

            locale = new Locale();
            locale.keys = new Dictionary<string, string>();

            if (langData != null)
            {
                locale = JsonConvert.DeserializeObject<Locale>(langData.text);
            }else
            {
                locale = JsonConvert.DeserializeObject<Locale>(dataDefault.text);
            }
        }

        public static string Translate(string key)
        {
            return locale.keys[key];
        }
    }

    [System.Serializable]
    public struct Locale
    {
        public Dictionary<string, string> keys;
    }
}