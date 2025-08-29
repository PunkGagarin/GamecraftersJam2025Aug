using System;
using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleSystem : IInitializable, IDisposable
    {
        [Inject] private BattleEnemyService _enemyService;
        [Inject] private BattleEventBus _eventBus;
        [Inject] private EnemyBusEvent _enemyEvents;
        [Inject] private PlayerService _playerService;
        [Inject] private CombatSystem _combatSystem;

        private int _totalBallChoice = 1;
        private int _currentBallChoice = 0;
        private BattleState _currentState;

        public void Initialize()
        {
            // _enemyEvents.OnDeath += SpawnNextWaveIfNeeded;
            //move to EventBus
            StartBattle();
        }

        public void Dispose()
        {
            // _enemyEvents.OnDeath -= SpawnNextWaveIfNeeded;
        }

        // todo:
        //private PlayerDeath (should be here??)

        public async void StartBattle()
        {
            Debug.Log($"Battle started");
            //todo: call from outside instead
            await Task.Delay(100);
            InitBattleData();
        }

        private void InitBattleData()
        {
            // _battleInventoryPresenter.InitBattleData();
            //todo: room should be selected from above
            _enemyService.CreateEnemiesFor(new Room());
            _enemyService.IncrementWave();

            //todo: надо ли?
            // _eventBus.BattleInited.Invoke();
            Debug.Log($"Init finished");
            StartShellGame();
        }

        private void StartShellGame()
        {
            Debug.Log($"ShellGame started");
            _currentBallChoice = 0;
            ChangeStateTo(BattleState.ShellGame);
        }

        private void ChangeStateTo(BattleState state)
        {
            _currentState = state;
            _eventBus.BattleStateChangedInvoke(_currentState);
        }

        //todo: currenlty have fake logic
        public void ChooseBall(int ballId)
        {
            Debug.Log($"On Ball choosen");
            _currentBallChoice++;
            //todo: addBall
            if (_currentBallChoice >= _totalBallChoice)
            {
                StartPlayerTurn();
            }
        }

        private async void StartPlayerTurn()
        {
            Debug.Log($"On Player turn start");
            ChangeStateTo(BattleState.PlayerTurn);
            await _combatSystem.DoPlayerTurn(); //todo: add await;

            if (ThereIsAliveEnemy())
                StartEnemyTurn();
            else if (ThereIsNextWave())
                IncrementWave();
            else
                FinishBattle();
        }

        private bool ThereIsNextWave()
        {
            return _enemyService.IsNextWave();
        }

        private void IncrementWave()
        {
            _enemyService.IncrementWave();
            StartShellGame();
        }

        private bool ThereIsAliveEnemy()
        {
            return _enemyService.IsAnyEnemyAlive();
        }

        private void FinishBattle()
        {
            //todo:
            Debug.Log("не осталось врагов, заканчиваем битву");
        }

        private async void StartEnemyTurn()
        {
            Debug.Log($"On Enemy turn start");
            ChangeStateTo(BattleState.EnemyTurn);
            await _combatSystem.DoEnemyTurn();

            if (PlayerIsDead())
                GameOver();
            else
                StartShellGame();
        }


        private void SpawnNextWaveIfNeeded(EnemyModel _)
        {
            if (ThereIsNextWave())
                IncrementWave();
        }

        private bool PlayerIsDead()
        {
            return _playerService.IsDead();
        }

        private void GameOver()
        {
            //todo:
            Debug.LogError("Player is dead");
        }
    }

}