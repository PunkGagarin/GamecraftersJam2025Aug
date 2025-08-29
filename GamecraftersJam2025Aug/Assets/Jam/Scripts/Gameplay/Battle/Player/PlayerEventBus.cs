using System;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerEventBus
    {
        public Action<(int currentHealth, int maxHealth, int damage)> OnDamageTaken = delegate { };
        public Action<(int currentHealth, int maxHealth, int heal)> OnHealTaken = delegate { };
        public Action OnDeath = delegate { };
        public Action<bool> OnSetActive = delegate { };
        public event Action OnAttack = delegate { };
        public event Action<PlayerModel> OnPlayerCreated = delegate { };
        
        
        public void Attack() => OnAttack.Invoke();
        public void PlayerCreated(PlayerModel player) => OnPlayerCreated.Invoke(player);
    }
}