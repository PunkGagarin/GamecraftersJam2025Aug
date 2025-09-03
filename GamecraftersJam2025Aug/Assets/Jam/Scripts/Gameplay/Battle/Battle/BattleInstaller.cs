using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleInstaller : MonoInstaller
    {

        [field: SerializeField]
        private BattleRewardUi RewardUi { get; set; }

        [field: SerializeField]
        private BattleLoseUi LoseUi { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleEventBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CombatSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<AttackAckAwaiter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleStarter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleResultPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<BattleRewardUi>()
                .FromInstance(RewardUi)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<BattleLoseUi>()
                .FromInstance(LoseUi)
                .AsSingle();
        }
    }
}