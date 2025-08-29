using Jam.Scripts.Gameplay.Battle.ShellGame;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleInstaller : MonoInstaller
    {
        [field: SerializeField]
        private ShellGameView ShellGameView { get; set; }
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShellGamePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleEventBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<CombatSystem>().AsSingle();
            
            Container.Bind<ShellGameView>().FromInstance(ShellGameView).AsSingle();
            
            // Container.BindInterfacesAndSelfTo<BattleInventoryPresenter>().AsSingle();
        }
    }
}