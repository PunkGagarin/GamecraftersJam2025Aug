using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleEventBus
    {
        public event Action<BattleState> OnBattleStateChanged = delegate { };
        public event Action<int> OnShellGameStarted = delegate { };
        public event Action<(int newWaveNumber, List<EnemyModel> enemies)> OnWaveChanged = delegate { };
        public event Action<(Guid attackId, EnemyModel enemy)> OnEnemyAttack = delegate { };
        public event Action<Guid> OnAttackPresented = delegate { };
        public event Action OnWin = delegate { };
        public event Action OnLose = delegate { };

        public void EnemyAttackInvoke((Guid attackId, EnemyModel enemy) e) => OnEnemyAttack(e);
        public void EnemyAttackFinishedInvoke(Guid id) => OnAttackPresented(id);
        public void WaveChangedInvoke((int newWaveNumber, List<EnemyModel> enemies) waveInfo) =>
            OnWaveChanged.Invoke(waveInfo);
        public void BattleStateChangedInvoke(BattleState state) => OnBattleStateChanged.Invoke(state);
        public void ShellGameStartedInvoke(int newWaveNumber) => OnShellGameStarted.Invoke(newWaveNumber);
        public void InvokeWin() => OnWin.Invoke();
        public void InvokeLose() => OnLose.Invoke();
    }
}