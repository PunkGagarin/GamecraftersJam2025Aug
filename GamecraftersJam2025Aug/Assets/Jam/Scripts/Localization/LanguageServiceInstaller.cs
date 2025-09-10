using UnityEngine;
using Zenject;

namespace Jam.Scripts
{
    public class LanguageServiceInstaller : MonoInstaller
    {
        [SerializeField] private LanguageService _languageServicePrefab;

        public override void InstallBindings()
        {
            InstallLanguageModel();
            InstallLanguageService();
            InstallLocalization();
        }

        private void InstallLanguageModel()
        {
            Container.Bind<LanguageModel>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void InstallLanguageService()
        {
            Container.Bind<LanguageService>()
                .FromComponentInNewPrefab(_languageServicePrefab)
                .AsSingle()
                .NonLazy();
        }

        private void InstallLocalization()
        {
            Container.Bind<LocalizationTool>()
                .AsSingle()
                .NonLazy();
        }
    }
}