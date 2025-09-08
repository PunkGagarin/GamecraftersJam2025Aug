using System;
using Jam.Scripts.Gameplay.Artifacts.Data;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealOnCritSystem : IArtifactSystem, IInitializable, IDisposable
    {
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private PlayerService _playerService;

        private int _healOnCritAmount;

        public void Initialize()
        {
            _battleEventBus.OnPlayerDealCritical += HandleEvent;
        }

        public void Dispose()
        {
            _battleEventBus.OnPlayerDealCritical -= HandleEvent;
        }

        public void Init(ArtifactSo data)
        {
            ArtifactHealOnCriticalSo so = data as ArtifactHealOnCriticalSo;
            
            if (so != null)
                _healOnCritAmount = so.HealAmount;
        }


        private void HandleEvent()
        {
            Execute();
        }

        public void Execute()
        {
            _playerService.Heal(_healOnCritAmount);
        }
    }
}