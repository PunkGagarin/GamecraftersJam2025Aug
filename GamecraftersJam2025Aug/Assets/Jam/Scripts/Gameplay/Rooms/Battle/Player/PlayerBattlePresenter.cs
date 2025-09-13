using System;
using Jam.Scripts.Gameplay.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Player
{
    public class PlayerBattlePresenter : IInitializable, IDisposable
    {
        [Inject] private PlayerEventBus _playerEventBus;
        [Inject] private BattleEventBus _battleBus;
        [Inject] private PlayerBattleView _view;

        public void Initialize()
        {
            _playerEventBus.OnPlayerCreated += InitView;
            _playerEventBus.OnDeath += ShowDeath;
            _playerEventBus.OnDamageTaken += ShowDamageTaken;
            _playerEventBus.OnHealTaken += ShowHeal;
            _playerEventBus.OnSetActive += SetActive;
            _playerEventBus.OnAttackStart += StartAttackAnimation;
            _playerEventBus.OnBallAdded += AddBallToQueue;
            _battleBus.OnPlayerTurnEnd += ClearBalls;
        }

        public void Dispose()
        {
            _playerEventBus.OnPlayerCreated -= InitView;
            _playerEventBus.OnDeath -= ShowDeath;
            _playerEventBus.OnDamageTaken -= ShowDamageTaken;
            _playerEventBus.OnHealTaken -= ShowHeal;
            _playerEventBus.OnSetActive -= SetActive;
            _playerEventBus.OnAttackStart -= StartAttackAnimation;
            _playerEventBus.OnBallAdded -= AddBallToQueue;
            _battleBus.OnPlayerTurnEnd -= ClearBalls;
        }

        private void ClearBalls()
        {
            _view.TurnOffAllQueueBalls();
        }

        private void ShowHeal((int currentHealth, int maxHealth, int heal) parameters)
        {
            _view.ShowHeal(parameters.currentHealth, parameters.maxHealth, parameters.heal);
        }

        private void ShowDamageTaken((int currentHealth, int maxHealth, int damage) parameters)
        {
            _view.ShowDamageTaken(parameters.currentHealth, parameters.maxHealth, parameters.damage);
        }

        private void ShowDeath()
        {
            _view.ShowDeath();
        }

        private void SetActive(bool isActive)
        {
            _view.gameObject.SetActive(isActive);
        }

        private async void StartAttackAnimation(Guid id)
        {
            await _view.PlayAttackAnimation();
            _view.TurnOffLastBall();
            _playerEventBus.AttackEndInvoke(id);
        }

        private void InitView(PlayerModel player)
        {
            _view.Init(player.MaxHealth);
        }

        private void AddBallToQueue(BallDto dto)
        {
            _view.TurnOnQueueBall(dto);
        }
    }
}