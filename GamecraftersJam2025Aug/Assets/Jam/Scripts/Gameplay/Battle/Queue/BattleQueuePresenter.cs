using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Queue
{
    public class BattleQueuePresenter : IInitializable, IDisposable
    {
        [Inject] private BattleBallsQueueView _queueView;
        [Inject] private BattleQueueBus _bus;


        public void Initialize()
        {
            _bus.OnInit += Init;
            _bus.OnNextBallsChoosen += DeactivateChoosenBalls;
            _bus.OnBallsShuffled += ShowAndReorderBalls;
        }

        public void Dispose()
        {
            _bus.OnInit -= Init;
            _bus.OnNextBallsChoosen -= DeactivateChoosenBalls;
            _bus.OnBallsShuffled -= ShowAndReorderBalls;
        }

        private void Init(List<BallDto> dtos)
        {
            _queueView.InitForBalls(dtos);
        }

        private void DeactivateChoosenBalls(List<int> ids)
        {
            foreach (int id in ids)
                _queueView.HideBall(id);
        }

        private void ShowAndReorderBalls(List<int> ids)
        {
            _queueView.ShowAllBalls();
            _queueView.ReorderBalls(ids);
        }
    }
}