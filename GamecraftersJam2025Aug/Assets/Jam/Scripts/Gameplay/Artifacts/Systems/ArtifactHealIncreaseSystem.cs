using Jam.Scripts.Gameplay.Rooms.Battle;
using Zenject;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactHealIncreaseSystem : IArtifactSystem
    {
        [Inject] private BattleEventBus _battleEventBus;

        private int _healIncreaseAmount;
        private OnHealDto _onHealDto;

        public void Initialize()
        {
            _battleEventBus.OnBeforeHeal += HandleEvent;
        }

        public void Dispose()
        {
            _battleEventBus.OnBeforeHeal -= HandleEvent;
        }

        public void Init(ArtifactSo data)
        {
            ArtifactHealIncreaseSo so = data as ArtifactHealIncreaseSo;

            if (so != null)
                _healIncreaseAmount = so.IncreaseAmount;
        }

        private void HandleEvent(OnHealDto dto)
        {
            _onHealDto = dto;
            Execute();
        }


        public void Execute()
        {
            _onHealDto.HealAmount += _healIncreaseAmount;
        }
    }
}