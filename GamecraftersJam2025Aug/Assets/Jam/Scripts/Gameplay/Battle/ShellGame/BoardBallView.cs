using Jam.Scripts.Gameplay.Battle.Queue.Model;
using UnityEngine;

public class BoardBallView : MonoBehaviour
{
    public int BallId { get; private set; }
    
    [field: SerializeField]
    public SpriteRenderer Sprite { get; private set; }

    [field: SerializeField]
    public BallUnitType UnitType { get; private set; }

    public void Init(BallDto ball)
    {
        BallId = ball.Id;
        Sprite.sprite = ball.Sprite;
    }
}

public enum BallUnitType
{
    None,
    Player,
    Enemy
}