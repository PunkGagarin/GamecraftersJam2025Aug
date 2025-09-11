using UnityEngine;
using Zenject;

namespace Jam.Prefabs.Gameplay.Gold
{
    public class GoldInstaller : MonoInstaller
    {
        [field: SerializeField]
        public GoldUI GoldUI { get; private set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GoldUI>()
                .FromInstance(GoldUI)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<GoldBus>().AsSingle();
            Container.BindInterfacesAndSelfTo<GoldPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<GoldService>().AsSingle();
        }
    }
}