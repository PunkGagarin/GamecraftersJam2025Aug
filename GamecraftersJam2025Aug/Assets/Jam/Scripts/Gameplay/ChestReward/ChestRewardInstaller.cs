using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.ChestReward
{
    public class ChestRewardInstaller : MonoInstaller
    {
        [SerializeField] protected ChestRewardView _chestRewardView;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<ChestRewardView>()
                .FromInstance(_chestRewardView)
                .AsSingle()
                .NonLazy();
            Container
                .BindInterfacesAndSelfTo<ChestRewardSystem>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}