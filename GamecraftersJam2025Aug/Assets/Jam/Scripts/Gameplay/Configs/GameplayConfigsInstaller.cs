using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "Configs Installer", menuName = "Gameplay/Configs/ConfigInstaller")]
    public class GameplayConfigsInstaller : ScriptableObjectInstaller
    {

        [SerializeField]
        private BallsConfigRepository _ballsConfigRepository;

        [SerializeField]
        private EnemyConfigRepository _enemyConfigRepository;

        [SerializeField]
        private PlayerUnitConfig _playerUnitConfig;

        [SerializeField]
        private MapConfig _mapConfig;

        public override void InstallBindings()
        {
            Container.Bind<BallsConfigRepository>().FromInstance(_ballsConfigRepository).AsSingle();
            Container.Bind<EnemyConfigRepository>().FromInstance(_enemyConfigRepository).AsSingle();
            Container.Bind<PlayerUnitConfig>().FromInstance(_playerUnitConfig).AsSingle();
            MapInstall();
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