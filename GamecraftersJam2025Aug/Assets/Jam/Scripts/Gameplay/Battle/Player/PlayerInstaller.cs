using Jam.Scripts.Gameplay.Player;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerBattlePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerModelFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerUnitService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerEventBus>().AsSingle();
            // Container.BindInterfacesAndSelfTo<PlayerBattleView>().AsSingle();
        }
        
    }
}