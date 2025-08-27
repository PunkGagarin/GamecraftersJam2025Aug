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
        [Inject] private BattleEnemyPanelUI _battleEnemyPanel;

        private Dictionary<EnemyModel, EnemyView> _currentWave = new();

        public void Initialize()
        {
            _battleEventBus.OnWaveChanged += StartNextWave;
        }

        public void Dispose()
        {
            _battleEventBus.OnWaveChanged -= StartNextWave;
        }

        private void StartNextWave((int newWaveNumber, List<EnemyModel> enemies) waveInfo)
        {
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
                view.Init(enemy.EnemySprite, enemy.MaxHealth);
            }
        }

        private void OnEnemyTakeDamage(EnemyModel model, int damage, int currentHealt, int maxHealth)
        {
            var view = _currentWave[model];
            view.SetDamage(damage);
            view.UpdateHealth(currentHealt, maxHealth);
        }
    }
}