using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Utils
{
    public static class ListUtils
    {
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}