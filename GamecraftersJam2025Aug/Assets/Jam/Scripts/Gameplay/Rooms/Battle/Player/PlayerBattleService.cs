using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class PlayerBattleService : IInitializable
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
            return _playerModel.CurrentBalls.Select( el => el.Id).ToList();
        }

        public void TakeDamage(int damage)
        {
            Debug.Log($" Taking {damage} damage to player");
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
        
        public void TakeNonLethalDamage(int damage)
        {
            Debug.Log($" Taking {damage} damage to player");
            int currentHealth = _playerModel.Health;
            int maxHealth = _playerModel.MaxHealth;
            if (damage >= currentHealth) 
                damage = currentHealth - 1;
            _playerModel.TakeDamage(damage);
            _eventBus.OnDamageTaken.Invoke((currentHealth, maxHealth, damage));
        }

        public void Heal(int healAmount)
        {
            int currentHealth = _playerModel.Health;
            int maxHealth = _playerModel.MaxHealth;

            healAmount = Math.Min(healAmount, maxHealth - currentHealth);

            _playerModel.Heal(healAmount);

            int afterHealHealth = _playerModel.Health;
            Debug.Log($" Healing {healAmount} damage to player");
            _eventBus.OnHealTaken.Invoke((afterHealHealth, maxHealth, healAmount));
        }

        public void HealPercent(int healPercent)
        {
            int healPercentAmount = healPercent * _playerModel.MaxHealth / 100;
            Heal(healPercentAmount);
        }

        public void IncreaseMaxHp(int amount)
        {
            _playerModel.IncreaseMaxHealth(amount);
            _eventBus.OnHealTaken.Invoke((_playerModel.Health, _playerModel.MaxHealth, amount));
        }

        public void DecreaseMaxHp(int amount)
        {
            int currentHealth = _playerModel.Health;
            int maxHealth = _playerModel.MaxHealth;
            if (amount >= maxHealth) 
                amount = maxHealth - 1;

            _playerModel.DecreaseMaxHealth(amount);
            _eventBus.OnDamageTaken.Invoke((currentHealth, maxHealth, currentHealth));
        }

        public bool IsDead()
        {
            return _playerModel.IsDead;
        }

        public void AddBall(int ballId)
        {
            _playerModel.AddBallId(ballId);
        }
        
        public void AddBall(BallDto ball)
        {
            _playerModel.AddBallDto(ball);
            _eventBus.BallAddedInvoke(ball);
        }

        public void ClearBalls()
        {
            Debug.Log("Clearing balls");
            _playerModel.ClearBalls();
        }

    }
}