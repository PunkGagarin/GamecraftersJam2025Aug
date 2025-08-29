using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Inventory.Models;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class CombatSystem : IInitializable, IDisposable
    {
        [Inject] private PlayerService _playerService;
        [Inject] private BattleEnemyService _battleEnemyService;
        [Inject] private PlayerEventBus _playerEventBus;

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }


        public async Task DoPlayerTurn()
        {
            var balls = _playerService.GetCurrentBalls();
            foreach (var ball in balls)
            {
                DoBallLogic(ball);
            }
            var targetType = TargetType.First;
            var enemiesToHit = FindEnemiesForTarget(targetType);
            _battleEnemyService.DealDamage(5, enemiesToHit[0]);
            await Task.Delay(500);
        }

        private void DoBallLogic(PlayerBallModel ball)
        {
            int damage = ball.Damage;
            var targetType = ball.TargetType;
            var enemiesToHit = FindEnemiesForTarget(targetType);
            foreach (var enemy in enemiesToHit)
            {
                _playerEventBus.Attack();
                //wait until we can Move On
                _battleEnemyService.DealDamage(damage, enemy);
            }
        }

        private List<EnemyModel> FindEnemiesForTarget(TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.First:
                    return _battleEnemyService.GetFirstEnemy();
                case TargetType.All:
                    return _battleEnemyService.GetAllEnemies();
                case TargetType.Last:
                    return _battleEnemyService.GetLastEnemy();
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }
        }

        public async Task DoEnemyTurn()
        {
            var enemies = _battleEnemyService.GetEnemiesForCurrentWave();
            foreach (var enemy in enemies)
            {
                _playerService.TakeDamage(enemy.Damage);
                await Task.Delay(500);
            }
        }
    }
}