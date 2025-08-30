using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.ShellGame.Installer
{
    public class ShellGameInstaller : MonoInstaller
    {

        [SerializeField]
        private ShellGameManager _shellGameManager;

        [SerializeField]
        private ShellGameView _gameView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShellGameManager>()
                .FromInstance(_shellGameManager)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGameView>()
                .FromInstance(_gameView)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShellGamePresenter>().AsSingle();
        }
    }
}