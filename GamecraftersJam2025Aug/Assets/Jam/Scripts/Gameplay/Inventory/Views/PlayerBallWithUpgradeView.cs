using System;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Inventory.Views
{
    public class PlayerBallWithUpgradeView : PlayerBallView, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField]
        public Button UpgradeButton { get; private set; }

        public Action<BallDto> OnBallUpgradeClicked { get; set; }

        private void Awake()
        {
            UpgradeButton.onClick.AddListener(OnBallClicked);
        }

        private void OnBallClicked()
        {
            OnBallUpgradeClicked?.Invoke(Dto);
        }

        private void OnDestroy()
        {
            UpgradeButton.onClick.RemoveListener(OnBallClicked);
        }


        public void TurnOnUpgrade()
        {
            UpgradeButton.enabled = true;
        }

        public void TurnOffUpgrade()
        {
            UpgradeButton.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.LogError("OnPointerEnter");
            // DescriptionText.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.LogError("OnPointer Exit");
            // DescriptionText.gameObject.SetActive(false);
        }
    }
}