using Jam.Scripts.Gameplay.Rooms.Battle;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        [field: SerializeField]
        public BallDescriptionUi DescriptionUi { get; set; }
        
        [field: SerializeField]
        public FinishGameUI FinishGameUI { get; set; }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FinishGameUI>()
                .FromInstance(FinishGameUI)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<BallDescriptionUi>()
                .FromInstance(DescriptionUi)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<TutorialSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<FirstRoomStarter>().AsSingle();
        }
    }
}