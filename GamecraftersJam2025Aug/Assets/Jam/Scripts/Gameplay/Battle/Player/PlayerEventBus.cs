using System;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerEventBus
    {
        public Action<(int currentHealth, int maxHealth, int damage)> OnDamageTaken = delegate { };
        public Action<(int currentHealth, int maxHealth, int heal)> OnHealTaken = delegate { };
        public Action OnDeath = delegate { };
    }
}