using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactDamageIncreaseSystem : IArtifactSystem
    {
        public class ArtifactFactory : BaseArtifactFactory<ArtifactDamageIncreaseSystem>
        {

        }
        
        [Inject] private BattleEventBus _battleEventBus;

        private int _damageIncreaseAmount;
        private OnBeforeDamageDto _dto;

        public ArtifactDamageIncreaseSystem(ArtifactSo data)
        {
            ArtifactDamageIncreaseSo so = data as ArtifactDamageIncreaseSo;

            if (so != null)
                _damageIncreaseAmount = so.DamageIncrease;
        }

        public void Initialize()
        {
            _battleEventBus.OnBeforeDamage += HandleEvent;
        }

        public void Dispose()
        {
            _battleEventBus.OnBeforeDamage -= HandleEvent;
        }

        private void HandleEvent(OnBeforeDamageDto dto)
        {
            _dto = dto;
            Execute();
        }


        public void Execute()
        {
            _dto.DamageAmount += _damageIncreaseAmount;
        }
    }
}