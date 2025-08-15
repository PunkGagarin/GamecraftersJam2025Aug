using Zenject;

namespace Jam.Scripts.Utils.Coroutine
{
    public class CoroutineHelperInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CoroutineHelper>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("CoroutineHelper")
                .AsSingle()
                .NonLazy();
        }
    }
}
