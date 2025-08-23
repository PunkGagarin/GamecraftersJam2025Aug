using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class InventoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerInventory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallsInventoryModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallsGenerator>().AsSingle();
        }
    }
}