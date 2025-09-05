using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Enemy
{
    public class EnemyInstaller : MonoInstaller
    {

        [field: SerializeField]
        private BattleEnemyPanelUI BattleEnemyPanel { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemyBattlePresenter>().AsSingle().NonLazy();
            Container.Bind<BattleEnemyService>().AsSingle();
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<EnemyEventBus>().AsSingle();

            Container.Bind<BattleEnemyPanelUI>().FromInstance(BattleEnemyPanel).AsSingle();
        }
    }
}