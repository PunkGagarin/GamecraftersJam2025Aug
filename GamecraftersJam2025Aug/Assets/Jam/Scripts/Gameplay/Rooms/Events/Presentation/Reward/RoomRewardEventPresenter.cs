using System;
using System.Collections.Generic;
using System.Linq;
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
        [Inject] private DefaultRewardView _defaultrewardView;
        [Inject] private ConcreteRewardView _concreteRewardView;
        [Inject] private RandomRewardView _randomRewardView;
        [Inject] private PlayerService _playerService;

        private RewardUiData _data;
        private Dictionary<IRewardCardUiData, BallType> _selectedBallsType = new();

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
                        _selectedBallsType.Add(data, BallType.None);
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_randomRewardView, data)); break;
                    }
                    case ConcreteBallRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_concreteRewardView, data)); break;
                    case GoldRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_defaultrewardView, data)); break;
                    case HealRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_defaultrewardView, data)); break;
                    case MaxHpIncreaseRewardCardUiData data:
                        prefabs.Add(new KeyValuePair<RewardView, IRewardCardUiData>(_defaultrewardView, data)); break;
                }
            }
            
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
                        BallType ballType = _selectedBallsType[data];
                        AddBallToPLayer(ballType); 
                    } break;
                    case ConcreteBallRewardCardUiData data: AddBallToPLayer(data.BallReward.Type); break;
                    case GoldRewardCardUiData data: AddGoldToPlayer(data.Value); break;
                    case HealRewardCardUiData data: HealPlayer(data.Value); break;
                    case MaxHpIncreaseRewardCardUiData data: IncreasePlayerMaxHp(data.Value); break;
                }  
            }
            _view.Hide();
            _view.ClearRewards();
            _roomEventBus.EventFinished();
        }


        private void AddGoldToPlayer(float value)
        {
            //tbd
        }
        private void IncreasePlayerMaxHp(float value) => _playerService.IncreaseMaxHp(Mathf.RoundToInt(value));

        private void HealPlayer(float value) => _playerService.Heal(Mathf.RoundToInt(value));

        private void AddBallToPLayer(BallType ballType) => _roomEventBus.BallSelected(ballType);

        public void OnRandomBallSelected(RandomBallRewardCardUiData data, BallType ballType)
        {
            _selectedBallsType[data] = ballType;
            if(!HaveAnyRandomNotSelectedReward()) 
                _view.SetGetRewardButtonEnable(true);
        }

        private bool HaveAnyRandomNotSelectedReward() => 
            _selectedBallsType.Any(e => e.Value == BallType.None);

        public void Dispose() => _roomEventBus.OnStartRewardEvent -= OnStartRewardEvent;
    }
}