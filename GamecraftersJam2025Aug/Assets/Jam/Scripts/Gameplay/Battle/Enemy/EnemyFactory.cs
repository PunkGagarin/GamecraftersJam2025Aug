using System.Collections.Generic;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyFactory
    {
        [Inject] private EnemyConfigRepository _enemyConfigRepository;

        public EnemyModel CreateEnemy()
        {
            EnemySo enemyConfig = _enemyConfigRepository.GetRandomEnemy();
            return new EnemyModel(enemyConfig.Damage, enemyConfig.Health, enemyConfig.Type, enemyConfig.Sprite);
        }

        public BattleWaveModel CreateBattleWaveModel(RoomBattleConfig room)
        {
            Dictionary<int, List<EnemyModel>> enemies = new();

            //todo: get from room settings
            int waveCount = 1;
            int enemyCount = 1;
            for (int i = 1; i <= waveCount; i++)
            {
                enemies.Add(i, new List<EnemyModel>());
                for (int j = 0; j < enemyCount; j++)
                {
                    var enemy = CreateEnemy();
                    enemies[i].Add(enemy);
                }
            }

            return new BattleWaveModel(enemies);
        }
    }
}