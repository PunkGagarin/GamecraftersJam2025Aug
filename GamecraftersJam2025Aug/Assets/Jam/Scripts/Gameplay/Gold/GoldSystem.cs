using System;
using System.Linq;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Rooms.Battle.Enemy;
using UnityEngine;
using Zenject;

namespace Jam.Prefabs.Gameplay.Gold
{
    public class GoldSystem : IInitializable, IDisposable
    {
        [Inject] private EnemyEventBus _enemyBus;
        [Inject] private GoldService _goldService;
        [Inject] private GoldConfig _config;

        public void Initialize()
        {
            _enemyBus.OnDeath += GetGoldFromEnemy;
        }


        public void Dispose()
        {
            _enemyBus.OnDeath -= GetGoldFromEnemy;
        }

        private void GetGoldFromEnemy(EnemyModel enemy)
        {
            Debug.Log("высчитываем получение золота для " + enemy.Type);
            int? rawGold = _config.GoldPerEnemy.FirstOrDefault(x => x.Key == enemy.Tier)?.Value;
            int goldGap = _config.Gap;
            if (rawGold.HasValue && goldGap > 0)
            {
                int min = (int)(rawGold.Value * (1 - goldGap / 100f));
                int max = (int)(rawGold.Value * (1 + goldGap / 100f));
                int amount = UnityEngine.Random.Range(min, max);
                _goldService.AddGold(amount);
            }
        }
    }
}