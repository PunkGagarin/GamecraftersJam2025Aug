using System.Collections;
using System.Collections.Generic;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyService
    {
        [Inject] private EnemyFactory _enemyFactory;

        public Dictionary<int, List<EnemyModel>> Enemies { get; private set; }

        public void CreateEnemiesFor(Room room)
        {
            Enemies = new();

            //todo: react on room
            //int enemyCount = room.EnemyCount;
            int waveCount = 2;
            int enemyCount = 2;
            for (int i = 1; i <= waveCount; i++)
            {
                Enemies.Add(i, new List<EnemyModel>());
                for (int j = 0; j < enemyCount; j++)
                {
                    var enemy = _enemyFactory.CreateEnemy();
                    Enemies[i].Add(enemy);
                }
            }
        }


        /// <summary>
        ///  возвращает модели врагов для волны
        /// </summary>
        /// <param name="waveNumber"> начинается с 1</param>
        /// <returns></returns>
        public IEnumerable GetEnemiesForWave(int waveNumber)
        {
            if (Enemies.Keys.Count <= waveNumber)
            {
                //такой волны не существует
                Debug.LogError($"Пытаемся получить данные о не созданной волне");
            }

            return Enemies[waveNumber];
        }
    }
}