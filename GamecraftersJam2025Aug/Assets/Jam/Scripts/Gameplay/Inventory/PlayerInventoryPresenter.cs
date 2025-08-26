using System;
using Jam.Scripts.Gameplay.Inventory.Models;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    internal class PlayerInventoryPresenter
    {
        

        // private ArtifactInventoryModel _artifactInventoryModel;
    }

    public class PlayerInventoryService : IInitializable, IDisposable
    {
        [Inject] private readonly BallsGenerator _inventoryFactory;
        private BallsInventoryModel _ballsInventoryModel;
        
        public void Initialize()
        {
            _ballsInventoryModel = _inventoryFactory.CreateBallsInventoryModel();
        }

        public void Dispose()
        {
        }
    }
}