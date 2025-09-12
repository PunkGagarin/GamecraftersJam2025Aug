using System;
using System.Linq;
using Jam.Scripts.Gameplay;
using Jam.Scripts.Gameplay.Battle.Enemy;
using Jam.Scripts.Gameplay.Rooms.Battle.Enemy;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class GamedesignUI : ContentUi
{
    [Inject] private EnemyFactory _enemyFactory;
    [Inject] private EnemyConfigRepository _config;
    [Inject] private GamedesignTestSo _testSo;

    [field: SerializeField]
    private Button OpenButton { get; set; }

    [field: SerializeField]
    private Button CloseButton { get; set; }

    [field: SerializeField]
    private Button ShowEnemiesButton { get; set; }

    private void Awake()
    {
        OpenButton.onClick.AddListener(Show);
        CloseButton.onClick.AddListener(Hide);
        ShowEnemiesButton.onClick.AddListener(CreateEnemiesFor);
    }

    private void OnDestroy()
    {
        OpenButton.onClick.RemoveListener(Show);
        CloseButton.onClick.RemoveListener(Hide);
        ShowEnemiesButton.onClick.RemoveListener(CreateEnemiesFor);
    }

    public void CreateEnemiesFor()
    {
        Debug.LogError($"Creating enemies for level:  {_testSo.Level}, floor: {_testSo.Floor}");
        var enemies =
            _enemyFactory.CreateBattleWaveModel(new RoomBattleConfig(RoomType.DefaultFight, _testSo.Level,
                _testSo.Floor));
        Debug.LogError(
            $"Враги были созданы, волны: {enemies.Enemies.Keys.Count}, " +
            $"враги: {enemies.Enemies.Values.Sum(enemies => enemies.Count)}");
        string msg = "";
        for (int i = 1; i <= enemies.Enemies.Keys.Count; i++)
        {
            msg += " Волна: " + i;
            var currentWave = enemies.Enemies[i];
            foreach (var enemy in currentWave)
            {
                msg += $" {enemy.Type}";
            }
            Debug.LogError($"{msg}");
            msg = "";
        }
    }

}