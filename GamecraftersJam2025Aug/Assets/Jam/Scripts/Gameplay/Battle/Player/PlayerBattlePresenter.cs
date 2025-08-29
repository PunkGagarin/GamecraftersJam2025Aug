using System;
using Jam.Scripts.Gameplay.Battle.Player.Ui;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerBattlePresenter : IInitializable, IDisposable
    {
        [Inject] private PlayerEventBus _playerEventBus;
        [Inject] private PlayerBattleView _view;

        public void Initialize()
        {
            _playerEventBus.OnPlayerCreated += InitView;
            _playerEventBus.OnDeath += ShowDeath;
            _playerEventBus.OnDamageTaken += ShowDamageTaken;
            _playerEventBus.OnHealTaken += ShowHeal;
            _playerEventBus.OnSetActive += SetActive;
            _playerEventBus.OnAttack += StartAttackAnimation;
        }

        public void Dispose()
        {
            _playerEventBus.OnPlayerCreated -= InitView;
            _playerEventBus.OnDeath -= ShowDeath;
            _playerEventBus.OnDamageTaken -= ShowDamageTaken;
            _playerEventBus.OnHealTaken -= ShowHeal;
            _playerEventBus.OnSetActive -= SetActive;
            _playerEventBus.OnAttack -= StartAttackAnimation;
        }

        private void ShowHeal((int currentHealth, int maxHealth, int heal) parameters)
        {
            _view.ShowHeal(parameters.currentHealth, parameters.maxHealth, parameters.heal);
        }

        private void ShowDamageTaken((int currentHealth, int maxHealth, int damage) parameters)
        {
            _view.ShowDamageTaken(parameters.currentHealth, parameters.maxHealth, parameters.damage);
        }

        private void ShowDeath()
        {
            _view.ShowDeath();
        }

        private void SetActive(bool isActive)
        {
            _view.gameObject.SetActive(isActive);
        }

        private void StartAttackAnimation()
        {
            Debug.Log("Скоро добавим анимацию атаки");
        }

        private void InitView(PlayerModel player)
        {
            _view.Init(player.MaxHealth);
        }
    }
}