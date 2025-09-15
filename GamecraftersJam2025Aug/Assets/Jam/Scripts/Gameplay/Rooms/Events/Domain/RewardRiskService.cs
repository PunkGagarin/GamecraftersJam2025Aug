using System.Collections.Generic;
using System.Linq;
using Jam.Prefabs.Gameplay.Gold;
using Jam.Scripts.Gameplay.Artifacts;
using Jam.Scripts.Gameplay.Inventory;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Rooms.Battle.Player;
using Jam.Scripts.Gameplay.Rooms.Events.BallRisk;
using Jam.Scripts.Gameplay.Rooms.Events.DamageRisk;
using Jam.Scripts.Gameplay.Rooms.Events.GoldRisk;
using Jam.Scripts.Gameplay.Rooms.Events.MaxHpIncreaseReward;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Gameplay.Rooms.Events.Domain
{
    public class RewardRiskService
    {
        [Inject] private LocalizationTool _localizationTool;
        [Inject] private PlayerInventoryService _playerInventoryService;
        [Inject] private BallDescriptionGenerator _ballDescriptionGenerator;
        [Inject] private ArtifactService _artifactService;
        [Inject] private PlayerBattleService _playerBattleService;
        [Inject] private GoldService _goldService;
        [Inject] private BallsGenerator _ballsGenerator;
        [Inject] private RoomEventBus _roomEventBus;
        [Inject] private RoomEventConfig _config;

        private const string GOLD_DESC_KEY = "REWARD_GOLD";
        private const string MAX_HP_DESC_KEY = "REWARD_MAX_HP";
        private const string HEAL_DESC_KEY = "REWARD_HEAL";
        private const string DAMAGE_DESC_KEY = "REWARD_DAMAGE";

        public RewardUiData GetRewardsForReward(RoomRewardEvent roomRewardEvent)
        {
            var rewards = roomRewardEvent.RewardsList
                .Select(GetRewardByType)
                .ToList();
            var clownMonologueStrings = GetClownStrings(roomRewardEvent.ClownMonologueKeys);
            return new RewardUiData(roomRewardEvent.Sprite, rewards, clownMonologueStrings);
        }

        public DealUiData GetRewardsAndRiskForDeal(RoomDealEvent roomDealEvent)
        {
            var clownMonologueStrings = GetClownStrings(roomDealEvent.ClownMonologueKeys);
            DealUiData dealUiData = new DealUiData(roomDealEvent.Sprite, clownMonologueStrings);
            foreach (var roomDealData in roomDealEvent.DealData)
            {
                var btnText = _localizationTool.GetText(roomDealData.ActionDescKey);
                IRewardCardUiData rewardCard = GetRewardByType(roomDealData.RewardType);
                IRiskCardUiData risk = GetRiskByType(roomDealData.RiskType);
                var data = new DealButtonData(btnText, rewardCard, risk);
                dealUiData.Buttons.Add(data);
            }

            return dealUiData;
        }

        private List<string> GetClownStrings(List<string> clownMonologueKeys) =>
            clownMonologueKeys.Select(clownMonologueKey => _localizationTool.GetText(clownMonologueKey)).ToList();

        public void ProcessReward(IRewardCardUiData rewardCardUiData)
        {
            Debug.Log($"Reward received: {rewardCardUiData}");
            switch (rewardCardUiData)
            {
                case RandomBallRewardCardUiData data:
                    AddBallToPlayer(data.SelectedBall.BallType, data.SelectedBall.Grade); break;
                case ConcreteBallRewardCardUiData data:
                    AddBallToPlayer(data.BallReward.Type, data.BallReward.Grade); break;
                case GoldRewardCardUiData data: AddGoldToPlayer(data.Value); break;
                case HealRewardCardUiData data: HealPlayer(data.Value); break;
                case MaxHpIncreaseRewardCardUiData data: IncreasePlayerMaxHp(data.Value); break;
                case ArtifactRewardCardUiData data: AddArtifactToPlayer(data.Type); break;
            }
        }

        public void ProcessRisk(IRiskCardUiData dataRisk)
        {
            Debug.Log($"Risk received: {dataRisk}");
            switch (dataRisk)
            {
                case BallLoseRiskCardUiData data: RemoveBallFromPlayer(data); break;
                case DamageRiskCardUiData data: TakeDamageFromPlayer(data.Value); break;
                case GoldRiskCardUiData data: TakeGoldFromPlayer(data.Value); break;
                case MaxHpDecreaseRiskCardUiData data: DecreasePlayerMaxHp(data.Value); break;
            }
        }

        public RewardUiData GetChestReward()
        {
            var chestSprite = _config.ChestSprite;
            var rewards = new List<RoomRewardEventData>()
                {
                    new ArtifactRewardData(),
                    new GoldRewardData
                    {
                        Amount = GetRandomGoldAmount(),
                        Sprite = _config.GoldSprite
                    }
                }
                .Select(GetRewardByType)
                .ToList();
            return new RewardUiData(chestSprite, rewards, new List<string>());
        }

        private int GetRandomGoldAmount()
        {
            var defaultGoldAmount = _config.DefaultGoldAmount;
            int goldGap = _config.Gap;
            int min = (int)(defaultGoldAmount * (1 - goldGap / 100f));
            int max = (int)(defaultGoldAmount * (1 + goldGap / 100f));
            int amount = Random.Range(min, max);
            return amount;
        }

        private IRiskCardUiData GetRiskByType(RoomRiskEventData risk)
        {
            var icon = risk.Sprite;
            var desc = "";
            switch (risk)
            {
                case BallRiskData p:
                {
                    var randomPlayerBall = _playerInventoryService.GetRandomPlayerBall();
                    var ball = new BallRewardCardUiData(icon, desc, randomPlayerBall.Type, randomPlayerBall.Grade);
                    _ballDescriptionGenerator.AddEffectsDescriptionTo(randomPlayerBall.Effects, ball);
                    return new BallLoseRiskCardUiData(ball);
                }
                case DamageRiskData p:
                    desc = FormatRiskDesc(p.Value, DAMAGE_DESC_KEY);
                    return new DamageRiskCardUiData(icon, desc, p.Value);
                case GoldRiskData p:
                    desc = FormatRiskDesc(p.Value, GOLD_DESC_KEY);
                    return new GoldRiskCardUiData(icon, desc, p.Value);
                case MaxHpDecreaseRiskData p:
                    desc = FormatRiskDesc(p.Value, MAX_HP_DESC_KEY);
                    return new MaxHpDecreaseRiskCardUiData(icon, desc, p.Value);
                default:
                    return null;
            }
        }

        private IRewardCardUiData GetRewardByType(RoomRewardEventData reward)
        {
            var icon = reward.Sprite;
            var desc = "";
            switch (reward)
            {
                case RandomBallRewardData:
                {
                    List<BallRewardCardUiData> rewards = new();
                    for (int i = 0; i < _config.BallsCountForRandomBall; i++)
                    {
                        var data = GetRandomBall();
                        rewards.Add(data);
                    }

                    return new RandomBallRewardCardUiData(rewards);
                }
                case ConcreteBallRewardData p:
                {
                    var grade = Random.Range(1, 3);
                    var ball = _ballsGenerator.CreateBallRewardDtoFrom(p.ConcreteBall.BallType, grade);
                    return new ConcreteBallRewardCardUiData(ball);
                }
                case GoldRewardData p:
                    desc = FormatRewardDesc(p.Amount, GOLD_DESC_KEY);
                    return new GoldRewardCardUiData(icon, desc, p.Amount);
                case MaxHpIncreaseRewardData p:
                    desc = FormatRewardDesc(p.Value, MAX_HP_DESC_KEY);
                    return new GoldRewardCardUiData(icon, desc, p.Value);
                case HealRewardData p:
                    desc = FormatRewardDesc(p.Value, HEAL_DESC_KEY);
                    return new HealRewardCardUiData(icon, desc, p.Value);
                case ArtifactRewardData p:
                {
                    var artifactType = _artifactService.GetRandomArtifactType();
                    var artifactDtoByType = _artifactService.GetArtifactDtoByType(artifactType);
                    icon = artifactDtoByType.Sprite;
                    desc = artifactDtoByType.Description;
                    return new ArtifactRewardCardUiData(icon, desc, artifactType);
                }
                case BallUpgradeRewardData p:
                    var prevBall = GetRandomPlayerBallWithGrade(1, out var upgradedBall);
                    if (prevBall == null || upgradedBall == null)
                        return GetDefaultGoldReward();
                    var prevValue =
                        new BallRewardCardUiData(prevBall.Icon, prevBall.Desc, prevBall.Type, prevBall.Grade);
                    var newValue = new BallRewardCardUiData(upgradedBall.Icon, upgradedBall.Desc, upgradedBall.Type,
                        upgradedBall.Grade);
                    return new BallUpgradeRewardCardUiData(prevValue, newValue, icon, desc);
                default: return null;
            }
        }

        private IRewardCardUiData GetDefaultGoldReward()
        {
            var gold = new GoldRewardData
            {
                Amount = GetRandomGoldAmount(),
                Sprite = _config.GoldSprite
            };
            return new GoldRewardCardUiData(gold.Sprite, FormatRewardDesc(gold.Amount, GOLD_DESC_KEY), gold.Amount);
        }


        private void AddArtifactToPlayer(ArtifactType dataType) =>
            _artifactService.AddArtifact(dataType);

        private void AddGoldToPlayer(float value) =>
            _goldService.AddGold(Mathf.RoundToInt(value));

        private void IncreasePlayerMaxHp(float value) =>
            _playerBattleService.IncreaseMaxHp(Mathf.RoundToInt(value));

        private void HealPlayer(float value) =>
            _playerBattleService.Heal(Mathf.RoundToInt(value));

        private void AddBallToPlayer(BallType ballType, int ballRewardGrade)
        {
            _roomEventBus.BallSelected(ballType, ballRewardGrade);
        }

        private void RemoveBallFromPlayer(BallLoseRiskCardUiData data)
        {
            Debug.Log($"RemoveBallFromPlayer {data.BallReward.Type} {data.BallReward.Grade}");
            var ball = _playerInventoryService.GetPlayerBall(data.BallReward.Type, data.BallReward.Grade);
            _playerInventoryService.RemoveBall(ball);
        }

        private void TakeDamageFromPlayer(float value)
        {
            _playerBattleService.TakeNonLethalDamage(Mathf.RoundToInt(value));
        }

        private void TakeGoldFromPlayer(float value)
        {
            var currentGoldAmount = _goldService.GetCurrentGoldAmount();
            _goldService.RemoveGold(currentGoldAmount < value ? currentGoldAmount : Mathf.RoundToInt(value));
        }

        private void DecreasePlayerMaxHp(float value)
        {
            Debug.Log($"DecreasePlayerMaxHp {value}");
            _playerBattleService.DecreaseMaxHp(Mathf.RoundToInt(value));
        }

        private BallRewardCardUiData GetRandomPlayerBallWithGrade(int grade, out BallRewardCardUiData newBall)
        {
            var result = _playerInventoryService.UpdateRandomPlayerBallWithGrade(grade, out var upgradedBall);
            newBall = upgradedBall;
            if (result == null || upgradedBall == null)
            {
                newBall = null;
                return null;
            }
            var prevModel = _ballsGenerator.CreateBallFor(result.Type, result.Grade);
            _ballDescriptionGenerator.AddEffectsDescriptionTo(prevModel.Effects, result);
            var newModel = _ballsGenerator.CreateBallFor(newBall.Type, newBall.Grade);
            _ballDescriptionGenerator.AddEffectsDescriptionTo(newModel.Effects, newBall);
            return result;
        }

        private BallRewardCardUiData GetRandomBall() =>
            _ballsGenerator.CreateRandomBallRewardDto();

        private string FormatRewardDesc(float value, string key)
        {
            var unit = _localizationTool.GetText(key);
            return $"+{Mathf.RoundToInt(value)} {unit}";
        }

        private string FormatRiskDesc(float value, string key)
        {
            var unit = _localizationTool.GetText(key);
            return $"-{Mathf.RoundToInt(value)} {unit}";
        }
    }
}