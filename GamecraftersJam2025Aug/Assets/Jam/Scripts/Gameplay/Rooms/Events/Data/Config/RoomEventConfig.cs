using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{ 
    //[CreateAssetMenu(fileName = "RoomEventConfig", menuName = "Game Resources/Configs/RoomEventConfig")]
    public class RoomEventConfig: ScriptableObject
    {
        [Header("Reward")]
        [SerializeField] public int BallsCountForRandomBall;
        [SerializeField] public int DefaultGoldAmount = 20;
        [SerializeField] public Sprite ChestSprite;
        [SerializeField] public Sprite GoldSprite;
        
        [field: Header(" рандомный ")]
        [field: SerializeField]
        public int Gap { get; private set; } = 10;

    }
}