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

        public void AddEffectsDescriptionTo(List<EffectInstance> effects, BallRewardCardUiData ballRewardDto)
        {
            var effectsDesc = GetEffectsDescription(effects);
            ballRewardDto.Desc = effectsDesc;
        }

        private string GetEffectsDescription(List<EffectDef> ballSoEffects)
        {
            string desc = "";
            foreach (var ballEffect in ballSoEffects) 
                desc = GetEffectDescription(ballEffect.ToInstance(), desc);
            return desc;
        }

        private string GetEffectsDescription(List<EffectInstance> ballSoEffects)
        {
            string desc = "";
            foreach (var ballEffect in ballSoEffects) 
                desc = GetEffectDescription(ballEffect, desc);
            return desc;
        }

        private string GetEffectDescription(EffectInstance effectInstance, string desc)
        {
            var targetDescKey = GetTargetDescKey(effectInstance.Targeting);
            var targetDesc = _localizationTool.GetText(targetDescKey);
            var effectTextKey = GetEffectTextKey(effectInstance);
            var effectText = _localizationTool.GetText(effectTextKey);
            var value = GetEffectValue(effectInstance);
            desc += string.Format(effectText, targetDesc, value);
            return desc;
        }

        private string GetEffectValue(EffectInstance ballEffect)
        {
            var value = "";
            switch (ballEffect.Payload)
            {
                case DamagePayload e: value = e.Damage.ToString(); break;
                case CriticalDamagePayload e: value = e.Damage.ToString(); break;
                case HealPayload e: value = e.Amount.ToString(); break;
                case PoisonPayload e: value = e.Damage.ToString(); break;
                case ShieldPayload e: value = e.Amount.ToString(); break;
            }

            return value;
        }

        private string GetEffectTextKey(EffectInstance ballEffect)
        {
            var key = "";
            switch (ballEffect.Payload)
            {
                case DamagePayload: key = DAMAGE_BALL_KEY; break;
                case CriticalDamagePayload: key = CRITICAL_BALL_KEY; break;
                case HealPayload: key = HEAL_BALL_KEY; break;
                case PoisonPayload: key = POISON_BALL_KEY; break;
                case ShieldPayload: key = SHIELD_BALL_KEY; break;
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