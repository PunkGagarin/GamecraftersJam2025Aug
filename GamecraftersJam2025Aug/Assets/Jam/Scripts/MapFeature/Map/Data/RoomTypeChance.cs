using System;
using UnityEngine;

namespace Jam.Scripts.MapFeature.Map.Data
{
    [Serializable]
    public class RoomTypeChance
    {
        [SerializeField] private int _level;
        [SerializeField] private RoomType _roomType;
        [SerializeField] private float _chance;

        public int Level => _level;
        public RoomType RoomType => _roomType;
        public float Chance => _chance;

        public RoomTypeChance(int level, RoomType roomType, float chance)
        {
            _level = level;
            _roomType = roomType;
            _chance = chance;
        }
    }
}