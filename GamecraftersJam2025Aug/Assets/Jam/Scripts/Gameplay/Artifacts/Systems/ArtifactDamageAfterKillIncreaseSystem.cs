using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Rooms;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactDamageAfterKillIncreaseSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactDamageAfterKillIncreaseSystem>
        {

        }

        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private EnemyEventBus _enemyBus;
        [Inject] private RoomRewardBus _rewardBus;

        private int _damageIncreaseAmount;
        private OnBeforeDamageDto _dto;
        private int _currentDamageIncrease;

        public ArtifactDamageAfterKillIncreaseSystem(ArtifactSo data)
        {
            ArtifactDamageAfterKillIncreaseSo so = data as ArtifactDamageAfterKillIncreaseSo;

            if (so != null)
                _damageIncreaseAmount = so.DamageIncrease;
        }

        public void Initialize()
        {
            _enemyBus.OnDeath += HandleEvent;
            _rewardBus.OnRoomCompleted += ResetDamage;
            _battleEventBus.OnBeforeDamage += IncreaseDamage;
        }

        public void Dispose()
        {
            _enemyBus.OnDeath -= HandleEvent;
            _rewardBus.OnRoomCompleted -= ResetDamage;
            _battleEventBus.OnBeforeDamage -= IncreaseDamage;
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

        private void HandleEvent(EnemyModel obj)
        {
            Execute();
        }


        public void Execute()
        {
            _currentDamageIncrease += _damageIncreaseAmount;
        }
    }
}