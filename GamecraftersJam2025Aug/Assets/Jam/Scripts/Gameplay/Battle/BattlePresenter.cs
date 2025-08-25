using System;
using Jam.Scripts.Gameplay.Player;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle
{
    public class BattlePresenter : IInitializable, IDisposable
    {
        //откуда берём врагов?
        [Inject] private PlayerBattlePresenter _playerBattlePresenter; //(хп, вью и т.д.)
        [Inject] private BattleInventoryPresenter _battleInventoryPresenter;
        // [Inject] private En
        


        public void Initialize()
        {
            
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
            _battleInventoryPresenter.InitBattleData();
            //create enemy
            //create or show player
            //prepare ShellGame
        }
    }

}