using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models.Definitions;
using NaughtyAttributes;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Inventory.Models
{
    [CreateAssetMenu(menuName = "Gameplay/Ball/PlayerBalls", fileName = "BallSo", order = 0)]
    public class BallSo : ScriptableObject
    {
        [field: SerializeField]
        public BallType BallType { get; private set; }

        [field: Dropdown("_intValues")]
        [field: SerializeField]
        public int Grade { get; private set; } = 1;

        [field: SerializeField]
        public Sprite Sprite { get; set; }

        [field: SerializeReference]
        public List<EffectDef> Effects { get; set; } = new();

        [field: SerializeField]
        public string Description { get; set; }

        #region ContextMenu

        private int[] _intValues = { 1, 2 };

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