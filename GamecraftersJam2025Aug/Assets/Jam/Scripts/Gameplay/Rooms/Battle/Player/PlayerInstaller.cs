using Jam.Scripts.Gameplay.Battle.Player;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [field: SerializeField]
        private PlayerBattleView BattleView { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerBattlePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerBattleService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerEventBus>().AsSingle().NonLazy();

            Container.Bind<PlayerModelFactory>().AsSingle();

            Container.Bind<PlayerBattleView>().FromInstance(BattleView).AsSingle();
        }
    }
}