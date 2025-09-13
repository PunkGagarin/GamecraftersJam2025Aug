using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealOnShuffleSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactHealOnShuffleSystem>
        {

        }

        [Inject] public BattleQueueBus _queueBus;
        [Inject] public PlayerBattleService playerBattleService;

        private int _healOnShuffleAmount;

        public ArtifactHealOnShuffleSystem(ArtifactSo data)
        {
            ArtifactHealOnShuffleSo healOnShuffleSo = data as ArtifactHealOnShuffleSo;
            if (healOnShuffleSo != null)
                _healOnShuffleAmount = healOnShuffleSo.HealAmount;
        }

        public void Initialize()
        {
            _queueBus.OnBallsShuffled += HandleEvent;
        }

        public void Dispose()
        {
            _queueBus.OnBallsShuffled -= HandleEvent;
        }

        private void HandleEvent(List<int> _)
        {
            Execute();
        }

        public void Execute()
        {
            Debug.LogError("Execute");
            playerBattleService.Heal(_healOnShuffleAmount);
        }
    }
}