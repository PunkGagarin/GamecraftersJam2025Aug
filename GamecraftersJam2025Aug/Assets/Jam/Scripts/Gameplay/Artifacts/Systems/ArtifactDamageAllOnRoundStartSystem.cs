using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Enemy;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactDamageAllOnRoundStartSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactDamageAllOnRoundStartSystem>
        {

        }

        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private BattleEnemyService _battleEnemyService;

        private int _damage;

        public ArtifactDamageAllOnRoundStartSystem(ArtifactSo data)
        {
            ArtifactDamageAllOnRoundStartSo so = data as ArtifactDamageAllOnRoundStartSo;

            if (so != null)
                _damage = so.Damage;
        }

        public void Initialize()
        {
            _battleEventBus.OnPlayerTurnStarted += HandleEvent;
        }

        public void Dispose()
        {
            _battleEventBus.OnPlayerTurnStarted -= HandleEvent;
        }

        private void HandleEvent()
        {
            Execute();
        }


        public void Execute()
        {
            var enemies = _battleEnemyService.GetAllEnemies();

            foreach (var enemy in enemies)
                _battleEnemyService.TakeDamage(_damage, enemy);
        }
    }
}