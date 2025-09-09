using System.Collections.Generic;
using Jam.Scripts.Gameplay.Inventory.Models;
using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events.Data
{ 
    //[CreateAssetMenu(fileName = "RoomEventConfig", menuName = "Game Resources/Configs/RoomEventConfig")]
    public class RoomEventConfig: ScriptableObject
    {
        [Header("Reward")]
        [SerializeField] public float PercentForHeal;
        [SerializeField] public int AmountOfGold;
        [SerializeField] public List<BallSo> Balls;
    }
}