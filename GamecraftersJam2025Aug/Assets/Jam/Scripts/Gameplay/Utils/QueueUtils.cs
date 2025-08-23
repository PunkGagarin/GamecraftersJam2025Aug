using System;
using System.Collections.Generic;

public static class QueueUtils
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this Queue<T> queue)
    {
        // Перекладываем в список
        var list = new List<T>(queue);
        queue.Clear();

        // Алгоритм Фишера–Йетса
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }

        // Заполняем обратно
        foreach (var item in list)
        {
            queue.Enqueue(item);
        }
    }
}