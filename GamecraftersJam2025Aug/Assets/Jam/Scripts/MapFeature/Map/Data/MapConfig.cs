using UnityEngine;
using UnityEngine.Serialization;

namespace Jam.Scripts.MapFeature.Map.Data
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Game Resources/Configs/MapConfig")]
    public class MapConfig : ScriptableObject
    {
        [Header("Structure")] 
        [SerializeField] public int Level = 2;
        [SerializeField] public int FloorsCountPerLevel = 10;
        [SerializeField] public int MaxRoomsPerFloor = 7;
        [SerializeField] public int MerchantCountFloorAppearance = 5;
        [SerializeField] public int ChestCountFloorAppearance = 3;
        [SerializeField] public float EventChance = .2f;
        [SerializeField] public float ChanceToHaveTwoRoomsOnNextFloor = 0.3f;

        [Header("Debug")] 
        [SerializeField] public bool EnableMapValidationOutput;

        [Header("Nodes Spacing")] 
        [SerializeField] public float HorizontalSpacing;
        [SerializeField] public float VerticalSpacing;

        
    }
}