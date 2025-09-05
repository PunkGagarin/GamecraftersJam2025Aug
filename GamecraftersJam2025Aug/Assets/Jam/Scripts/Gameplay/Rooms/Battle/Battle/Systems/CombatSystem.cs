using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Inventory.Models.Definitions;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Battle
{
    public class CombatSystem : IInitializable, IDisposable
    {
        [Inject] private PlayerService _playerService;
        [Inject] private BattleEnemyService _battleEnemyService;
        [Inject] private PlayerEventBus _playerEventBus;
        [Inject] private EnemyEventBus _enemyEventBus;
        [Inject] private AttackAckAwaiter _waiter;
        [Inject] private PlayerInventoryService _inventoryService;

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }


        public async Task DoPlayerTurn()
        {
            var ballIds = _playerService.GetCurrentBattleBallIds();
            foreach (var ballId in ballIds)
            {
                BallBattleDto ball = _inventoryService.GetBattleBallById(ballId);

                if (HasNoTarget(ball))
                    continue;

                await DoBallLogic(ball);
            }
        }

        private bool HasNoTarget(BallBattleDto ball)
        {
            // todo: self player effect when no enemies?
            // return ball.TargetType == TargetType.Player || !_battleEnemyService.IsAnyEnemyAlive();
            return !_battleEnemyService.IsAnyEnemyAlive();
        }

        private async Task DoBallLogic(BallBattleDto ball, TargetType targetType)
        {
            int damage = ball.Damage;
            var enemiesToHit = FindEnemiesForTarget(targetType);

            if (enemiesToHit == null || enemiesToHit.Count == 0)
            {
                Debug.LogError($" пытаемся ударить по врагам, но их нет, изменить логику??");
                return;
            }

            foreach (var enemy in enemiesToHit)
            {
                var guid = Guid.NewGuid();
                _playerEventBus.AttackStartInvoke(guid);
                await _waiter.Wait(guid);
                _battleEnemyService.DealDamage(damage, enemy);
            }
        }

        private async Task DoBallLogic(BallBattleDto ball)
        {
            var effects = ball.Effects;

            var guid = Guid.NewGuid();
            _playerEventBus.AttackStartInvoke(guid);
            await _waiter.Wait(guid);

            foreach (var effect in effects)
            {
                ApplyEffect(effect);
            }
        }

        private void ApplyEffect(EffectInstance effect)
        {
            switch (effect.Payload)
            {
                case DamagePayload p: DoDirectDamage(effect.Targeting, p); break;
                case HealPayload p: Heal(effect.Targeting, p); break;
                case ShieldPayload p: GiveShield(effect.Targeting, p); break;
                case PoisonPayload p: AddPoisoinStacks(effect.Targeting, p); break;
                case CriticalDamagePayload p: ApplyCrit(effect.Targeting, p); break;
            }
        }

        private void DoDirectDamage(TargetType targetType, DamagePayload damagePayload)
        {
            if (targetType == TargetType.Player)
                DoSelfDamage(damagePayload.Damage);
            else
            {
                var targets = FindEnemiesForTarget(targetType);
                foreach (var enemy in targets)
                {
                    _battleEnemyService.DealDamage(damagePayload.Damage, enemy);
                }
            }
        }

        private void DoSelfDamage(int damage)
        {
            _playerService.TakeDamage(damage);
        }

        private void Heal(TargetType targetType, HealPayload healPayload)
        {
            if (targetType == TargetType.Player)
                _playerService.Heal(healPayload.Amount);
            else
            {
                var targets = FindEnemiesForTarget(targetType);
                foreach (var enemy in targets)
                {
                    _battleEnemyService.Heal(healPayload.Amount, enemy);
                }
            }
        }

        private void GiveShield(TargetType targetType, ShieldPayload payLoad)
        {
            throw new NotImplementedException();
        }

        private void AddPoisoinStacks(TargetType targetType, PoisonPayload payLoad)
        {
            throw new NotImplementedException();
        }

        private void ApplyCrit(TargetType targetType, CriticalDamagePayload payLoad)
        {
            int damage = FindCritDamage(payLoad);

            if (targetType == TargetType.Player)
                DoSelfDamage(damage);
            else
            {
                var targets = FindEnemiesForTarget(targetType);
                foreach (var enemy in targets)
                {
                    _battleEnemyService.DealDamage(payLoad.Damage, enemy);
                }
            }
        }

        private int FindCritDamage(CriticalDamagePayload payLoad)
        {
            int random = Random.Range(0, 100);

            if (random <= payLoad.Chance)
                return (int)(payLoad.Damage * payLoad.Multiplier);

            Debug.LogError($"Something is wrong with crit");
            return 0;
        }

        private List<EnemyModel> FindEnemiesForTarget(TargetType targetType)
        {
            return targetType switch
            {
                TargetType.First => _battleEnemyService.GetFirstEnemy(),
                TargetType.All => _battleEnemyService.GetAllEnemies(),
                TargetType.Last => _battleEnemyService.GetLastEnemy(),
                TargetType.Random => _battleEnemyService.GetRandomEnemy(),
                // TargetType.Player => _battleEnemyService.GetLastEnemy(),
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