using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealFromDamageSystem : IArtifactSystem
    {
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private PlayerService _playerService;

        private int _healIncreaseAmount;

        public void Initialize()
        {
            _battleEventBus.OnAfterDamage += HandleEvent;
        }

        public void Dispose()
        {
            _battleEventBus.OnAfterDamage -= HandleEvent;
        }

        private void HandleEvent(int damage)
        {
            int healAmount = damage * _healIncreaseAmount / 100;
            _playerService.Heal(healAmount);
        }

        public void Init(ArtifactSo data)
        {
            ArtifactHealFromDamageSo so = data as ArtifactHealFromDamageSo;

            if (so != null)
                _healIncreaseAmount = so.HealPercent;
        }


        public void Execute()
        {
            _playerService.Heal(_healIncreaseAmount);
        }
    }
}