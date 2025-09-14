using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Rooms;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactDamageAfterQueueShuffleIncreaseSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactDamageAfterQueueShuffleIncreaseSystem>
        {

        }

        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private RoomRewardBus _rewardBus;
        [Inject] private BattleQueueBus _queueBus;

        private int _damageIncreaseAmount;
        private OnBeforeDamageDto _dto;
        private int _currentDamageIncrease;
        private int _shuffleCount;

        public ArtifactDamageAfterQueueShuffleIncreaseSystem(ArtifactSo data)
        {
            ArtifactDamageAfterQueueShuffleIncreaseSo so = data as ArtifactDamageAfterQueueShuffleIncreaseSo;

            if (so != null)
                _damageIncreaseAmount = so.DamageIncrease;
        }

        public void Initialize()
        {
            _queueBus.OnBallsShuffled += SetDamageIncrease;
            _rewardBus.OnRoomCompleted += Reset;
            _battleEventBus.OnBeforeDamage += IncreaseDamage;
        }

        public void Dispose()
        {
            _queueBus.OnBallsShuffled -= SetDamageIncrease;
            _rewardBus.OnRoomCompleted -= Reset;
            _battleEventBus.OnBeforeDamage -= IncreaseDamage;
        }

        private void Reset()
        {
            ResetDamage();
            ShuffleReset();
        }

        private void ResetDamage()
        {
            _currentDamageIncrease = 0;
        }

        private void IncreaseDamage(OnBeforeDamageDto dto)
        {
            dto.DamageAmount += _currentDamageIncrease;
            ResetDamage();
        }

        private void ShuffleReset()
        {
            _shuffleCount = 0;
        }

        private void SetDamageIncrease(List<int> _)
        {
            if (_shuffleCount > 0)
                _currentDamageIncrease += _damageIncreaseAmount;
            
            _shuffleCount++;
        }
    }
}