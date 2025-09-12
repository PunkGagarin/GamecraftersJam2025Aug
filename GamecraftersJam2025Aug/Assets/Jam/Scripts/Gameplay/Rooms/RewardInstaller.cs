using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms
{
    public class RewardInstaller : MonoInstaller
    {
        [field: SerializeField]
        private BattleWinUi WinUi { get; set; }
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RoomRewardBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleWinUi>()
                .FromInstance(WinUi)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<BattleWinPresenter>().AsSingle();
        }
    }
}