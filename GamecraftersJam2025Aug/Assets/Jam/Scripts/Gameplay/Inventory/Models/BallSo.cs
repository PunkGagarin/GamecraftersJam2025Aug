using System.Collections.Generic;
using Jam.Scripts.Gameplay.Configs;
using Jam.Scripts.Gameplay.Inventory.Models.Definitions;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    [CreateAssetMenu(menuName = "Gameplay/Ball/PlayerBalls", fileName = "BallSo", order = 0)]
    public class BallSo : ScriptableObject
    {
        [field: SerializeField]
        public int Damage { get; private set; }

        [field: SerializeField]
        public BallType BallType { get; private set; }

        [field: SerializeField]
        public TargetType TargetType { get; set; }

        [field: SerializeField]
        public Sprite Sprite { get; set; }

        [field: SerializeReference]
        public List<EffectDef> Effects { get; set; } = new();

        #region ContextMenu
        
        [ContextMenu("Effects/Add/Damage")]
        private void AddDamage() => Effects.Add(new DamageEffectDef());

        [ContextMenu("Effects/Add/Poison")]
        private void AddPoison() => Effects.Add(new PoisonEffectDef());

        [ContextMenu("Effects/Add/Heal")]
        private void AddHeal() => Effects.Add(new HealEffectDef());
        
        [ContextMenu("Effects/Add/Shield")]
        private void AddShield() => Effects.Add(new ShieldEffectDef());    
        
        [ContextMenu("Effects/Add/Crit")]
        private void AddCrit() => Effects.Add(new CriticalEffectDef());
        #endregion
    }

}