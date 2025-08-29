using System;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyBusEvent
    {
        public event Action<(EnemyModel unit, int currentHealth, int maxHealth, int damage)> OnDamageTaken = delegate
        {
        };

        public event Action<(EnemyModel unit, int currentHealth, int maxHealth, int heal)> OnHealTaken = delegate { };
        public event Action<EnemyModel> OnDeath = delegate { };

        public void InvokeDamageTaken(EnemyModel unit, int currentHealth, int maxHealth, int damage) =>
            OnDamageTaken.Invoke((unit, currentHealth, maxHealth, damage));

        public void InvokeHealTaken(EnemyModel unit, int currentHealth, int maxHealth, int heal) =>
            OnHealTaken.Invoke((unit, currentHealth, maxHealth, heal));

        public void InvokeDeath(EnemyModel unit) => OnDeath.Invoke(unit);
    }
}