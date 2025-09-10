using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Enemy
{
    public class EnemyBattlePresenter : IInitializable, IDisposable
    {
        [Inject] private BattleEnemyService _battleEnemyService;
        [Inject] private BattleConfig _battleConfig;
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private EnemyEventBus _enemyEventBus;
        [Inject] private BattleEnemyPanelUI _battleEnemyPanel;

        private Dictionary<EnemyModel, EnemyView> _currentWave = new();

        public void Initialize()
        {
            _battleEventBus.OnWaveChanged += StartNextWave;
            _enemyEventBus.OnDamageTaken += TakeDamage;
            _enemyEventBus.OnDeath += SetEnemyEventDeath;
            _enemyEventBus.OnAttackStart += StartAttackAnimation;
            _enemyEventBus.OnDamageBoosted += BoostEnemyDamage;
            _enemyEventBus.OnDamageReset += ResetDamage;
        }

        public void Dispose()
        {
            _battleEventBus.OnWaveChanged -= StartNextWave;
            _enemyEventBus.OnDamageTaken -= TakeDamage;
            _enemyEventBus.OnDeath -= SetEnemyEventDeath;
            _enemyEventBus.OnAttackStart -= StartAttackAnimation;
            _enemyEventBus.OnDamageBoosted -= BoostEnemyDamage;
            _enemyEventBus.OnDamageReset -= ResetDamage;
        }

        private void TakeDamage((EnemyModel unit, int damage, int currentHealth, int maxHealth) info)
        {
            EnemyView view = _currentWave[info.unit];
            view.SetHealth(info.currentHealth, info.maxHealth);
            view.SetDamageText(info.damage);
        }

        private void StartNextWave((int newWaveNumber, List<EnemyModel> enemies) waveInfo)
        {
            Debug.Log($"Сетим вью врагов для {waveInfo.enemies.Count}");
            _currentWave = new();
            var enemyViews = _battleEnemyPanel.EnemyViews;
            int viewCount = enemyViews.Count;

            for (int index = 0; index < waveInfo.enemies.Count; index++)
            {
                if (index >= viewCount)
                {
                    Debug.LogError("врагов в волне больше чем прокинутых UI врага!");
                    break;
                }

                EnemyModel enemy = waveInfo.enemies[index];
                var view = enemyViews[index];
                _currentWave.Add(enemy, view);
                view.gameObject.SetActive(true);
                view.Init(enemy.MaxHealth, enemy.CurrentDamage);
                view.SetSprite(enemy.EnemySprite);
            }
        }

        private void SetEnemyEventDeath(EnemyModel enemy)
        {
            var view = _currentWave[enemy];
            view.gameObject.SetActive(false);
        }

        private async void StartAttackAnimation(Guid id, EnemyModel enemyToAttack)
        {
            var view = _currentWave[enemyToAttack];
            await view.PlayAttackAnimation();
            _enemyEventBus.InvokeAttackEnd(id);
        }

        private void BoostEnemyDamage(EnemyModel enemy, int boostedDamage)
        {
            var view = _currentWave[enemy];
            view.SetAttackTextWithAnimation(boostedDamage);
        }

        private void ResetDamage(EnemyModel enemy, int damage)
        {
            var view = _currentWave[enemy];
            view.SetAttackText(damage);
        }
    }
}