using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerUnitService : IInitializable
    {
        [Inject] private readonly PlayerModelFactory _playerFactory;
        
        private PlayerModel _playerModel;

        public void Initialize()
        {
            Debug.Log("Player unit initializing");
            _playerModel = _playerFactory.CreatePlayer();
            Debug.Log("Player unit initialized");
        }

        public void TakeDamage(int damage)
        {
            _playerModel.TakeDamage(damage);

            var health = _playerModel.Health;
            if (health <= 0)
                _playerModel.SetIsDead(true);
        }

        public void Heal(int healAmount)
        {
            var currentHealth = _playerModel.Health;
            var maxHealth = _playerModel.MaxHealth;
            
            if (currentHealth + healAmount > maxHealth)
                healAmount = maxHealth - currentHealth;

            _playerModel.Heal(healAmount);
        }
    }
}