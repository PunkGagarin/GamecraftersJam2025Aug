using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleEventBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CombatSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<AttackAckAwaiter>().AsSingle();
            
            // Container.BindInterfacesAndSelfTo<BattleInventoryPresenter>().AsSingle();
        }
    }
}