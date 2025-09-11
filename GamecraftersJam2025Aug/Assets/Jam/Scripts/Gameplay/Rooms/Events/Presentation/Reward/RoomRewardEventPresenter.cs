using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Prefabs.Gameplay.Gold;
using Jam.Scripts.Gameplay.Artifacts;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Events.Domain;
using NUnit.Framework;
using UnityEngine;
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
        [Inject] private PlayerService _playerService;
        [Inject] private GoldService _goldService;
        [Inject] private ArtifactService _artifactService;

        private RewardUiData _data;
        private List<RewardBallFieldState> _selectedBalls = new();
        private bool _isBallUpgraded = true;

        public void Initialize() => _roomEventBus.OnStartRewardEvent += OnStartRewardEvent;

        private void OnStartRewardEvent(RewardUiData data)
        {
            _data = data;
            _view.Show();
            _view.ShowRewardEvent(data);
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
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_randomRewardView, data)); break;
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

        private void ShowBallUpgradeReward(List<KeyValuePair<RewardView, IRewardCardUiData>> prefabs, BallUpgradeRewardCardUiData data)
        {
            prefabs.Clear();
            prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_ballUpgradeRewardView, data));
            _isBallUpgraded = false;
            _view.SetGetRewardButtonEnable(false);
            _view.InitializePrefabs(prefabs);
        }

        public void OnGetRewardClicked()
        {
            foreach (var rewardCardUiData in _data.Rewards)
            {
                switch (rewardCardUiData)
                {
                    case RandomBallRewardCardUiData data:
                    {
                        RewardBallFieldState state = _selectedBalls.First(state => state.RewardCardUiData == data);
                        AddBallToPlayer(state.BallType, state.Grade); 
                    } break;
                    case ConcreteBallRewardCardUiData data: AddBallToPlayer(data.BallReward.Type, data.BallReward.Grade); break;
                    case GoldRewardCardUiData data: AddGoldToPlayer(data.Value); break;
                    case HealRewardCardUiData data: HealPlayer(data.Value); break;
                    case MaxHpIncreaseRewardCardUiData data: IncreasePlayerMaxHp(data.Value); break;
                    case ArtifactRewardCardUiData data: AddArtifactToPlayer(data.Type); break;
                }  
            }
            _view.Hide();
            _view.ClearRewards();
            _roomEventBus.EventFinished();
        }

        private void AddArtifactToPlayer(ArtifactType dataType) => _artifactService.AddArtifact(dataType);
        
        private void AddGoldToPlayer(float value) => _goldService.AddGold(Mathf.RoundToInt(value));
        
        private void IncreasePlayerMaxHp(float value) => _playerService.IncreaseMaxHp(Mathf.RoundToInt(value));

        private void HealPlayer(float value) => _playerService.Heal(Mathf.RoundToInt(value));

        private void AddBallToPlayer(BallType ballType, int ballRewardGrade) => 
            _roomEventBus.BallSelected(ballType, ballRewardGrade);

        public void OnRandomBallSelected(RandomBallRewardCardUiData data, BallType ballType, int grade)
        {
            var item = _selectedBalls.First(state => state.RewardCardUiData == data);
            item.BallType = ballType;
            item.Grade = grade;
            CheckGetRewardButton();
        }

        private void CheckGetRewardButton()
        {
            if(IsButtonCanBeEnabled()) 
                _view.SetGetRewardButtonEnable(true);
        }

        public void OnBallUpgraded()
        {
            _isBallUpgraded = true;
            CheckGetRewardButton();
        }

        private bool IsButtonCanBeEnabled() => !HaveAnyRandomNotSelectedReward() && _isBallUpgraded;

        private bool HaveAnyRandomNotSelectedReward() => 
            _selectedBalls.Any(e => e.BallType == BallType.None);

        public void Dispose() => _roomEventBus.OnStartRewardEvent -= OnStartRewardEvent;
    }
}