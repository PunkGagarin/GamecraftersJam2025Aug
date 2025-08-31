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


        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShellGameManagerView>()
                .FromInstance(ShellGameManager)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGameButtonUi>()
                .FromInstance(GameButtonUi)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGamePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShellGameEventBus>().AsSingle();
        }
    }
}