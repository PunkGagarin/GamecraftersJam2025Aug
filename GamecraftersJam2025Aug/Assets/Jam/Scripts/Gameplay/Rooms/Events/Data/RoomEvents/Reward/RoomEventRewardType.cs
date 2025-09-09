namespace Jam.Scripts.Gameplay.Rooms.Events
{
    public enum RoomEventRewardType
    {
        None = 0,
        RandomBall = 1,
        ConcreteBall = 2,
        Gold = 3,
        HealPlayer = 4, //todo precent from max hp
        MaxHpIncrease = 5,
        // BallUpgrade = 6
        RandomFromList = 7,
    }
}