using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        [Inject] private BattleEnemyPanelUI _view;

        private Dictionary<EnemyModel, EnemyView> _currentWave = new();

        public void Initialize()
        {
            _battleEventBus.OnWaveChanged += StartNextWave;
            _enemyEventBus.OnDamageTaken += TakeDamage;
            _enemyEventBus.OnStartEnemyDeath += StartEnemyDeath;
            _enemyEventBus.OnAttackStart += StartAttackAnimation;
            _enemyEventBus.OnDamageBoosted += BoostEnemyDamage;
            _enemyEventBus.OnDamageReset += ResetDamage;
        }
        
        public void Dispose()
        {
            _battleEventBus.OnWaveChanged -= StartNextWave;
            _enemyEventBus.OnDamageTaken -= TakeDamage;
            _enemyEventBus.OnStartEnemyDeath -= StartEnemyDeath;
            _enemyEventBus.OnAttackStart -= StartAttackAnimation;
            _enemyEventBus.OnDamageBoosted -= BoostEnemyDamage;
            _enemyEventBus.OnDamageReset -= ResetDamage;
        }

        private void TakeDamage((EnemyModel unit, int damage, int currentHealth, int maxHealth) info)
        {
            EnemyView view = _currentWave[info.unit];
            view.SetHealth(info.currentHealth, info.maxHealth);
            view.SetDamageText(info.damage);
            view.PlayDamageAnimation();
        }

        private void StartNextWave((int newWaveNumber, List<EnemyModel> enemies, int totalWaves) waveInfo)
        {
            Debug.Log($"Сетим вью врагов для {waveInfo.enemies.Count}");
            _currentWave = new();
            var enemyViews = _view.EnemyViews;
            int viewCount = enemyViews.Count;
            _view.SetWaveText(waveInfo.newWaveNumber, waveInfo.totalWaves);

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
                view.PrepareStartPosition();
                view.gameObject.SetActive(true);
                view.Init(enemy.MaxHealth, enemy.CurrentDamage);
                view.SetEnemyGraphic(enemy.EnemyGraphic);
            }
        }
        
        private void StartEnemyDeath(EnemyModel enemy)
        {
            _ = SetEnemyDead(enemy);
        }

        
        private async UniTask SetEnemyDead(EnemyModel enemy)
        {
            var view = _currentWave[enemy];
            await view.PlayDeathAnimation();
            view.gameObject.SetActive(false);
            _enemyEventBus.InvokeEndEnemyDeath(enemy);
            Debug.Log("Enemy died");
        }

        private async void StartAttackAnimation(Guid id, EnemyModel enemyToAttack)
        {
            try
            {
                var view = _currentWave[enemyToAttack];
                await view.PlayAttackAnimation();
                _enemyEventBus.InvokeAttackEnd(id);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
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