using Jam.Scripts.Gameplay.Rooms.Events.Data;
using Jam.Scripts.UI;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Events.Presentation
{
    public class RoomRewardEventView : ContentUi
    {
        [Inject] private RoomRewardEventPresenter _presenter;

        public void ShowRewardEvent(RoomEvent roomEvent)
        {
            //todo
        }
    }
}