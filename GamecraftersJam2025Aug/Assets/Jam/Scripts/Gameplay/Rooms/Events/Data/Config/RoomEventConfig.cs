using UnityEngine;

namespace Jam.Scripts.Gameplay.Rooms.Events
{ 
    //[CreateAssetMenu(fileName = "RoomEventConfig", menuName = "Game Resources/Configs/RoomEventConfig")]
    public class RoomEventConfig: ScriptableObject
    {
        [Header("Reward")]
        [SerializeField] public int BallsCountForRandomBall;
    }
}