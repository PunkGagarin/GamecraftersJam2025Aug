using System.Threading.Tasks;
using DG.Tweening;
using Jam.Scripts.Gameplay.Rooms.Battle.Shared.Ui;
using Jam.Scripts.UI;
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
        public Transform EnemyGraphicPlaceholder { get; private set; }
        
        [field: SerializeField]
        public AnimationCurve AppearCurve { get; private set; }
        
        private UnitGraphic _unitGraphic;
        private Vector3 _startPosition;

        public void SetSprite(Sprite sprite) => Sprite.sprite = sprite;

        public void SetEnemyGraphic(EnemyGraphic graphic)
        {
            foreach (Transform child in EnemyGraphicPlaceholder) 
                Destroy(child.gameObject);
            _unitGraphic = Instantiate(graphic, EnemyGraphicPlaceholder);
        }
        
        public override async Task PlayAttackAnimation()
        {
            await _unitGraphic.Attack();
        }

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
            // Вращаем через анимационную кривую
            DOTween.To(
                () => 0f,                               // от 0
                t => transform.localPosition = Vector3.Lerp(_startPosition, Vector3.zero, t), // интерполяция
                1f,                                     // до 1
                .3f                                // за время
            );
        }
    }
}