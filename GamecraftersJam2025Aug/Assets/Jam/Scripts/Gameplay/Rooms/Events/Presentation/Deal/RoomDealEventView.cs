using Jam.Scripts.Gameplay.Rooms.Events.Data;
using Jam.Scripts.UI;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomDealEventView : ContentUi
    {
        [Inject] private RoomDealEventPresenter _presenter;

        public void ShowDealEvent(DealUiData roomEvent)
        {
            //todo
        }
    }
}