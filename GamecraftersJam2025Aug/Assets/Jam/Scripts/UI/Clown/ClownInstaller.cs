using UnityEngine;
using Zenject;

namespace Jam.Scripts.UI.Clown
{
    public class ClownInstaller : MonoInstaller
    {
        [SerializeField] private ClownAnimationController _clownAnimationController;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<ClownEventBus>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            Container
                .Bind<ClownAnimationController>()
                .FromInstance(_clownAnimationController)
                .AsSingle()
                .NonLazy();
            Container
                .BindInterfacesAndSelfTo<ClownAnimatorPresenter>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}