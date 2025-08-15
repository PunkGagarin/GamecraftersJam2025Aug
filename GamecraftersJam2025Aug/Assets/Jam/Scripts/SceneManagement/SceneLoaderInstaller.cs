using Zenject;

namespace Jam.Scripts.SceneManagement
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromNew().AsSingle().NonLazy();
        }
    }
}
