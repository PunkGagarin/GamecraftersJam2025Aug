using System;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealOnCritSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactHealOnCritSystem>
        {

        }
        
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private PlayerService _playerService;

        private int _healOnCritAmount;

        public ArtifactHealOnCritSystem(ArtifactSo data)
        {
            ArtifactHealOnCriticalSo so = data as ArtifactHealOnCriticalSo;
            
            if (so != null)
                _healOnCritAmount = so.HealAmount;
        }

        public void Initialize()
        {
            _battleEventBus.OnPlayerDealCritical += HandleEvent;
        }

        public void Dispose()
        {
            _battleEventBus.OnPlayerDealCritical -= HandleEvent;
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