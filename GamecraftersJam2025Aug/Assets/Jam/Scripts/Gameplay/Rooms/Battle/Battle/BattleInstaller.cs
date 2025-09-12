using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleInstaller : MonoInstaller
    {

        [field: SerializeField]
        private BattleLoseUi LoseUi { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleEventBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CombatSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<AttackAckAwaiter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleStarter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleLosePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleWinGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<WinRewardSystem>().AsSingle();

            Container.BindInterfacesAndSelfTo<BattleLoseUi>()
                .FromInstance(LoseUi)
                .AsSingle();
        }
    }
}