using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Gameplay.Battle.Enemy;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    // [CreateAssetMenu(menuName = "Gameplay/Enemies/Repository", fileName = "EnemyConfigRepository", order = 0)]
    public class EnemyConfigRepository : ScriptableObject
    {
        [field: SerializeField]
        public List<EnemySo> Enemies { get; private set; }

        [field: Header("Конфиг пулов, этажи с 1")]
        [field: SerializeField]
        public EnemyPool EnemyPool { get; private set; }

        [field: SerializeField]
        public List<EnemyTierInfo> TierListWeight { get; private set; }

        [field: Header("Значения для создания врагов из пула")]
        [field: SerializeField]
        public int FloorWeightMultiplier { get; private set; } = 2;

        [field: SerializeField]
        public int StartRoomWeight { get; private set; } = 0;

        [field: SerializeField]
        public int MaxEnemiesPerWave { get; private set; } = 0;


        public EnemySo GetRandomEnemy()
        {
            return Enemies[Random.Range(0, Enemies.Count)];
        }

        public Dictionary<EnemyTier, int> GetEnemyWeightConfig(int floor)
        {
            Debug.Log($"Getting enemy weight config for floor: {floor}");
            List<CustomKeyValue<EnemyTier, int>> lastTier = new();
            for (int i = 0; i < TierListWeight.Count; i++)
            {
                var customKeyValue = TierListWeight[i].TierInfo;
                int configFloor = customKeyValue.Key;
                if (configFloor <= floor)
                {
                    lastTier = customKeyValue.Value;
                }
            }

            return lastTier.ToDictionary(x => x.Key, x => x.Value);
        }

        public EnemySo PickEnemyFor(int currentRoomWeight, int roomFloor, int level)
        {
            Debug.Log($"Picking enemy for: weight: {currentRoomWeight}, floor: {roomFloor}, level: {level}");

            EnemyTier maxTier = GetMaxTierForWeight(currentRoomWeight);
            Debug.Log($"Max tier for weight: {maxTier}");
            Dictionary<EnemyTier, List<EnemySo>> allEnemies = new()
            {
                { EnemyTier.Easy, new List<EnemySo>() },
                { EnemyTier.Medium, new List<EnemySo>() },
                { EnemyTier.Hard, new List<EnemySo>() }
            };

            LevelInfo levelInfo = EnemyPool.Levels.FirstOrDefault(el => el.LevelNumber == level);

            if (levelInfo == null)
            {
                Debug.Log($"can't find level info for level: {level}");
                return null;
            }

            foreach (var floor in levelInfo.Floors)
            {
                if (floor.FloorNumber <= roomFloor)
                {
                    foreach (var enemy in floor.Enemies.Where(enemy => enemy.Tier <= maxTier))
                    {
                        allEnemies[enemy.Tier].Add(enemy);
                    }
                }
            }

            return RandomEnemyByWeight(allEnemies, roomFloor);
        }

        private EnemySo RandomEnemyByWeight(Dictionary<EnemyTier, List<EnemySo>> allEnemies, int roomFloor)
        {
            Dictionary<EnemyTier, int> weightedConfig = GetEnemyWeightConfig(roomFloor);

            // int totalWeight = weightedConfig.Sum(x => x.Value);
            int totalWeight = 0;

            for (int i = 0; i < weightedConfig.Keys.Count; i++)
            {
                var currentType = weightedConfig.Keys.ElementAt(i);
                if (allEnemies[currentType].Count > 0)
                    totalWeight += weightedConfig[currentType];
            }


            int randomWeight = Random.Range(0, totalWeight + 1);
            int currentWeight = 0;

            foreach (var (tier, weight) in weightedConfig)
            {
                currentWeight += weight;
                if (randomWeight <= currentWeight)
                {
                    var currentTierEnemies = allEnemies[tier];
                    return currentTierEnemies[Random.Range(0, currentTierEnemies.Count)];
                }
            }

            return null;
        }

        private EnemyTier GetMaxTierForWeight(int currentRoomWeight)
        {
            int lastTier = Enum.GetValues(typeof(EnemyTier)).Cast<int>().Last();

            if (currentRoomWeight < lastTier)
                lastTier = currentRoomWeight;

            return (EnemyTier)lastTier;
        }
    }
}


[Serializable]
public class EnemyPool
{
    [field: SerializeField]
    public List<LevelInfo> Levels { get; set; }
}

[Serializable]
public class LevelInfo
{
    [field: SerializeField]
    public int LevelNumber { get; set; }

    [field: SerializeField]
    public List<FloorInfo> Floors { get; set; }
}

[Serializable]
public class FloorInfo
{
    [field: SerializeField]
    public int FloorNumber { get; set; }

    [field: SerializeField]
    public List<EnemySo> Enemies { get; set; }

    // public override EnemySo this[int index] => Enemies[index];
}