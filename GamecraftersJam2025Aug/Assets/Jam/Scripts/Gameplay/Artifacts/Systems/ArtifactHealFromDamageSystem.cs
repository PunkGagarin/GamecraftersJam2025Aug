using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{

    public class ArtifactHealFromDamageSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactHealFromDamageSystem>
        {

        }

        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private PlayerService _playerService;

        public ArtifactHealFromDamageSystem(ArtifactSo data)
        {
            ArtifactHealFromDamageSo so = data as ArtifactHealFromDamageSo;

            if (so != null)
                _healIncreaseAmount = so.HealPercent;
        }

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
            int healAmount = Mathf.Max(1, damage * _healIncreaseAmount / 100);
            _playerService.Heal(healAmount);
        }
    }
}