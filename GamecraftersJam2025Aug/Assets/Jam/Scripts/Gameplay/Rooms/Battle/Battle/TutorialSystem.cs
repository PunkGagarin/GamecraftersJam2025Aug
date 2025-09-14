using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.ShellGame;
using Jam.Scripts.Gameplay.Rooms.Battle.Systems;
using Jam.Scripts.MapFeature.Map.Data;
using Jam.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Gameplay.Rooms.Battle
{
    public class TutorialSystem : IInitializable, IDisposable
    {
        [Inject] private readonly LocalizationTool _locTool;
        [Inject] private readonly ClownMonologueController _clownMonologue;
        [Inject] private readonly FirstRoomStarter _firstRoomStarter;
        [Inject] private readonly BattleSystem _battleSystem;
        [Inject] private readonly ShellGameButtonUi _buttonUi;
        [Inject] private readonly ShellGameView _shellGameView;

        public List<string> firstActKeys = new() { "DIALOG_1_STR_1", "DIALOG_1_STR_2", "DIALOG_1_STR_3", };

        public List<string> secondActKeys = new()
        {
            "DIALOG_2_STR_1",
            "DIALOG_2_STR_2",
            "DIALOG_2_STR_3",
            "DIALOG_2_STR_4",
            "DIALOG_2_STR_5",
            "DIALOG_2_STR_6",
            "DIALOG_2_STR_7",
            "DIALOG_2_STR_8",
            "DIALOG_2_STR_9"
        };

        public List<string> secondActPartTwoKeys = new() { };
        public List<string> secondActPartThreeKeys = new() { };
        public List<string> thirdActKeys = new() { "DIALOG_3_STR_1", "DIALOG_3_STR_2" };

        public List<string> thirdActPartTwoKeys =
            new() { "DIALOG_3_STR_3", "DIALOG_6_STR_1", "DIALOG_6_STR_2", "DIALOG_6_STR_3" };

        public List<string> fourthActKeys = new() { "DIALOG_4_STR_1", "DIALOG_4_STR_2" };
        public List<string> fifthActKeys = new() { "DIALOG_5_STR_1" };
        public List<string> sixthActKeys = new() { "DIALOG_6_STR_1", "DIALOG_6_STR_2", "DIALOG_6_STR_3" };

        private string _wastutorialcompleted = "WasTutorialCompleted";

        public bool IsTutorial { get; set; } = true;
        public bool FirstTicketChoosen { get; set; }

        public void Initialize()
        {
            SetupTutorial();
            if (IsTutorial)
                Act1();
        }

        public void Dispose()
        {
        }

        private void SetupTutorial()
        {
            if (PlayerPrefs.HasKey(_wastutorialcompleted))
            {
                IsTutorial = true;
                FirstTicketChoosen = true;
            }
        }

        private void StartNextAct()
        {
        }

        public void Speak(List<string> keys)
        {
            List<string> texts = new List<string>();

            foreach (var key in keys)
                texts.Add(_locTool.GetText(key));

            _clownMonologue.ShowTextList(texts);
        }

        public void Act1()
        {
            _clownMonologue.OnDialogueCompleted += StartBattle;
            Speak(firstActKeys);
            _buttonUi.OnBallChosen += SetFirstThicketChoosen;
        }

        private void StartBattle()
        {
            Debug.Log("Start tutorial battle");
            _clownMonologue.OnDialogueCompleted -= StartBattle;
            _battleSystem.StartBattle(new RoomBattleConfig(RoomType.DefaultFight, 1, 1));
        }

        private void SetFirstThicketChoosen(int obj)
        {
            _buttonUi.OnBallChosen -= SetFirstThicketChoosen;
            FirstTicketChoosen = true;
            Speak(secondActKeys);
            _shellGameView.OnCupClicked += ReactOnFirstBallChoosen;
        }

        private void ReactOnFirstBallChoosen(CupView cupView)
        {
            if (cupView.BallView == null || cupView.BallView.UnitType == BallUnitType.None)
            {
                //reacto on empty
            }
            else
            {
                Speak(thirdActPartTwoKeys);
            }
            _shellGameView.OnCupClicked -= ReactOnFirstBallChoosen;
            FinishTutorial();
        }

        private void FinishTutorial()
        {
            PlayerPrefs.SetString(_wastutorialcompleted, "true");
        }

        //
        // public void Act2()
        // {
        //     Speak(secondActKeys);
        // }


    }

    // 1. вступительное слово
    // 2. второе вступительное слово


    // 3. нажми на билетик (показать 1 билетик)
    // 4. показывает 3 стакана 1 шар
    // 5. замешиваем шары (первый шафл)
    // 6. показываем врага (скип?)
    // 7. игрок выбирает где шарик
    // 8. если выбрал правильно - наносим урон врагу и проигрываем диалог клоуна 
    // 9. если неправильно - враг атакует игрока клоун говорит что ты лох
    // 10. показываем 1 и 2 билетики клоун говорит что-то
    // 11. повтор с атакой и замешиванием шаров
    // 12. показываем все 3 билетик
    // 13. туториал заканчивается
    // 14. при первой победе реплика клоуна


}