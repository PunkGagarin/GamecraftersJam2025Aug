using System;
using Jam.Scripts.Gameplay.Artifacts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Gameplay.Artifacts
{
    public class ArtifactTestView : MonoBehaviour
    {
        [field: SerializeField]
        public Button TestButton { get; private set; }

        [field: SerializeField]
        public ArtifactType ArtifactType { get; private set; }

        public event Action<ArtifactType> OnArtifactAdded = delegate { };

        private void Awake()
        {
            TestButton.onClick.AddListener(AddARt);
        }

        private void AddARt()
        {
            OnArtifactAdded.Invoke(ArtifactType);
        }
    }
}