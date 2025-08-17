using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Localization
{
    public class Localization
    {
        private const string UNDEFINED = "Undefined";
        private const string LANGUAGE = "Language";
        private const string LANG = "LANG";
        private const string ID = "ID";
        private const string KEY = "key";

        [Inject] private LanguageService _languageService;
        private LanguageConfig _languageConfig;

        private readonly Dictionary<int, Language> _languages = new();
        private int _currentLanguageID => (int)_languageService.CurrentLanguage;

        [Inject]
        public Localization(LanguageConfig languageConfig)
        {
            _languageConfig = languageConfig;
            Initialize(_languageConfig.LocalizationTextAssets);
        }

        private void Initialize(List<TextAsset> languageFiles)
        {
            foreach (TextAsset languageFile in languageFiles)
            {
                XDocument languageXMLData = XDocument.Parse(languageFile.text);
                var language = new Language
                {
                    languageID = int.Parse(languageXMLData.Element(LANGUAGE).Attribute(ID).Value),
                    languageString = languageXMLData.Element(LANGUAGE).Attribute(LANG).Value
                };
                foreach (XElement text in languageXMLData.Element(LANGUAGE).Elements())
                {
                    if (!language.TextKeyValueList.ContainsKey(text.Attribute(KEY).Value))
                        language.TextKeyValueList.Add(text.Attribute(KEY).Value, text.Value.Replace('%', '\n'));
                }

                _languages.Add(language.languageID, language);
            }
        }

        public string GetText(string key)
        {
            if (_languages.ContainsKey(_currentLanguageID))
            {
                return _languages[_currentLanguageID].TextKeyValueList.ContainsKey(key)
                    ? _languages[_currentLanguageID].TextKeyValueList[key]
                    : UNDEFINED;
            }

            Debug.Log($"[Localization] Key wasn't found {key}");
            return UNDEFINED;
        }
    }

    [Serializable]
    public class Language
    {
        public string languageString;
        public int languageID;
        public Dictionary<string, string> TextKeyValueList = new();
    }
}