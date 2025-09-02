using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;
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
            _eventBus.PlayerCreated(_playerModel);
        }

        public List<int> GetCurrentBattleBallIds()
        {
            return _playerModel.CurrentBallIds;
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

            healAmount = Math.Min(healAmount, maxHealth - currentHealth);

            _playerModel.Heal(healAmount);

            int afterHealHealth = _playerModel.Health;
            _eventBus.OnHealTaken.Invoke((afterHealHealth, maxHealth, healAmount));
        }

        public bool IsDead()
        {
            return _playerModel.IsDead;
        }

        public void AddBall(int ballId)
        {
            Debug.Log($"Adding ball with id {ballId}");
            _playerModel.AddBallId(ballId);
        }

        public void ClearBalls()
        {
            Debug.Log("Clearing balls");
            _playerModel.ClearBalls();
        }
    }

}