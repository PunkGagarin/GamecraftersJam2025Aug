using System;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BaseUnit
    {
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Shield { get; private set; }
        public bool IsDead { get; private set; }


        public event Action<(int currentHealth, int maxHealth, int damage)> OnDamageTaken = delegate { };
        public event Action<(int currentHealth, int maxHealth, int heal)> OnHealTaken = delegate { };

        public event Action OnDead = delegate { };

        public void TakeDamage(int damage)
        {
            Health -= damage;
            OnDamageTaken.Invoke((Health, MaxHealth, damage));
        }

        public void SetIsDead(bool isDead)
        {
            IsDead = isDead;
            if (IsDead)
                OnDead.Invoke();
        }

        public void Heal(int healAmount)
        {
            Health += healAmount;
            OnHealTaken.Invoke((Health, MaxHealth, healAmount));
        }
    }
}