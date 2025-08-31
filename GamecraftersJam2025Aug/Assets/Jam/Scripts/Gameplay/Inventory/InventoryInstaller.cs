using Jam.Scripts.Gameplay.Inventory.Models;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class InventoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerInventoryPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallsGenerator>().AsSingle();
        }
    }
}