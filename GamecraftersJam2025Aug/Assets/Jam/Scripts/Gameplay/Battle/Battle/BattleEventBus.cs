using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleEventBus
    {
        public event Action<BattleState> OnBattleStateChanged = delegate { };
        public event Action<(int newWaveNumber, List<EnemyModel> enemies)> OnWaveChanged = delegate { };

        public void WaveChangedInvoke((int newWaveNumber, List<EnemyModel> enemies) waveInfo) =>
            OnWaveChanged.Invoke(waveInfo);

        public void BattleStateChangedInvoke(BattleState state)
        {
            Debug.Log($" on state event raised {state}");
            OnBattleStateChanged.Invoke(state);
        }
    }

}