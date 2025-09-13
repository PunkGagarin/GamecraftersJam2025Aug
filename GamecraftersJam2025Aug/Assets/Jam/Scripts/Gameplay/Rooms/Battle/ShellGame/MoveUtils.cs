using UnityEngine;
using Cysharp.Threading.Tasks;

public static class MoveUtils
{
    /// <summary>
    /// Асинхронно перемещает трансформ от startPos к endPos за duration секунд через Lerp.
    /// </summary>
    public static async UniTask MoveOverTime2D(Transform target, Vector2 startPos, Vector2 endPos, float duration)
    {
        float startTime = Time.time;

        while (true)
        {
            float t = (Time.time - startTime) / duration;

            if (t >= 1f)
                break;

            Vector2 newPos = Vector2.Lerp(startPos, endPos, t);
            target.position = new Vector3(newPos.x, newPos.y, target.position.z);

            await UniTask.Yield();
        }

        // гарантируем финал
        target.position = new Vector3(endPos.x, endPos.y, target.position.z);
    }

    /// <summary>
    /// Упрощенный вызов — берет старт из текущей позиции трансформа.
    /// </summary>
    public static UniTask MoveOverTime2D(Transform target, Vector2 endPos, float duration)
    {
        return MoveOverTime2D(target, target.position, endPos, duration);
    }
}