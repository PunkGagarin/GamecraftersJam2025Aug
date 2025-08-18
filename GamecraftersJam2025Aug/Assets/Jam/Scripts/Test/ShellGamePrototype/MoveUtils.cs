using UnityEngine;
using System.Threading.Tasks;

public static class MoveUtils
{
    /// <summary>
    /// Асинхронно перемещает трансформ от startPos к endPos за duration секунд через Lerp.
    /// </summary>
    public static async Task MoveOverTime2D(Transform target, Vector2 startPos, Vector2 endPos, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector2 newPos = Vector2.Lerp(startPos, endPos, t);
            target.position = new Vector3(newPos.x, newPos.y, target.position.z);

            await Task.Yield(); // ждем следующий кадр
        }

        target.position = new Vector3(endPos.x, endPos.y, target.position.z);
    }

    /// <summary>
    /// Упрощенный вызов — берет старт из текущей позиции трансформа.
    /// </summary>
    public static Task MoveOverTime2D(Transform target, Vector2 endPos, float duration)
    {
        return MoveOverTime2D(target, target.position, endPos, duration);
    }
}