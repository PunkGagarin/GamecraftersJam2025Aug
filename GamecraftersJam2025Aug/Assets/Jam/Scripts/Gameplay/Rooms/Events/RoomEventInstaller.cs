using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class RoomEventInstaller : MonoInstaller
    {
        [SerializeField] private RoomDealEventView _roomDealEventView;
        [SerializeField] private RoomRewardEventView _roomRewardEventView;
        [SerializeField] private DefaultRewardView _defaultrewardPrefab;
        [SerializeField] private ConcreteRewardView _concreteRewardPrefab;
        [SerializeField] private RandomRewardView _randomRewardPrefab;
        [SerializeField] private BallUpgradeRewardView _ballUpgradeRewardPrefab;
        [SerializeField] private DealCardView _dealCardPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RoomEventBus>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DefaultRewardView>().FromInstance(_defaultrewardPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ConcreteRewardView>().FromInstance(_concreteRewardPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RandomRewardView>().FromInstance(_randomRewardPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BallUpgradeRewardView>().FromInstance(_ballUpgradeRewardPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomDealEventView>().FromInstance(_roomDealEventView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DealCardView>().FromInstance(_dealCardPrefab).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RewardRiskService>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomRewardEventView>().FromInstance(_roomRewardEventView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomDealEventPresenter>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomRewardEventPresenter>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomEventService>().FromNew().AsSingle().NonLazy();
        }
    }
}