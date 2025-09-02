using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Battle.Queue;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using Jam.Scripts.Gameplay.Battle.ShellGame;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.MapFeature.Map.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleSystem : IInitializable, IDisposable
    {
        [Inject] private BattleEnemyService _enemyService;
        [Inject] private BattleEventBus _eventBus;
        [Inject] private EnemyEventBus _enemyEvent;
        [Inject] private ShellGameEventBus _shellGameEventBus;
        [Inject] private PlayerService _playerService;
        [Inject] private PlayerInventoryService _playerInventory;
        [Inject] private BattleQueueService _battleQueueService;
        [Inject] private CombatSystem _combatSystem;
        [Inject] private ShellGameView _shellGame;

        private int _totalBallChoice = 1;
        private int _currentBallChoice = 0;
        private BattleState _currentState;

        public void Initialize()
        {
            StartBattle();
        }

        public void Dispose()
        {
            
        }

        private void ChangeStateTo(BattleState state)
        {
            _currentState = state;
            _eventBus.BattleStateChangedInvoke(_currentState);
        }

        public async void StartBattle()
        {
            Debug.Log($"Battle started");
            //todo: call from outside instead
            await Task.Delay(100);
            InitBattleData();
        }

        private void InitBattleData()
        {
            //todo: room should be selected from above
            _enemyService.CreateEnemiesFor(new Room());
            _enemyService.IncrementWave();
            _shellGameEventBus.InitInvoke();
            var playerBallModels = _playerInventory.GetAllBallsCopy();
            _battleQueueService.Init(playerBallModels);

            //todo: надо ли?
            // _eventBus.BattleInited.Invoke();
            
            Debug.Log($"Init finished");
            StartShellGame();
        }

        private void StartShellGame()
        {
            CleanUpRound();
            ChangeStateTo(BattleState.ShellGame);
        }

        private void CleanUpRound()
        {
            _currentBallChoice = 0;
            _playerService.ClearBalls();
        }

        public void ChooseBall(int ballId)
        {
            _playerService.AddBall(ballId);
        }

        public async void StartPlayerTurn()
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
            CleanUpBattle();
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

        private bool PlayerIsDead()
        {
            return _playerService.IsDead();
        }

        private void GameOver()
        {
            //todo:
            Debug.LogError("Player is dead");
        }

        public List<BallDto> PrepareBallsForNextShuffle(int ballsCount)
        {
            return _battleQueueService.GetNextBall(ballsCount);
        }

        public void CleanUpBattle()
        {
            // todo: implement me
        }
    }
}