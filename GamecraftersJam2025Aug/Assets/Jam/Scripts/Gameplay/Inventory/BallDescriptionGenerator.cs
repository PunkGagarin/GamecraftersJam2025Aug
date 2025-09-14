using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models;
using Jam.Scripts.Gameplay.Inventory.Models.Definitions;
using Jam.Scripts.Gameplay.Rooms.Events.Presentation;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class BallDescriptionGenerator
    {
        [Inject] private LocalizationTool _localizationTool;

        public void AddEffectsDescriptionTo(List<EffectDef> ballSoEffects, BallRewardCardUiData ballRewardDto)
        {
            var effectsDesc = GetEffectsDescription(ballSoEffects, ballRewardDto.Type);
            ballRewardDto.Desc = effectsDesc;
        }

        public void AddEffectsDescriptionTo(List<EffectInstance> ballSoEffects, PlayerBallModel ballRewardDto)
        {
            var effectsDesc = GetEffectsDescription(ballSoEffects, ballRewardDto.Type);
            ballRewardDto.Description = effectsDesc;
        }

        public void AddEffectsDescriptionTo(List<EffectInstance> effects, BallRewardCardUiData ballRewardDto)
        {
            var effectsDesc = GetEffectsDescription(effects, ballRewardDto.Type);
            ballRewardDto.Desc = effectsDesc;
        }

        private string GetEffectsDescription(List<EffectDef> ballSoEffects, BallType type)
        {
            string desc = "<b>" + type + "</b>\n";
            foreach (var ballEffect in ballSoEffects)
                desc = GetEffectDescription(ballEffect.ToInstance(), desc);
            return desc;
        }

        private string GetEffectsDescription(List<EffectInstance> ballSoEffects, BallType type)
        {
            string desc = "<b>" + type + "</b>\n";
            foreach (var ballEffect in ballSoEffects)
                desc = GetEffectDescription(ballEffect, desc);
            return desc;
        }

        private string GetEffectDescription(EffectInstance effectInstance, string desc)
        {
            var key = GetEffectTextKey(effectInstance);
            var effectText = _localizationTool.GetText(key) + "\n";
            var value = GetEffectValue(effectInstance);
            desc += string.Format(effectText, value);
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

        private string GetEffectTextKey(EffectInstance effectInstance)
        {
            var payloadPart = effectInstance.Payload switch
            {
                DamagePayload => "DAMAGE",
                CriticalDamagePayload => "CRIT",
                HealPayload => "HEAL",
                PoisonPayload => "POISON",
                ShieldPayload => "SHIELD",
                _ => "UNKNOWN"
            };

            var targetPart = effectInstance.Targeting switch
            {
                TargetType.First => "FIRST",
                TargetType.Last => "LAST",
                TargetType.Random => "RANDOM",
                TargetType.Player => "PLAYER",
                TargetType.All => "ALL",
                _ => "NONE"
            };

            return $"BALL_REWARD_DESC_{payloadPart}_{targetPart}";
        }

    }
}