using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Queue;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class BattleQueuePresenter : IInitializable, IDisposable
    {
        [Inject] private BattleBallsQueueView _queueView;
        [Inject] private BattleQueueBus _bus;
        [Inject] private RoomRewardBus _roomRewardBus;


        public void Initialize()
        {
            _bus.OnInit += Init;
            _bus.OnNextBallsChoosen += DeactivateChoosenBalls;
            _bus.OnBallsShuffled += ShowAndReorderBalls;
            _bus.OnBallsShuffled += ShowAndReorderBalls;
            _roomRewardBus.OnRoomCompleted += CleanUp;
        }

        public void Dispose()
        {
            _bus.OnInit -= Init;
            _bus.OnNextBallsChoosen -= DeactivateChoosenBalls;
            _bus.OnBallsShuffled -= ShowAndReorderBalls;
            _roomRewardBus.OnRoomCompleted -= CleanUp;
        }

        private void CleanUp()
        {
            _queueView.CleanUp();
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