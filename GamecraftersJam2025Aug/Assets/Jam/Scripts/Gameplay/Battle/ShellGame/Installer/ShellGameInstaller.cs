using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.ShellGame.Installer
{
    public class ShellGameInstaller : MonoInstaller
    {

        [field: SerializeField]
        private ShellGameManagerView ShellGameManager { get; set; }

        [field: SerializeField]
        private ShellGameButtonUi GameButtonUi { get; set; }
        
        [field: SerializeField]
        private ShellGameConfig GameConfig { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShellGameManagerView>()
                .FromInstance(ShellGameManager)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGameButtonUi>()
                .FromInstance(GameButtonUi)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<ShellGameConfig>()
                .FromInstance(GameConfig)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGamePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShellGameEventBus>().AsSingle();
        }
    }
}