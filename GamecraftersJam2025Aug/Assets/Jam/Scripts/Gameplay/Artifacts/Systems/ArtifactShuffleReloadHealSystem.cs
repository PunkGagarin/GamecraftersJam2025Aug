using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactShuffleReloadHealSystem : IArtifactSystem, IInitializable, IDisposable
    {

        [Inject] public BattleQueueBus _queueBus;
        [Inject] public PlayerService _playerService;

        private int _healOnShuffleAmount;

        public void Initialize()
        {
            _queueBus.OnBallsShuffled += HandleEvent;
        }

        public void Dispose()
        {
            _queueBus.OnBallsShuffled -= HandleEvent;
        }

        public void Init(ArtifactSo data)
        {
            ArtifactHealOnShuffleSo healOnShuffleSo = data as ArtifactHealOnShuffleSo;
            if (healOnShuffleSo != null)
                _healOnShuffleAmount = healOnShuffleSo.HealAmount;
        }

        private void HandleEvent(List<int> _)
        {
            Execute();
        }

        public void Execute()
        {
            Debug.LogError("Execute");
            _playerService.Heal(_healOnShuffleAmount);
        }
    }
}