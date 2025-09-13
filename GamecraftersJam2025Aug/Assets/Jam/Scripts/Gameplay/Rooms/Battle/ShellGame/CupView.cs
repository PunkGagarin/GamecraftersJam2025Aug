using System;
using UnityEngine;

public class CupView : ClickableView<CupView>
{

    [field: SerializeField]
    public SpriteRenderer Sprite { get; private set; }

    public BoardBallView BallView { get; private set; }

    public void SetBall(BoardBallView ballView)
    {
        BallView = ballView;
        BallView.transform.parent = transform;
        BallView.transform.localPosition = Vector3.zero + new Vector3(0, -.3f, 0);
    }

    public void ShowBall()
    {
        var oldColor = Sprite.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.2f);

        Sprite.color = newColor;

        // var oldOutColor = OutlineSprite.color;
        // Color newOutColor = new Color(oldOutColor.r, oldOutColor.g, oldOutColor.b, 0.2f);

        // OutlineSprite.color = newOutColor;
    }

    public void HideBall()
    {
        var oldColor = Sprite.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
        Sprite.color = newColor;

        // var oldOutColor = OutlineSprite.color;
        // Color newOutColor = new Color(oldOutColor.r, oldOutColor.g, oldOutColor.b, 1f);

        // OutlineSprite.color = newOutColor;
    }

    public void MadeColorRandom()
    {
        Sprite.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f), 1f);
    }

    public void RemoveBall()
    {
        if (BallView == null) return;

        BallView.transform.parent = null;
        BallView.transform.localPosition = Vector3.zero + new Vector3(0, 0, 0);
        BallView = null;
    }
}