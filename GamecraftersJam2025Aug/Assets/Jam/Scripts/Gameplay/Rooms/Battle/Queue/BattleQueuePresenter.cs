using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class BattleQueuePresenter : IInitializable, IDisposable
    {
        [Inject] private BattleBallsQueueView _queueView;
        [Inject] private BattleQueueBus _bus;
        [Inject] private RoomRewardBus _roomRewardBus;
        [Inject] private RoomEventBus _roomEventBus; // todo chest


        public void Initialize()
        {
            _bus.OnInit += Init;
            _bus.OnNextBallsChoosen += DeactivateChoosenBalls;
            _bus.OnBallsShuffled += ShowAndReorderBalls;
            _bus.OnBallsShuffled += ShowAndReorderBalls;
            _roomRewardBus.OnRoomCompleted += CleanUp;
            _roomEventBus.OnStartDealEvent += OnStartDealEvent;
            _roomEventBus.OnStartRewardEvent += OnStartRewardEvent;
            _roomEventBus.OnEventFinished += OnEventFinished;
        }

        private void OnEventFinished() => _queueView.gameObject.SetActive(true);

        private void OnStartDealEvent(DealUiData obj) => _queueView.gameObject.SetActive(false);

        private void OnStartRewardEvent(RewardUiData obj) => _queueView.gameObject.SetActive(false);

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