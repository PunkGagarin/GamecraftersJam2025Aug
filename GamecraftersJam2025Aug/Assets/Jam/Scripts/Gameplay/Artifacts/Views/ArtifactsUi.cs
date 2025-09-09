using System;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Artifacts.Views
{
    public class ArtifactsUi : MonoBehaviour
    {
        [field: SerializeField]
        public Button TestButton { get; private set; }

        [field: SerializeField]
        public ArtifactType ArtifactType { get; private set; }

        [field: SerializeField]
        public Transform Parent { get; private set; }

        [field: SerializeField]
        public ArtifactView ArtifactViewPrefab { get; private set; }

        public event Action<ArtifactType> OnArtifactAdded = delegate { };

        private void Awake()
        {
            TestButton.onClick.AddListener(AddArt);
        }

        private void AddArt()
        {
            OnArtifactAdded.Invoke(ArtifactType);
        }


        public void AddArtifact(ArtifactDto dto)
        {
            ArtifactView view = Instantiate(ArtifactViewPrefab, Parent);
            view.Init(dto);
        }
    }
}