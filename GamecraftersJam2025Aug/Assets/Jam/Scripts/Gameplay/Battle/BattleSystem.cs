using System;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.MapFeature.Map.Data;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleSystem : IInitializable, IDisposable
    {
        [Inject] private BattleInventoryPresenter _battleInventoryPresenter;
        [Inject] private BattleEnemyService _enemyService;

        public void Initialize()
        {
            //move to EventBus
            StartBattle();
        }

        public void Dispose()
        {
        }

        public void StartBattle()
        {
            InitBattleData();
        }

        private void InitBattleData()
        {
            // _battleInventoryPresenter.InitBattleData();
            //todo: room should be selected from above
            _enemyService.CreateEnemiesFor(new Room());
            _enemyService.IncrementWave();
            //create enemy
            //create or show player
            //prepare ShellGame
        }
    }

}