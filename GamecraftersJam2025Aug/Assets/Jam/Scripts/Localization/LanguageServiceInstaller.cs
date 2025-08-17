using UnityEngine;
using Zenject;

namespace Jam.Scripts.Localization
{
    public class LanguageServiceInstaller : MonoInstaller
    {
        [SerializeField] private LanguageService languageServicePrefab;

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
                .FromComponentInNewPrefab(languageServicePrefab)
                .AsSingle()
                .NonLazy();
        }

        private void InstallLocalization()
        {
            Container.Bind<Localization>()
                .AsSingle()
                .NonLazy();
        }
    }
}