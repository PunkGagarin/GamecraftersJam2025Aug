using System.Collections.Generic;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class BattleWaveModel
    {
        public int CurrentBattleWave { get; private set; }
        public Dictionary<int, List<EnemyModel>> Enemies { get; private set; }

        public BattleWaveModel(Dictionary<int, List<EnemyModel>> enemies)
        {
            Enemies = enemies;
        }

        public void IncrementWave()
        {
            CurrentBattleWave++;
        }
    }
}