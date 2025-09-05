using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms
{
    public class RewardInstaller : MonoInstaller
    {
        [field: SerializeField]
        private BattleRewardUi RewardUi { get; set; }
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RoomEventBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<RewardPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleRewardUi>()
                .FromInstance(RewardUi)
                .AsSingle();
        }
    }
}