using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using Jam.Scripts.UI;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomRewardEventPresenter : IInitializable, IDisposable
    {
        [Inject] private RoomRewardEventView _view;
        [Inject] private RoomEventBus _roomEventBus;
        [Inject] private DefaultRewardView _defaultRewardView;
        [Inject] private BallUpgradeRewardView _ballUpgradeRewardView;
        [Inject] private ConcreteRewardView _concreteRewardView;
        [Inject] private RandomRewardView _randomRewardView;
        [Inject] private ClownMonologueController _clownMonologueController;
        [Inject] private RoomEventService _roomEventService;

        private RewardUiData _data;
        private List<RewardBallFieldState> _selectedBalls = new();

        public void Initialize() => _roomEventBus.OnStartRewardEvent += OnStartRewardEvent;

        private void OnStartRewardEvent(RewardUiData data)
        {
            _data = data;
            _view.Show();
            _view.ShowRewardEvent(data);
            ShowClownBubble(data);
        }

        private void ShowClownBubble(RewardUiData data)
        {
            if (data.ClownMonologueStrings.Count == 1)
                _clownMonologueController.ShowTextWithTimer(data.ClownMonologueStrings.First());
            else
                _clownMonologueController.ShowTextList(data.ClownMonologueStrings);
        }

        public void OnStartClicked()
        {
            List<KeyValuePair<RewardView, IRewardCardUiData>> prefabs = new();
            foreach (var rewardCardUiData in _data.Rewards)
            {
                switch (rewardCardUiData)
                {
                    case RandomBallRewardCardUiData data:
                    {
                        _view.SetGetRewardButtonEnable(false);
                        _selectedBalls.Add(new RewardBallFieldState(data, BallType.None, 0));
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_randomRewardView, data));
                        break;
                    }
                    case ConcreteBallRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_concreteRewardView, data)); break;
                    case GoldRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_defaultRewardView, data)); break;
                    case HealRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_defaultRewardView, data)); break;
                    case MaxHpIncreaseRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_defaultRewardView, data)); break;
                    case BallUpgradeRewardCardUiData data:
                    {
                        ShowBallUpgradeReward(prefabs, data);
                        return;
                    }
                    case ArtifactRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_defaultRewardView, data)); break;
                }
            }

            _view.InitializePrefabs(prefabs);
        }

        private void ShowBallUpgradeReward(List<KeyValuePair<RewardView, IRewardCardUiData>> prefabs,
            BallUpgradeRewardCardUiData data)
        {
            prefabs.Clear();
            prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_ballUpgradeRewardView, data));
            _view.InitializePrefabs(prefabs);
        }

        public void OnGetRewardClicked()
        {
            foreach (var rewardCardUiData in _data.Rewards)
            {
                if (rewardCardUiData is RandomBallRewardCardUiData data)
                {
                    RewardBallFieldState state = _selectedBalls.First(state => state.RewardCardUiData == data);
                    data.SelectedBall = state;
                    _roomEventService.ProcessReward(data);
                }
                else
                {
                    _roomEventService.ProcessReward(rewardCardUiData);
                }
            }

            _view.Hide();
            _view.ClearRewards();
            _selectedBalls = new List<RewardBallFieldState>();
            _data = null;
            _roomEventBus.EventFinished();
        }
        
        public void OnRandomBallSelected(RandomBallRewardCardUiData data, BallType ballType, int grade)
        {
            var item = _selectedBalls.First(state => state.RewardCardUiData == data);
            item.BallType = ballType;
            item.Grade = grade;
            CheckGetRewardButton();
        }

        private void CheckGetRewardButton()
        {
            if (IsButtonCanBeEnabled())
                _view.SetGetRewardButtonEnable(true);
        }

        private bool IsButtonCanBeEnabled() => !HaveAnyRandomNotSelectedReward();

        private bool HaveAnyRandomNotSelectedReward() =>
            _selectedBalls.Any(e => e.BallType == BallType.None);

        public void Dispose() => _roomEventBus.OnStartRewardEvent -= OnStartRewardEvent;
    }
}