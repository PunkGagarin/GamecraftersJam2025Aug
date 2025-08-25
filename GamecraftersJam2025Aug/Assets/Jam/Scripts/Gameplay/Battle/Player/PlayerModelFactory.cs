using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerModelFactory
    {
        [Inject] private PlayerUnitConfig _playerUnitConfig;
        [Inject] private PlayerEventBus _bus;

        //Config
        public PlayerModel CreatePlayer()
        {
            return new PlayerModel(_playerUnitConfig.StartHealth, _bus);
        }
    }
}