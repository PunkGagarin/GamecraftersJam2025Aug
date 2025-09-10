using System;
using Jam.Scripts.Localization;
using UnityEngine;
using Zenject;

namespace Jam.Scripts
{
    public class LanguageService : MonoBehaviour
    {
        public event Action OnSwitchLanguage;
        
        [Inject] private LanguageModel _languageModel;
        
        public LanguageType CurrentLanguage
        {
            get => _languageModel.Language;
            set
            {
                _languageModel.SaveLanguage(value);
                OnSwitchLanguage?.Invoke();
            }
        }
    }
}