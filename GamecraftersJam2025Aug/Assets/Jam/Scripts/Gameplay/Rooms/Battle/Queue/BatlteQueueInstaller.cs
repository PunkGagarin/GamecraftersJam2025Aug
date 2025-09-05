using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Queue
{
    public class BatlteQueueInstaller : MonoInstaller
    {
        [SerializeField]
        private BattleBallsQueueView _view;

        public override void InstallBindings()
        {
            Container.Bind<BattleBallsQueueView>().FromInstance(_view).AsSingle();
            Container.Bind<BattleQueueBus>().AsSingle();

            Container.BindInterfacesAndSelfTo<BattleQueuePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleQueueService>().AsSingle();
        }
    }
}