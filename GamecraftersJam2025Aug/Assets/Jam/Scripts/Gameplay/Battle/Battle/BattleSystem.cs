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
        [Inject] private PlayerService _playerService;
        [Inject] private CombatSystem _combatSystem;

        private BattleState _currentState;
        private int _totalBallChoice = 1;
        private int _currentBallChoice = 0;

        public BattleState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                _eventBus.BattleStateChangedInvoke(_currentState);
            }
        }

        public void Initialize()
        {
            //move to EventBus
            StartBattle();
        }

        public void Dispose()
        {
            
        }

        // todo:
        //private PlayerDeath (should be here??)

        public async void StartBattle()
        {
            Debug.Log($"Battle started");
            await Task.Delay(100);
            InitBattleData();
        }

        private void InitBattleData()
        {
            // _battleInventoryPresenter.InitBattleData();
            //todo: room should be selected from above
            _enemyService.CreateEnemiesFor(new Room());
            _enemyService.IncrementWave();

            //надо ли?
            // _eventBus.BattleInited.Invoke();
            Debug.Log($"Init finished");
            StartShellGame();
        }

        private void StartShellGame()
        {
            Debug.Log($"ShellGame started");
            CurrentState = BattleState.ShellGame;
        }

        public void ChooseBall(int i)
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
            _currentState = BattleState.PlayerTurn;
            _combatSystem.DoPlayerTurn(); //add await;
            if (ThereIsAliveEnemy())
                StartEnemyTurn();
            else
                FinishBattle();
        }

        private bool ThereIsAliveEnemy()
        {
            return _enemyService.IsAnyEnemyAlive();
        }

        private void FinishBattle()
        {
            Debug.Log("не осталось врагов, заканчиваем битву");
        }

        private void StartEnemyTurn()
        {
            Debug.Log($"On Enemy turn start");
            _currentState = BattleState.EnemyTurn;
            _combatSystem.DoEnemyTurn();
            
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
            Debug.LogError("Player is dead");
        }
    }

}