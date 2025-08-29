using System;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGamePresenter : IInitializable, IDisposable
    {
        [Inject] private readonly ShellGameView _shellGameView;
        [Inject] private readonly BattleEventBus _battleBus;
        [Inject] private readonly BattleSystem _battleSystem;
        //battle service?
        //battle model with current state?

        public void Initialize()
        {
            Debug.Log("ShellGamePresenter initializing");
            _battleBus.OnBattleStateChanged += OnShellGameStarted;
            _shellGameView.ChooseAttackButton.onClick.AddListener(OnPlayerAttackChosen);
        }

        public void Dispose()
        {
            _shellGameView.ChooseAttackButton.onClick.RemoveListener(OnPlayerAttackChosen);
            _battleBus.OnBattleStateChanged -= OnShellGameStarted;
        }

        private void OnPlayerAttackChosen()
        {
            _shellGameView.TurnOffButtonInteraction();
            _battleSystem.ChooseBall(1);
        }

        private void OnShellGameStarted(BattleState state)
        {
            Debug.Log($"ShellGame view init {state}, {state != BattleState.ShellGame}");
            if (state != BattleState.ShellGame)
                return;
            
            _shellGameView.TurnOnButtonInteraction();
        }
    }

}