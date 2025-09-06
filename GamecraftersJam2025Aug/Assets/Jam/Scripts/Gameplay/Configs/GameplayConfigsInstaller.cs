using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Battle.ShellGame;
using Jam.Scripts.Gameplay.Rooms.Events.Data;
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
        private RoomEventRepository _roomEventRepository;

        [SerializeField]
        private PlayerUnitConfig _playerUnitConfig;

        [SerializeField]
        private MapConfig _mapConfig;

        [SerializeField]
        private BattleConfig _battleConfig;

        [field: SerializeField]
        private ShellGameConfig GameConfig { get; set; }

        public override void InstallBindings()
        {
            Container.Bind<BallsConfigRepository>().FromInstance(_ballsConfigRepository).AsSingle();
            Container.Bind<EnemyConfigRepository>().FromInstance(_enemyConfigRepository).AsSingle();
            Container.Bind<RoomEventRepository>().FromInstance(_roomEventRepository).AsSingle();
            Container.Bind<PlayerUnitConfig>().FromInstance(_playerUnitConfig).AsSingle();
            Container.Bind<BattleConfig>().FromInstance(_battleConfig).AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGameConfig>()
                .FromInstance(GameConfig)
                .AsSingle();

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