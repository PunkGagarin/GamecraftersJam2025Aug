using UnityEngine;
using Zenject;

namespace Jam.Scripts.Localization
{
    public class LanguageModel
    {
        private const string LANGUAGE_KEY = "Language";

        private LanguageConfig _languageConfig;

        public LanguageType Language { get; private set; }

        [Inject]
        public LanguageModel(LanguageConfig languageConfig)
        {
            _languageConfig = languageConfig;
            LoadLanguage();
        }

        private void LoadLanguage()
        {
            var savedLanguage = PlayerPrefs.GetInt(LANGUAGE_KEY, (int)_languageConfig.DefaultLanguage);
            Language = savedLanguage switch
            {
                (int)LanguageType.English => LanguageType.English,
                (int)LanguageType.Russian => LanguageType.Russian,
                _ => Language
            };
        }

        public void SaveLanguage(LanguageType language)
        {
            PlayerPrefs.SetInt(LANGUAGE_KEY, (int)language);
            LoadLanguage();
        }
    }
}