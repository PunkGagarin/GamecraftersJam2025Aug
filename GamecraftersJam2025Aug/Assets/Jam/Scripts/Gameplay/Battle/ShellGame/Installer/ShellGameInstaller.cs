using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.ShellGame.Installer
{
    public class ShellGameInstaller : MonoInstaller
    {

        [field: SerializeField]
        private ShellGameView ShellGame { get; set; }

        [field: SerializeField]
        private ShellGameButtonUi GameButtonUi { get; set; }


        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShellGameView>()
                .FromInstance(ShellGame)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGameButtonUi>()
                .FromInstance(GameButtonUi)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGamePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShellGameEventBus>().AsSingle();
        }
    }
}