using UnityEngine;

namespace Jam.Scripts.Gameplay.Battle.Enemy
{
    public class EnemyModel : BaseUnit
    {
        public int Damage { get; private set; }
        public int CurrentDamage { get; private set; }
        public EnemyType Type { get; private set; }
        public Sprite EnemySprite { get; private set; }

        public EnemyModel(int damage, int health, EnemyType type, Sprite sprite)
        {
            Damage = damage;
            CurrentDamage = damage;
            Health = health;
            MaxHealth = health;
            Type = type;
            EnemySprite = sprite;
        }

        public void SetCurrentDamage(int boostedDamage)
        {
            CurrentDamage = boostedDamage;
        }
    }
}