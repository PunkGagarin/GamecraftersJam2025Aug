using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Zenject;

public sealed class AttackAckAwaiter : IInitializable,IDisposable
{
    [Inject] private readonly BattleEventBus _bus;
    [Inject] private readonly PlayerEventBus _playerBus;
    [Inject] private readonly EnemyEventBus _enemyEventBus;

    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<bool>> _pending = new();

    public void Initialize()
    {
        _playerBus.OnAttackEnd += OnAck;
        _enemyEventBus.OnAttackEnd += OnAck;
    }

    public void Dispose()
    {
        _bus.OnAttackPresented -= OnAck;
        foreach (var kv in _pending)
            kv.Value.TrySetCanceled();
        _pending.Clear();
    }

    public async Task Wait(Guid attackId, int timeoutMs = 8000, CancellationToken ct = default)
    {
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (!_pending.TryAdd(attackId, tcs))
            throw new InvalidOperationException($"Attack {attackId} already pending.");

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var delay = Task.Delay(timeoutMs, timeoutCts.Token);

        var completed = await Task.WhenAny(tcs.Task, delay).ConfigureAwait(false);

        if (completed == delay)
        {
            _pending.TryRemove(attackId, out _);
            throw new TimeoutException($"AttackPresented timeout for {attackId}");
        }

        timeoutCts.Cancel(); // отменяем задержку
        await tcs.Task.ConfigureAwait(false); // пропускаем исключения, если были (не должно)
    }

    private void OnAck(Guid attackId)
    {
        if (_pending.TryRemove(attackId, out var tcs))
            tcs.TrySetResult(true);
    }

}