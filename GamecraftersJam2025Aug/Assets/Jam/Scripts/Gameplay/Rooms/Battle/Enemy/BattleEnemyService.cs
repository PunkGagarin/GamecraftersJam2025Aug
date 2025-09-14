using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Enemy
{
    public class BattleEnemyService : IInitializable, IDisposable

    {
        [Inject] private EnemyFactory _enemyFactory;
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private EnemyEventBus _enemyEventBus;
        [Inject] private EnemyConfigRepository _enemyConfig;
        [Inject] private AttackAckAwaiter _waiter;


        private BattleWaveModel _battleWaveModel;


        public void CreateEnemiesFor(RoomBattleConfig room)
        {
            _battleWaveModel = _enemyFactory.CreateBattleWaveModel(room);
        }

        public void IncrementWave()
        {
            _battleWaveModel.IncrementWave();
            int nextWave = _battleWaveModel.CurrentBattleWave;
            int totalWaves = _battleWaveModel.Enemies.Keys.Count;
            var nextWaveEnemies = _battleWaveModel.Enemies[nextWave];

            _battleEventBus.WaveChangedInvoke((nextWave, nextWaveEnemies, totalWaves));
        }

        public void CleanUpBattleData()
        {
            _battleWaveModel = null;
        }

        public List<EnemyModel> GetFirstEnemy()
        {
            return new List<EnemyModel>() { GetAliveEnemiesForCurrentWave()?[0] };
        }

        public List<EnemyModel> GetAllEnemies()
        {
            return GetAliveEnemiesForCurrentWave();
        }

        public List<EnemyModel> GetRandomEnemy()
        {
            var aliveEnemies = GetAliveEnemiesForCurrentWave();
            var randomEnemy = aliveEnemies?[Random.Range(0, aliveEnemies.Count)];
            return new List<EnemyModel> { randomEnemy };
        }

        public List<EnemyModel> GetLastEnemy()
        {
            return new List<EnemyModel> { GetAliveEnemiesForCurrentWave()?[^1] };
        }

        public List<EnemyModel> GetAliveEnemiesForCurrentWave()
        {
            int currentBattleWave = _battleWaveModel.CurrentBattleWave;
            if (!_battleWaveModel.Enemies.TryGetValue(currentBattleWave, out var enemies))
                Debug.LogError($" пытаемся получить врагов волны {currentBattleWave} но её или врагов нет");
            else
                enemies.RemoveAll(enemy => enemy.IsDead);

            return enemies;
        }

        public async UniTask TakeDamage(int damage, EnemyModel enemy)
        {
            Debug.Log($" Dealing {damage} damage to {enemy.Type}");
            enemy.TakeDamage(damage);
            _enemyEventBus.InvokeDamageTaken(enemy, damage, enemy.Health, enemy.MaxHealth);

            int currentHealth = enemy.Health;

            if (currentHealth <= 0)
            {
                var guid = Guid.NewGuid();
                _enemyEventBus.InvokeStartDeath(enemy, guid);
                await _waiter.Wait(guid);
                enemy.SetIsDead(true);
            }
        }

        public void Heal(int healAmount, EnemyModel enemy)
        {
            int currentHealth = enemy.Health;
            int maxHealth = enemy.MaxHealth;

            healAmount = Math.Min(healAmount, maxHealth - currentHealth);

            enemy.Heal(healAmount);

            int afterHealHealth = enemy.Health;
            Debug.Log($" Healing {healAmount} damage to {enemy.Type}");
            _enemyEventBus.InvokeHealTaken(enemy, afterHealHealth, maxHealth, healAmount);
        }

        private void OnEndEnemyDeath(EnemyModel enemy, Guid _)
        {
            _battleWaveModel.RemoveDeadEnemy(enemy);
            _enemyEventBus.InvokeDeath(enemy);
        }

        public bool IsAnyEnemyAlive()
        {
            int currentBattleWave = _battleWaveModel.CurrentBattleWave;
            if (_battleWaveModel.Enemies.TryGetValue(currentBattleWave, out var enemies))
                return enemies.Count > 0 && enemies.Any(enemy => !enemy.IsDead);
            else
                return false;
        }

        public bool IsNextWave()
        {
            int currentBattleWave = _battleWaveModel.CurrentBattleWave;
            return _battleWaveModel.Enemies.TryGetValue(currentBattleWave + 1, out var enemies);
        }

        public void BoostAllEnemies()
        {
            var enemies = GetAliveEnemiesForCurrentWave();
            foreach (var enemy in enemies)
            {
                BoostEnemy(enemy);
            }
        }

        private void BoostEnemy(EnemyModel enemy)
        {
            int boost = _enemyConfig.EnemyBallBoost;
            int boostedDamage = enemy.CurrentDamage + enemy.Damage * boost / 100;
            enemy.SetCurrentDamage(boostedDamage);
            _enemyEventBus.InvokeDamageBoosted(enemy, boostedDamage);
        }

        public void ResetDamage()
        {
            var enemies = GetAliveEnemiesForCurrentWave();
            foreach (var enemy in enemies)
            {
                ResetDamageFor(enemy);
            }
        }

        private void ResetDamageFor(EnemyModel enemy)
        {
            enemy.SetCurrentDamage(enemy.Damage);
            _enemyEventBus.InvokeDamageReset(enemy, enemy.Damage);
        }

        public void Initialize()
        {
            _enemyEventBus.OnEndEnemyDeath += OnEndEnemyDeath;
        }

        public void Dispose()
        {
            _enemyEventBus.OnEndEnemyDeath -= OnEndEnemyDeath;
        }
    }
}