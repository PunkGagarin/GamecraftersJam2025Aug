using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class BattleEnemyService
    {
        [Inject] private EnemyFactory _enemyFactory;
        [Inject] private BattleEventBus _battleEventBus;
        [Inject] private EnemyEventBus _enemyEventBus;


        private BattleWaveModel _battleWaveModel;


        //вызывается оркестратором битвы
        public void CreateEnemiesFor(RoomBattleConfig room)
        {
            _battleWaveModel = _enemyFactory.CreateBattleWaveModel(room);
        }

        public void IncrementWave()
        {
            _battleWaveModel.IncrementWave();
            int nextWave = _battleWaveModel.CurrentBattleWave;
            var nextWaveEnemies = _battleWaveModel.Enemies[nextWave];

            _battleEventBus.WaveChangedInvoke((nextWave, nextWaveEnemies));
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

        public List<EnemyModel> GetLastEnemy()
        {
            return new List<EnemyModel>() { GetAliveEnemiesForCurrentWave()?[^1] };
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

        public void DealDamage(int damage, EnemyModel enemy)
        {
            enemy.TakeDamage(damage);
            _enemyEventBus.InvokeDamageTaken(enemy, damage, enemy.Health, enemy.MaxHealth);

            int currentHealth = enemy.Health;

            if (currentHealth <= 0)
                SetDeath(enemy);
        }

        private void SetDeath(EnemyModel enemy)
        {
            enemy.SetIsDead(true);
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
    }
}