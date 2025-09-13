using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Inventory.Views;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class InventoryInstaller : MonoInstaller
    {

        [field: SerializeField]
        private PlayerInventoryView View { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerInventoryView>().FromInstance(View).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInventoryPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInventoryService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallDescriptionGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallsGenerator>().AsSingle();
        }
    }
}