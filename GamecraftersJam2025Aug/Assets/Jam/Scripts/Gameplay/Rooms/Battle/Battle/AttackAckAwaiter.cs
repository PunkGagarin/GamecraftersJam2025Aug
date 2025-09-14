using System;
using System.Collections.Concurrent;
using System.Threading;
using Cysharp.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public sealed class AttackAckAwaiter : IInitializable, IDisposable
    {
        [Inject] private readonly BattleEventBus _bus;
        [Inject] private readonly PlayerEventBus _playerBus;
        [Inject] private readonly EnemyEventBus _enemyEventBus;

        private readonly ConcurrentDictionary<Guid, UniTaskCompletionSource<bool>> _pending = new();

        public void Initialize()
        {
            _playerBus.OnAttackEnd += OnAck;
            _enemyEventBus.OnAttackEnd += OnAck;
        }

        public void Dispose()
        {
            // Исправлено: отписываемся от тех же событий, что и подписались
            _playerBus.OnAttackEnd -= OnAck;
            _enemyEventBus.OnAttackEnd -= OnAck;

            foreach (var kv in _pending)
                kv.Value.TrySetCanceled();

            _pending.Clear();
        }

        /// <summary>
        /// Ждём подтверждение завершения атаки до таймаута/отмены.
        /// </summary>
        public async UniTask Wait(Guid attackId, int timeoutMs = 8000, CancellationToken ct = default)
        {
            var tcs = new UniTaskCompletionSource<bool>();

            if (!_pending.TryAdd(attackId, tcs))
                throw new InvalidOperationException($"Attack {attackId} already pending.");

            try
            {
                // Ожидаем ack с внешней отменой и таймаутом.
                // Если ct отменён -> прилетит OperationCanceledException.
                // Если истёк timeout -> прилетит TimeoutException.
                await tcs.Task
                    .AttachExternalCancellation(ct)
                    .Timeout(TimeSpan.FromMilliseconds(timeoutMs));
            }
            catch (TimeoutException)
            {
                // Отдельно перекидываем более говорящий текст
                throw new TimeoutException($"AttackPresented timeout for {attackId}");
            }
            finally
            {
                _pending.TryRemove(attackId, out _);
            }
        }

        private void OnAck(Guid attackId)
        {
            if (_pending.TryRemove(attackId, out var tcs))
                tcs.TrySetResult(true);
        }
    }
}
