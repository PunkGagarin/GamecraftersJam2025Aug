using System;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerEventBus
    {
        public Action<(int currentHealth, int maxHealth, int damage, bool isSelfDamage)> OnDamageTaken = delegate { };
        public Action<(int currentHealth, int maxHealth, int heal)> OnHealTaken = delegate { };
        public Action OnDeath = delegate { };
        public Action<bool> OnSetActive = delegate { };
        public event Action<Guid> OnAttackStart = delegate { };
        public event Action<Guid> OnAttackEnd = delegate { };
        public event Action<PlayerModel> OnPlayerCreated = delegate { };
        public event Action<BallDto> OnBallAdded = delegate { };
        
        
        public void AttackStartInvoke(Guid id) => OnAttackStart.Invoke(id);
        public void AttackEndInvoke(Guid id) => OnAttackEnd.Invoke(id);
        public void PlayerCreated(PlayerModel player) => OnPlayerCreated.Invoke(player);
        public void BallAddedInvoke(BallDto ball) => OnBallAdded.Invoke(ball);
    }
}