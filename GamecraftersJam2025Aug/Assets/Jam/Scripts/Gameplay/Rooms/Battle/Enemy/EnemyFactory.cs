using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Enemy
{
    public class EnemyFactory
    {
        [Inject] private EnemyConfigRepository _config;

        public BattleWaveModel CreateBattleWaveModel(RoomBattleConfig room)
        {
            int roomWeight = room.Floor * _config.FloorWeightMultiplier + _config.StartRoomWeight;
            Debug.Log($"Room weight: {roomWeight}");

            List<EnemyModel> roomEnemies = CreateEnemiesForRoom(roomWeight, room);
            var enemies = CreateWaveModel(roomEnemies, roomWeight);

            Debug.Log($"Wave count: {enemies.Keys.Count}");

            return new BattleWaveModel(enemies);
        }

        private Dictionary<int, List<EnemyModel>> CreateWaveModel(List<EnemyModel> roomEnemies, int roomWeight)
        {
            int maxEnemyPerWave = _config.MaxEnemiesPerWave;

            int totalEnemies = roomEnemies.Count;

            int waveCount = Mathf.CeilToInt(1f * roomWeight / totalEnemies);
            if (waveCount == 1)
                waveCount++;

            int currentEnemyPerWave = Mathf.CeilToInt(1f * totalEnemies / waveCount);
            if (currentEnemyPerWave > maxEnemyPerWave)
            {
                waveCount = Mathf.CeilToInt(1f * totalEnemies / maxEnemyPerWave);
                currentEnemyPerWave = maxEnemyPerWave;
            }

            var enemies = FormWavesFromEnemies(roomEnemies, waveCount, currentEnemyPerWave);
            return enemies;
        }

        private Dictionary<int, List<EnemyModel>> FormWavesFromEnemies(List<EnemyModel> roomEnemies, int waveCount,
            int currentEnemyPerWave)
        {
            Dictionary<int, List<EnemyModel>> enemies = new();

            int index = 0;
            int currentWave = waveCount;
            
            while (index < roomEnemies.Count)
            {
                if (!enemies.ContainsKey(currentWave))
                    enemies.Add(currentWave, new List<EnemyModel>());

                enemies[currentWave].Add(roomEnemies[index]);
                
                index++;
                currentWave--;
                if (currentWave == 0)
                    currentWave = waveCount;

            }
            return enemies;
        }

        private List<EnemyModel> CreateEnemiesForRoom(int roomWeight, RoomBattleConfig room)
        {
            Debug.Log($"Start creating enemies for room {room.Floor}");
            int roomFloor = room.Floor;
            List<EnemyModel> enemies = new List<EnemyModel>();
            int currentRoomWeight = roomWeight;

            while (currentRoomWeight > 0)
            {
                var enemy = _config.PickEnemyFor(currentRoomWeight, roomFloor, room.Level);
                if (enemy == null)
                    Debug.LogError($" enemy is null");

                currentRoomWeight -= (int)enemy.Tier;
                var enemyModel = CreateEnemyModel(enemy);
                enemies.Add(enemyModel);
            }

            Debug.Log($" enemies count: {enemies.Count}");
            return enemies;
        }

        private static EnemyModel CreateEnemyModel(EnemySo roomEnemy)
        {
            return new EnemyModel(roomEnemy.Damage, roomEnemy.Health,
                roomEnemy.Type, roomEnemy.Sprite, roomEnemy.Tier, roomEnemy.EnemyGraphic);
        }
    }
}