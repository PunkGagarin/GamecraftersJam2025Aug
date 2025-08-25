using System;
using Jam.Scripts.Gameplay.Battle.Player;
using Zenject;

namespace Jam.Scripts.Gameplay.Player
{
    public class PlayerBattlePresenter : IInitializable, IDisposable
    {
        [Inject] private PlayerEventBus _playerEventBus;
        // [Inject] private PlayerBattleView _playerBattleView;

        public void Initialize()
        {
            _playerEventBus.OnDeath += ShowDeath;
            _playerEventBus.OnDamageTaken += ShowDamageTaken;
            _playerEventBus.OnHealTaken += ShowHeal;
        }

        private void ShowHeal((int currentHealth, int maxHealth, int heal) parameters)
        {
            
        }

        private void ShowDamageTaken((int currentHealth, int maxHealth, int damage) parameters)
        {
            
        }

        private void ShowDeath()
        {
            //show dead sprite
            //play player death sound
        }

        public void Dispose()
        {
        }
    }
}