using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.Enemy;

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

        public void RemoveDeadEnemy(EnemyModel enemy)
        {
            Enemies[CurrentBattleWave].Remove(enemy);
        }
    }
}