using UnityEngine;

public class MyBall : MonoBehaviour
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