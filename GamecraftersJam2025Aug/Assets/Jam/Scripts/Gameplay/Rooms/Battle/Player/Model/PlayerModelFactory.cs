using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Zenject;

namespace Jam.Scripts.Gameplay.Battle.Player
{
    public class PlayerModelFactory
    {
        [Inject] private PlayerUnitConfig _playerUnitConfig;

        //Config
        public PlayerModel CreatePlayer()
        {
            return new PlayerModel(_playerUnitConfig.StartHealth);
        }
    }
}