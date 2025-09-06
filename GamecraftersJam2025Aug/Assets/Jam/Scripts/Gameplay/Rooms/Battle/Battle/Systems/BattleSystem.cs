using System.Collections.Generic;
using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Battle.Queue;
using Jam.Scripts.Gameplay.Battle.Queue.Model;
using Jam.Scripts.Gameplay.Battle.ShellGame;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Rooms.Battle.Enemy;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Systems
{
    public class BattleSystem
    {
        [Inject] private ShellGameEventBus _shellGameEventBus;
        [Inject] private BattleEventBus _eventBus;
        [Inject] private RoomRewardBus _roomRewardBus;
        [Inject] private EnemyEventBus _enemyEvent;
        
        [Inject] private BattleEnemyService _enemyService;
        [Inject] private PlayerService _playerService;
        [Inject] private PlayerInventoryService _playerInventory;
        [Inject] private BattleQueueService _battleQueueService;
        [Inject] private CombatSystem _combatSystem;
        [Inject] private ShellGameView _shellGame;

        private BattleState _currentState;

        private void ChangeStateTo(BattleState state)
        {
            _currentState = state;
            _eventBus.BattleStateChangedInvoke(_currentState);
        }

        public void StartBattle(RoomBattleConfig room)
        {
            Debug.Log($"Battle started for room {room.Floor} level {room.Level}");
            InitBattleData(room);
        }

        private void InitBattleData(RoomBattleConfig room)
        {
            _enemyService.CreateEnemiesFor(room);
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

            var queueCount = _battleQueueService.GetQueueCount();
            _eventBus.ShellGameStartedInvoke(queueCount);
        }

        private void CleanUpRound()
        {
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
            await _combatSystem.DoPlayerTurn();

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
            _roomRewardBus.InvokeRoomCompleted();
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
            
        }
    }
}