using Zenject;

namespace Jam.Scripts.Utils.Pause
{
    public class PauseServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<PauseService>()
                .AsSingle()
                .NonLazy();
        }
    }
}