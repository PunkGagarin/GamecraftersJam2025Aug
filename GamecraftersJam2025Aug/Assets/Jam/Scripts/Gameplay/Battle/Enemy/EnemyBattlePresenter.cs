using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyBattlePresenter
    {
        private List<EnemyBattleModel> _enemies;

        [Inject] private EnemiesConfig _enemiesConfig;

        public void InitEnemies(RoomSettings roomSettings)
        {
            //get enemiesCount
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