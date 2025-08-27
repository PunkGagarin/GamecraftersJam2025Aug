using System;
using System.Collections.Generic;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class BattleEventBus
    {
        public Action<(int newWaveNumber, List<EnemyModel> enemies)> OnWaveChanged = delegate { };
    }
}