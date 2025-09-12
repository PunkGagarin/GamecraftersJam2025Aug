using Jam.Scripts.Gameplay.Battle;
using Jam.Scripts.Gameplay.Battle.Enemy;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Enemy
{
    public class EnemyModel : BaseUnit
    {
        public int Damage { get; private set; }
        public int CurrentDamage { get; private set; }
        public EnemyType Type { get; private set; }
        public Sprite EnemySprite { get; private set; }
        public EnemyTier Tier { get; }

        public EnemyModel(int damage, int health, EnemyType type, Sprite sprite, EnemyTier tier)
        {
            Damage = damage;
            CurrentDamage = damage;
            Health = health;
            MaxHealth = health;
            Type = type;
            EnemySprite = sprite;
            Tier = tier;
        }

        public void SetCurrentDamage(int boostedDamage)
        {
            CurrentDamage = boostedDamage;
        }
    }
}