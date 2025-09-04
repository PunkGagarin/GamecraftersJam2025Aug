using UnityEngine;

[CreateAssetMenu(menuName = "Create GamedesignTestSo", fileName = "GamedesignTestSo", order = 0)]
public class GamedesignTestSo : ScriptableObject
{

    [field: SerializeField]
    public int Level { get; set; }

    [field: SerializeField]
    public int Floor { get; set; }

}