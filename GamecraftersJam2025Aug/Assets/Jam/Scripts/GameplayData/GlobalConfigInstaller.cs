using Jam.Scripts.Localization;
using Jam.Scripts.MapFeature.Map;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.GameplayData
{
    [CreateAssetMenu(fileName = "Global Config Installer", menuName = "Game Resources/Global Config Installer")]
    public class GlobalConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private LanguageConfig _languageConfig;

        public override void InstallBindings()
        {
            LanguageInstall();
        }

        private void LanguageInstall()
        {
            Container
                .Bind<LanguageConfig>()
                .FromInstance(_languageConfig)
                .AsSingle();
        }
    }
}