using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyBattlePresenter : IInitializable, IDisposable
    {
        [Inject] private BattleEnemyService _battleEnemyService;
        [Inject] private BattleConfig _battleConfig;
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private EnemyBusEvent _enemyBusEvent;
        [Inject] private BattleEnemyPanelUI _battleEnemyPanel;

        private Dictionary<EnemyModel, EnemyView> _currentWave = new();

        public void Initialize()
        {
            Debug.Log($" EnemyBattlePresenter initializing");
            _battleEventBus.OnWaveChanged += StartNextWave;
            _enemyBusEvent.OnDamageTaken += UpdateEnemyView;
            _enemyBusEvent.OnDeath +=  SetEnemyDeath;
        }

        public void Dispose()
        {
            _battleEventBus.OnWaveChanged -= StartNextWave;
            _enemyBusEvent.OnDamageTaken -= UpdateEnemyView;
            _enemyBusEvent.OnDeath -= SetEnemyDeath;
        }

        private void UpdateEnemyView((EnemyModel unit, int damage, int currentHealth, int maxHealth) info)
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
                view.Init(enemy.MaxHealth);
                view.SetSprite(enemy.EnemySprite);
            }
        }

        private void OnEnemyTakeDamage(EnemyModel model, int damage, int currentHealt, int maxHealth)
        {
            var view = _currentWave[model];
            view.SetDamageText(damage);
            view.UpdateHealth(currentHealt, maxHealth);
        }

        private void SetEnemyDeath(EnemyModel enemy)
        {
            var view = _currentWave[enemy];
            view.gameObject.SetActive(false);
        }
    }
}