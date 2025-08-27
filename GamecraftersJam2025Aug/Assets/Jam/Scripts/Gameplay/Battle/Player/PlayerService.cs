using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerService : IInitializable
    {
        [Inject] private readonly PlayerModelFactory _playerFactory;
        [Inject] private readonly PlayerEventBus _eventBus;

        private PlayerModel _playerModel;

        public void Initialize()
        {
            _playerModel = _playerFactory.CreatePlayer();

            // точно в true?
            _playerModel.SetActive(true);
            _eventBus.OnSetActive.Invoke(true);
        }

        public void TakeDamage(int damage)
        {
            _playerModel.TakeDamage(damage);

            int currentHealth = _playerModel.Health;
            int maxHealth = _playerModel.MaxHealth;
            _eventBus.OnDamageTaken.Invoke((currentHealth, maxHealth, damage));

            if (currentHealth <= 0)
            {
                _playerModel.SetIsDead(true);
                _eventBus.OnDeath.Invoke();
            }
        }

        public void Heal(int healAmount)
        {
            int currentHealth = _playerModel.Health;
            int maxHealth = _playerModel.MaxHealth;

            if (currentHealth + healAmount > maxHealth)
                healAmount = maxHealth - currentHealth;

            _playerModel.Heal(healAmount);

            int afterHealHealth = _playerModel.Health;
            _eventBus.OnHealTaken.Invoke((afterHealHealth, maxHealth, healAmount));
        }
    }
}