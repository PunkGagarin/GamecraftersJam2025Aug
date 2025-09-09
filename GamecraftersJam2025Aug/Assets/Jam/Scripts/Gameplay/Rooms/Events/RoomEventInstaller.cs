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

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RoomEventBus>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomDealEventView>().FromInstance(_roomDealEventView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomRewardEventView>().FromInstance(_roomRewardEventView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomDealEventPresenter>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomRewardEventPresenter>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomEventService>().FromNew().AsSingle().NonLazy();
        }
    }
}