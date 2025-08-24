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
        [SerializeField] private MapConfig _mapConfig;

        public override void InstallBindings()
        {
            LanguageInstall();
            MapInstall();
        }

        private void LanguageInstall()
        {
            Container
                .Bind<LanguageConfig>()
                .FromInstance(_languageConfig)
                .AsSingle();
        }

        private void MapInstall()
        {
            Container
                .Bind<MapConfig>()
                .FromInstance(_mapConfig)
                .AsSingle();
        }
    }
}