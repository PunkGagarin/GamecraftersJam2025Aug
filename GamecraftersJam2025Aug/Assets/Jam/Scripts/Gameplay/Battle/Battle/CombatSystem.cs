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
        [Inject] private EnemyEventBus _enemyEventBus;
        [Inject] private AttackAckAwaiter _waiter;

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }


        public async Task DoPlayerTurn()
        {
            // var balls = _playerService.GetCurrentBalls();
            // foreach (var ball in balls)
            // {
            //     DoBallLogic(ball);
            //     await Task.Delay(500);
            // }

            var balls = _playerService.GetBallsCount();
            for (int i = 0; i < balls; i++)
            {
                if (_battleEnemyService.IsAnyEnemyAlive())
                    await DoBallLogic();
            }
        }

        private async Task DoBallLogic()
        {
            // int damage = ball.Damage;
            int damage = 5;
            var targetType = TargetType.First;
            var enemiesToHit = FindEnemiesForTarget(targetType);
            foreach (var enemy in enemiesToHit)
            {
                var guid = Guid.NewGuid();
                _playerEventBus.AttackStartInvoke(guid);
                await _waiter.Wait(guid);
                _battleEnemyService.DealDamage(damage, enemy);
            }
        }

        private List<EnemyModel> FindEnemiesForTarget(TargetType targetType)
        {
            return targetType switch
            {
                TargetType.First => _battleEnemyService.GetFirstEnemy(),
                TargetType.All => _battleEnemyService.GetAllEnemies(),
                TargetType.Last => _battleEnemyService.GetLastEnemy(),
                _ => throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null)
            };
        }

        public async Task DoEnemyTurn()
        {
            var enemies = _battleEnemyService.GetAliveEnemiesForCurrentWave();
            foreach (var enemy in enemies)
            {
                var guid = Guid.NewGuid();
                _enemyEventBus.InvokeAttackStart(guid, enemy);
                await _waiter.Wait(guid);
                _playerService.TakeDamage(enemy.Damage);
            }
        }
    }
}