namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyModel : BaseUnit
    {
        private readonly EnemyBusEvent _eventBus;
        public int Damage { get; private set; }
        public EnemyType Type { get; private set; }

        public EnemyModel(int damage, int health, EnemyType type, EnemyBusEvent eventBus)
        {
            _eventBus = eventBus;
            Damage = damage;
            Health = health;
            Type = type;
        }
        
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _eventBus.OnDamageTaken.Invoke((this,Health, MaxHealth, damage));
        }

        public override void SetIsDead(bool isDead)
        {
            base.SetIsDead(isDead);
            if (IsDead)
                _eventBus.OnDeath.Invoke(this);
        }

        public override void Heal(int healAmount)
        {
            base.Heal(healAmount);
            _eventBus.OnHealTaken.Invoke((this,Health, MaxHealth, healAmount));
        }
    }
}