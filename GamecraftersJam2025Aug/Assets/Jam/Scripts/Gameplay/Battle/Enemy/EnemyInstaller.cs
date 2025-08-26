using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EnemyBattlePresenter>().AsSingle();
            Container.Bind<EnemyService>().AsSingle();
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<EnemyBusEvent>().AsSingle();
        }
    }
}