using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [field: SerializeField]
        public BallDescriptionUi DescriptionUi { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BallDescriptionUi>()
                .FromInstance(DescriptionUi)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<FirstRoomStarter>().AsSingle();
        }
    }
}