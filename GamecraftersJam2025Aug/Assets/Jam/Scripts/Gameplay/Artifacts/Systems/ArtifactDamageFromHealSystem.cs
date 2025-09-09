using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Enemy;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactDamageFromHealSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactDamageFromHealSystem>
        {

        }

        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private BattleEnemyService _enemyService;

        public ArtifactDamageFromHealSystem(ArtifactSo data)
        {
            ArtifactDamageFromHealSo so = data as ArtifactDamageFromHealSo;

            if (so != null)
                _healIncreaseAmount = so.DamagePercent;
        }

        private int _healIncreaseAmount;

        public void Initialize()
        {
            _battleEventBus.OnHeal += HandleEvent;
        }

        public void Dispose()
        {
            _battleEventBus.OnHeal -= HandleEvent;
        }

        private void HandleEvent(int heal)
        {
            var firstEnemy = _enemyService.GetFirstEnemy();
            if (firstEnemy.Count <= 0)
                return;

            int damageAmount = Mathf.Max(1, heal * _healIncreaseAmount / 100);
            _enemyService.TakeDamage(damageAmount, firstEnemy[0]);
        }
    }
}