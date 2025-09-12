using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Inventory.Models.Definitions;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class BallDescriptionGenerator
    {
        [Inject] private LocalizationTool _localizationTool;
        
        private const string FIRST_TARGET_KEY = "BALL_REWARD_DESC_TARGET_FIRST";
        private const string LAST_TARGET_KEY = "BALL_REWARD_DESC_TARGET_LAST";
        private const string RANDOM_TARGET_KEY = "BALL_REWARD_DESC_TARGET_RANDOM";
        private const string PLAYER_TARGET_KEY = "BALL_REWARD_DESC_TARGET_PLAYER";
        private const string ALL_TARGET_KEY = "BALL_REWARD_DESC_TARGET_ALL";
        private const string DAMAGE_BALL_KEY = "BALL_REWARD_DESC_DAMAGE";
        private const string CRITICAL_BALL_KEY = "BALL_REWARD_DESC_CRITICAL";
        private const string HEAL_BALL_KEY = "BALL_REWARD_DESC_HEAL";
        private const string POISON_BALL_KEY = "BALL_REWARD_DESC_POISON";
        private const string SHIELD_BALL_KEY = "BALL_REWARD_DESC_SHIELD";

        public void AddEffectsDescriptionTo(List<EffectDef> ballSoEffects, BallRewardCardUiData ballRewardDto)
        {
            var effectsDesc = GetEffectsDescription(ballSoEffects);
            ballRewardDto.Desc = effectsDesc;
        }

        private string GetEffectsDescription(List<EffectDef> ballSoEffects)
        {
            string desc = "";
            foreach (var ballEffect in ballSoEffects)
            {
                var targetDescKey = GetTargetDescKey(ballEffect.Targeting);
                var targetDesc = _localizationTool.GetText(targetDescKey);
                var effectTextKey = GetEffectTextKey(ballEffect);
                var effectText = _localizationTool.GetText(effectTextKey);
                var value = GetEffectValue(ballEffect);
                desc += string.Format(effectText, targetDesc, value);
            }

            return desc;
        }


        private string GetEffectValue(EffectDef ballEffect)
        {
            var value = "";
            switch (ballEffect)
            {
                case DamageEffectDef e: value = e.Amount.ToString(); break;
                case CriticalEffectDef e: value = e.Damage.ToString(); break;
                case HealEffectDef e: value = e.Amount.ToString(); break;
                case PoisonEffectDef e: value = e.Damage.ToString(); break;
                case ShieldEffectDef e: value = e.Amount.ToString(); break;
            }

            return value;
        }

        private string GetEffectTextKey(EffectDef ballEffect)
        {
            var key = "";
            switch (ballEffect)
            {
                case DamageEffectDef: key = DAMAGE_BALL_KEY; break;
                case CriticalEffectDef: key = CRITICAL_BALL_KEY; break;
                case HealEffectDef: key = HEAL_BALL_KEY; break;
                case PoisonEffectDef: key = POISON_BALL_KEY; break;
                case ShieldEffectDef: key = SHIELD_BALL_KEY; break;
            }

            return key;
        }

        private string GetTargetDescKey(TargetType ballEffectTargeting)
        {
            var key = "";
            switch (ballEffectTargeting)
            {
                case TargetType.None:
                    break;
                case TargetType.First:
                    key = FIRST_TARGET_KEY;
                    break;
                case TargetType.All:
                    key = ALL_TARGET_KEY;
                    break;
                case TargetType.Last:
                    key = LAST_TARGET_KEY;
                    break;
                case TargetType.Random:
                    key = RANDOM_TARGET_KEY;
                    break;
                case TargetType.Player:
                    key = PLAYER_TARGET_KEY;
                    break;
            }

            return key;
        }
    }
}