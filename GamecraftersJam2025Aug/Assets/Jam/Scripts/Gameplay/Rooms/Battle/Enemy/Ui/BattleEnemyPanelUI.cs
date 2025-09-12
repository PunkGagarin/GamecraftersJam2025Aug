using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Enemy
{
    public class BattleEnemyPanelUI : MonoBehaviour
    {

        [field: SerializeField]
        public List<Transform> EnemyPoints { get; private set; }

        [field: SerializeField]
        public List<EnemyView> EnemyViews { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI WaveText { get; private set; }

        public void SetWaveText(int currentWave, int maxWave) => WaveText.text = $"{currentWave}/{maxWave}";


    }
}