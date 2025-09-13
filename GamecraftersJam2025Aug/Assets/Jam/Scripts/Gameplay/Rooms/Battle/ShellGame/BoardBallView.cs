using Jam.Scripts.Gameplay.Rooms.Battle.Queue;
using TMPro;
using UnityEngine;

public class BoardBallView : MonoBehaviour
{
    public int BallId { get; private set; }

    [field: SerializeField]
    public SpriteRenderer Sprite { get; private set; }
    
    [field: SerializeField]
    public SpriteRenderer BallShadow { get; private set; }

    [field: SerializeField]
    public BallUnitType UnitType { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI IdText { get; private set; }

    public void Init(BallDto ball)
    {
        BallId = ball.Id;
        Sprite.sprite = ball.Sprite;
        
        if (IdText != null)
            IdText.text = ball.Id.ToString();
    }

    public void CleanUp()
    {
        BallId = 0;
        if (IdText != null)
            IdText.text = "None";
    }
}