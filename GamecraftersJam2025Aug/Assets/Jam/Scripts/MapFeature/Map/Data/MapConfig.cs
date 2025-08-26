using UnityEngine;

namespace Jam.Scripts.MapFeature.Map.Data
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Game Resources/Configs/MapConfig")]
    public class MapConfig : ScriptableObject
    {
        [Header("Structure")] 
        [SerializeField] public int Level = 2;
        [SerializeField] public int FloorsCountPerLevel = 10;
        [SerializeField] public int MinRoomsPerFloor = 3;
        [SerializeField] public int MaxRoomsPerFloor = 7;
        [SerializeField] public float WeightToRemoveAdditionalConnection = 0.7f;
        
        [Header("Room types chances")] 
        [SerializeField] public int MerchantCountFloorAppearance = 5;
        [SerializeField] public int ChestCountFloorAppearance = 3;
        [SerializeField] public float EventChance = .2f;

        [Header("Nodes Spacing")] 
        [SerializeField] public float HorizontalSpacing;
        [SerializeField] public float VerticalSpacing;

        
    }
}