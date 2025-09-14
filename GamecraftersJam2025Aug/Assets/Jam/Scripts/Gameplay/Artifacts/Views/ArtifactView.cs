using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Artifacts.Views
{
    public class ArtifactView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [field: SerializeField] 
        public Image Image { get; private set; }
        //
        // [field: SerializeField] 
        // public TextMeshProUGUI DescriptionText { get; private set; }  
        //
        // [field: SerializeField] 
        // public TextMeshProUGUI ArtifactTypeText { get; private set; }

        public event Action<string> OnMouseEnter = delegate { };
        public event Action OnMouseExit = delegate { };
        
        private ArtifactDto _artifactDto;

        public void Init(ArtifactDto dto)
        {
            _artifactDto = dto;
            Image.sprite = dto.Sprite;
            // DescriptionText.text = dto.Description;
            // ArtifactTypeText.text = dto.Type.ToString();
        }
        
        public void OnPointerEnter(PointerEventData eventData) => OnMouseEnter?.Invoke(_artifactDto.Description);
        public void OnPointerExit(PointerEventData eventData) => OnMouseExit?.Invoke();
    }
}