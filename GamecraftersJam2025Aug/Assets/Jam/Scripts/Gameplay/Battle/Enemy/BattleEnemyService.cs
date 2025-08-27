using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class BattleEnemyService
    {
        [Inject] private EnemyFactory _enemyFactory;
        [Inject] private BattleEventBus _battleEventBus;


        private BattleWaveModel _battleWaveModel;


        //вызывается оркестратором битвы
        public void CreateEnemiesFor(Room room)
        {
            _battleWaveModel = _enemyFactory.CreateBattleWaveModel(room);
        }

        public void IncrementWave()
        {
            _battleWaveModel.IncrementWave();
            int nextWave = _battleWaveModel.CurrentBattleWave;
            var nextWaveEnemies = _battleWaveModel.Enemies[nextWave];

            _battleEventBus.OnWaveChanged.Invoke((nextWave, nextWaveEnemies));
        }
        
        public void CleanUpBattleData()
        {
            _battleWaveModel = null;
        }
    }

}