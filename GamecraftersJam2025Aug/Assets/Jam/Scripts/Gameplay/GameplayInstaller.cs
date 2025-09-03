using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FirstRoomStarter>().AsSingle();
        }
    }
}