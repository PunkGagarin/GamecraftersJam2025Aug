using DG.Tweening;
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

        public void SetAttackTextWithAnimation(int boostedDamage)
        {
            AttackText.text = boostedDamage.ToString();
            PlayAnimationIncreaseScaleAndGoUpWithReturn();
        }
        
        public void SetAttackText(int boostedDamage)
        {
            AttackText.text = boostedDamage.ToString();
        }

        private void PlayAnimationIncreaseScaleAndGoUpWithReturn()
        {
            transform.DOScale(1.2f, 0.2f).OnComplete(() => transform.DOScale(1f, 0.2f));
        }
    }
}