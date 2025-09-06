using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.ChestReward
{
    public class ChestRewardView : ClickableView<ChestRewardView>
    {
        private Vector3 originalScale;
        public float scaleMultiplier = 1.2f;

        void Start() => originalScale = transform.localScale;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        void OnMouseEnter() => transform.localScale = originalScale * scaleMultiplier;
        void OnMouseExit() => transform.localScale = originalScale;
    }
}