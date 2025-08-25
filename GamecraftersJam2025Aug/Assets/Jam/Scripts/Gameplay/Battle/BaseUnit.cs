using System;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BaseUnit
    {
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int Shield { get; protected set; }
        public bool IsDead { get; protected set; }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public virtual void SetIsDead(bool isDead)
        {
            IsDead = isDead;
        }

        public virtual void Heal(int healAmount)
        {
            Health += healAmount;
        }
    }
}