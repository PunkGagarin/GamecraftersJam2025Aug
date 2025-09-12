using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class WinRewardSystem
    {
        [Inject] private PlayerService _playerService;
        [Inject] private BattleWinConfig _winConfig;
        
        
        
        public void Heal()
        {
            _playerService.HealPercent(_winConfig.HealAmountPercent);
        }
    }
}