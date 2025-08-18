using System;
using UnityEngine;

public class Thimble : ClickableView<Thimble>
{

    [field: SerializeField]
    public SpriteRenderer Sprite { get; private set; }

    public MyBall MyBall { get; private set; }

    public void SetBall(MyBall myBall)
    {
        MyBall = myBall;
        MyBall.transform.parent = transform;
        MyBall.transform.localPosition = Vector3.zero;
    }

    public void ShowBall()
    {
        var oldColor = Sprite.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.2f);

        Sprite.color = newColor;
    }

    public void HideBall()
    {
        var oldColor = Sprite.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
        Sprite.color = newColor;
    }

    public void MadeColorRandom()
    {
        Sprite.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f), 1f);
    }
}