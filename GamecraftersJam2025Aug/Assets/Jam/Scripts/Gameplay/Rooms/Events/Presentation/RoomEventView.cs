using Jam.Scripts.Gameplay.Rooms.Events.Data;
using Jam.Scripts.UI;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomEventView : ContentUi
    {
        [Inject] private RoomEventPresenter _presenter;

        public void ShowEvent(RoomEvent roomEvent)
        {
            //todo
        }
    }
}