using Jam.Prefabs.Gameplay.Gold;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Battle.ShellGame;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.ShellGame;
using Jam.Scripts.Gameplay.Rooms.Events;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Configs
{
    // [CreateAssetMenu(fileName = "Configs Installer", menuName = "Gameplay/Configs/ConfigInstaller")]
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

        [SerializeField]
        private RoomEventConfig _roomEventConfig;

        [field: SerializeField]
        private ShellGameConfig GameConfig { get; set; }

        [field: SerializeField]
        private GoldConfig GoldConfig { get; set; }

        [field: SerializeField]
        private BattleWinConfig WinConfig { get; set; }

        public override void InstallBindings()
        {
            Container.Bind<BallsConfigRepository>().FromInstance(_ballsConfigRepository).AsSingle();
            Container.Bind<EnemyConfigRepository>().FromInstance(_enemyConfigRepository).AsSingle();
            Container.Bind<RoomEventRepository>().FromInstance(_roomEventRepository).AsSingle();
            Container.Bind<PlayerUnitConfig>().FromInstance(_playerUnitConfig).AsSingle();
            Container.Bind<BattleConfig>().FromInstance(_battleConfig).AsSingle();
            Container.Bind<RoomEventConfig>().FromInstance(_roomEventConfig).AsSingle();
            Container.Bind<ShellGameConfig>().FromInstance(GameConfig).AsSingle();
            Container.Bind<GoldConfig>().FromInstance(GoldConfig).AsSingle();
            Container.Bind<BattleWinConfig>().FromInstance(WinConfig).AsSingle();

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