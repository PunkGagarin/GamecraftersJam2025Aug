using System;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyBusEvent
    {
        public Action<(EnemyModel unit,int currentHealth, int maxHealth, int damage)> OnDamageTaken = delegate { };
        public Action<(EnemyModel unit,int currentHealth, int maxHealth, int heal)> OnHealTaken = delegate { };
        public Action<EnemyModel> OnDeath = delegate { };
    }
}