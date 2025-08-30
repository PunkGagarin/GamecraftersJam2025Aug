using UnityEngine;

public class BallView : MonoBehaviour
{

    [field: SerializeField]
    public MyBallType Type { get; private set; }
}

public enum MyBallType
{
    None,
    Player,
    Enemy
}