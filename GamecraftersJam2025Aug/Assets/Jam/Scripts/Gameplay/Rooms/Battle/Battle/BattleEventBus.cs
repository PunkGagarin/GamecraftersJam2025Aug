using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class BattleEventBus
    {
        public event Action<BattleState> OnBattleStateChanged = delegate { };
        public event Action<int> OnShellGameStarted = delegate { };
        public event Action<(int newWaveNumber, List<EnemyModel> enemies)> OnWaveChanged = delegate { };
        public event Action<(Guid attackId, EnemyModel enemy)> OnEnemyAttack = delegate { };
        public event Action<Guid> OnAttackPresented = delegate { };
        public event Action OnPlayerDealCritical = delegate { };
        public event Action OnLose = delegate { };
        public event Action<OnHealDto> OnBeforeHeal = delegate { };
        
        
        public void BeforeHealFromBallInvoke(OnHealDto dto) => OnBeforeHeal.Invoke(dto);
        public void EnemyAttackInvoke((Guid attackId, EnemyModel enemy) e) => OnEnemyAttack(e);
        public void EnemyAttackFinishedInvoke(Guid id) => OnAttackPresented(id);

        public void WaveChangedInvoke((int newWaveNumber, List<EnemyModel> enemies) waveInfo) =>
            OnWaveChanged.Invoke(waveInfo);

        public void BattleStateChangedInvoke(BattleState state) => OnBattleStateChanged.Invoke(state);
        public void ShellGameStartedInvoke(int newWaveNumber) => OnShellGameStarted.Invoke(newWaveNumber);
        public void InvokeLose() => OnLose.Invoke();
        public void PlayerDealCriticalInvoke() => OnPlayerDealCritical.Invoke();
    }
}