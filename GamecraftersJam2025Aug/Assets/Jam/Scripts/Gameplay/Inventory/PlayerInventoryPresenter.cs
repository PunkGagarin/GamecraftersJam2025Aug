using System;
using Jam.Scripts.Gameplay.Inventory.Models;
using Zenject;

namespace Jam.Scripts.Gameplay.Inventory
{
    public class PlayerInventoryPresenter : IInitializable, IDisposable
    {
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        [Inject] private readonly InventoryBus _bus;
        // [Inject] private readonly PlayerInventoryView _view;


        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
        
        
    }

}