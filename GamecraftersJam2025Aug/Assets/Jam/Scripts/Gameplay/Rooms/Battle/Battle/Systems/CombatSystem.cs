using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Enemy;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Systems
{
    public class CombatSystem : IInitializable, IDisposable
    {
        [Inject] private PlayerService _playerService;
        [Inject] private BattleEnemyService _battleEnemyService;
        [Inject] private PlayerEventBus _playerEventBus;
        [Inject] private EnemyEventBus _enemyEventBus;
        [Inject] private BattleEventBus _battleEventBus;
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
                case DamagePayload p: DoDirectDamage(effect.Targeting, p.Damage); break;
                case HealPayload p: Heal(effect.Targeting, p); break;
                case ShieldPayload p: GiveShield(effect.Targeting, p); break;
                case PoisonPayload p: AddPoisoinStacks(effect.Targeting, p); break;
                case CriticalDamagePayload p: ApplyCrit(effect.Targeting, p); break;
            }
        }

        private void DoDirectDamage(TargetType targetType, int damage)
        {
            if (targetType == TargetType.Player)
                DoSelfDamage(damage);
            else
            {
                var targets = new List<EnemyModel>(FindEnemiesForTarget(targetType));
                foreach (var enemy in targets)
                {
                    OnBeforeDamageDto dto = new OnBeforeDamageDto { DamageAmount = damage };
                    _battleEventBus.OnBeforeDamageInvoke(dto);

                    _battleEnemyService.TakeDamage(dto.DamageAmount, enemy);
                    _battleEventBus.OnAfterDamageInvoke(dto.DamageAmount);
                }
            }
        }

        private void DoSelfDamage(int damage)
        {
            _playerService.TakeDamage(damage);
        }

        private void Heal(TargetType targetType, HealPayload healPayload)
        {
            var healDto = new OnHealDto { HealAmount = healPayload.Amount };
            _battleEventBus.BeforeHealFromBallInvoke(healDto);

            var healAmount = healDto.HealAmount;
            if (targetType == TargetType.Player)
            {
                _playerService.Heal(healAmount);
                _battleEventBus.OnHealInvoke(healAmount);
            }
            else
            {
                var targets = FindEnemiesForTarget(targetType);
                foreach (var enemy in targets)
                {
                    _battleEnemyService.Heal(healAmount, enemy);
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
            DoDirectDamage(targetType, damage);
        }

        private int FindCritDamage(CriticalDamagePayload payLoad)
        {
            int random = Random.Range(0, 100);

            if (random <= payLoad.Chance)
            {
                _battleEventBus.PlayerDealCriticalInvoke();
                return (int)(payLoad.Damage * payLoad.Multiplier);
            }

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
                _playerService.TakeDamage(enemy.CurrentDamage);
            }
        }
    }
}