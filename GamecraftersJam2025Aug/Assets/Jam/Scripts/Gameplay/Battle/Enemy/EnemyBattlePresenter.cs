using System.Collections.Generic;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyBattlePresenter
    {
        private EnemyService _enemyService;
        private BattleEnemyView _enemyView;

        [Inject] private BattleConfig _battleConfig;
        
        //map view and models
        private Dictionary<EnemyModel, EnemyView> _currentWave = new();
        private int currentWaveNumber = 1;

        public void InitEnemies(Room room)
        {
            _enemyService.CreateEnemiesFor(room);
            var firstWave = _enemyService.GetEnemiesForWave(currentWaveNumber);
            foreach (var enemy in firstWave)
            {
                
            }
            //for enemiesCount:
            //GenerateEnemies
            //SubscribeAllEnemies
        }

        public class RoomSettings
        {
            // private int level;
            // private RoomType roomType;
        }

        public class EnemiesConfig : ScriptableObject
        {

        }
    }

}