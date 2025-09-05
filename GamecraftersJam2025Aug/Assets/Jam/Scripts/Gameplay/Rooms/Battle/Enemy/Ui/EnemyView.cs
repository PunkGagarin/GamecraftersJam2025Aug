using Jam.Scripts.Gameplay.Rooms.Battle.Shared.Ui;
using TMPro;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Enemy
{
    public class EnemyView : BaseUnitView
    {
        [field: SerializeField]
        public TextMeshProUGUI AttackText { get; private set; }
        
        [field: SerializeField]
        public SpriteRenderer Sprite { get; private set; }
        
        public void SetSprite(Sprite sprite) => Sprite.sprite = sprite;

        public void Init(int maxHealth, int attack)
        {
            base.Init(maxHealth);
            AttackText.text = attack.ToString();
        }
    }
}