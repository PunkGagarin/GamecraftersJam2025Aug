using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Jam.Prefabs.Gameplay.Gold
{
    public class GoldUI : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI GoldText { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI AnimatedGoldText { get; private set; }

        public void UpdateUi(int diff, int newTotal)
        {
            SetGoldUi(newTotal);
            AnimateGoldTextWithDoTweenYUpAndFade(diff);
        }

        private void AnimateGoldTextWithDoTweenYUpAndFade(int diff)
        {
            AnimatedGoldText.text = diff.ToString();
            AnimatedGoldText.gameObject.SetActive(true);
            AnimatedGoldText.transform.DOLocalMoveY(AnimatedGoldText.transform.localPosition.y + 20, 1f)
                .OnComplete(() => AnimatedGoldText.gameObject.SetActive(false));
        }

        public void SetGoldUi(int gold)
        {
            GoldText.text = gold.ToString();
        }
    }
}