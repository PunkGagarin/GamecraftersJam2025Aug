using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gamedesign
{
    public class GamedesignInstaller : MonoInstaller
    {
        [SerializeField]
        private GamedesignUI _gamedesignUI;
        
        [SerializeField]
        private GamedesignTestSo _testSo;
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GamedesignUI>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GamedesignTestSo>().FromInstance(_testSo).AsSingle().NonLazy();
        }
    }
}