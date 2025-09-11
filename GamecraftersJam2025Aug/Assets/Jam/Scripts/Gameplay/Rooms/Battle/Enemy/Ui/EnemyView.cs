using System;
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

        [field: SerializeField]
        public AnimationCurve AppearCurve { get; private set; }

        private Vector3 _startPosition;

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

        public void PrepareStartPosition()
        {
            transform.localPosition = new Vector3(2f, .5f, 0f);
            _startPosition = transform.localPosition;
        }

        public void OnEnable()
        {
            PlayAppearAnimation();
        }

        public void PlayAppearAnimation()
        {
            DOTween.To(
                () => 0f,
                t =>
                {
                    // t = 0..1 прогресс времени
                    float x = Mathf.Lerp(_startPosition.x, Vector3.zero.x, t);
                    float y = Mathf.Lerp(_startPosition.y, Vector3.zero.y, AppearCurve.Evaluate(t));
                    float z = 0;

                    transform.localPosition = new Vector3(x, y, z);
                },
                1f,
                .15f
            );
        }
    }
}