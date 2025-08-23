using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattlePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleInventoryPresenter>().AsSingle();
        }
    }
}