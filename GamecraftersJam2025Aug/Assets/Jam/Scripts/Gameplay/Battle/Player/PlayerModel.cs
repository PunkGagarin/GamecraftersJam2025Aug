namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerModel : BaseUnit
    {
        private PlayerEventBus _eventBus;
        public PlayerModel(int health, PlayerEventBus bus) : base()
        {
            Health = health;
            MaxHealth = health;
            IsDead = false;
        }
        
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _eventBus.OnDamageTaken.Invoke((Health, MaxHealth, damage));
        }

        public override void SetIsDead(bool isDead)
        {
            base.SetIsDead(isDead);
            if (IsDead)
                _eventBus.OnDeath.Invoke();
        }

        public override void Heal(int healAmount)
        {
            base.Heal(healAmount);
            _eventBus.OnHealTaken.Invoke((Health, MaxHealth, healAmount));
        }
    }
}