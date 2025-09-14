using System;
using Jam.Scripts.Gameplay.Rooms.Battle;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class BallDescriptionUi : ContentUi, IInitializable, IDisposable
    {

        [Inject] private BattleEventBus _eventBus;
    
        [field: SerializeField]
        public TextMeshProUGUI DescText { get; set; }
    
        public void SetDesc(string desc)
        {
            DescText.text = desc;
        }

        public void Initialize()
        {
            _eventBus.OnPlayerTurnStarted += Hide;
            _eventBus.OnWin += Hide;
        }

        public void Dispose()
        {
            _eventBus.OnWin -= Hide;
        }

        private void Hide(WinDto obj)
        {
            Hide();
        }
    }
}
