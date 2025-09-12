using Jam.Prefabs.Gameplay.Gold;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class WinRewardSystem
    {
        [Inject] private PlayerService _playerService;
        [Inject] private BattleWinConfig _winConfig;
        [Inject] private GoldConfig _goldConfig;
        [Inject] private GoldService _goldService;


        public void Heal()
        {
            if (_goldService.HasGold(_goldConfig.HealPrice))
            {
                _goldService.RemoveGold(_goldConfig.HealPrice);
                _playerService.HealPercent(_winConfig.HealAmountPercent);
            }
            else
                Debug.LogError("Пытаемся списать золото, но у нас его не хватает");
        }

        public bool HasGoldForHeal()
        {
            return _goldService.HasGold(_goldConfig.HealPrice);
        }

        public bool HasGoldToBuyFirstGrade()
        {
            return _goldService.HasGold(_goldConfig.FirstGradeBallPrice);
        }

        public bool HasGoldForUpgrade()
        {
            return _goldService.HasGold(_goldConfig.UpgradeBallPrice);
        }

        public void TryToBuyBall(BallType dataType, int dataGrade, int dataGoldPrice)
        {
            if (_goldService.HasGold(dataGoldPrice))
            {
                _goldService.RemoveGold(dataGoldPrice);
                //addBall
            }
            else
                Debug.LogError("Пытаемся списать золото, но у нас его не хватает");
            
        }

        public bool HasGoldToBuySecondGrade()
        {
            return _goldService.HasGold(_goldConfig.SecondGradeBallPrice);
        }
    }
}