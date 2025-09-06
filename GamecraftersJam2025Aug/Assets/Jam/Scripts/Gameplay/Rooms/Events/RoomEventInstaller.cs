using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public class RoomEventInstaller : MonoInstaller
    {
        [SerializeField] private RoomEventView roomEventView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RoomEventBus>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomEventView>().FromInstance(roomEventView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomEventPresenter>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RoomEventService>().FromNew().AsSingle().NonLazy();
        }
    }
}