using System;
using System.Collections.Generic;

namespace Jam.Scripts.Gameplay.Rooms.Battle.Queue
{
    public class BattleQueueBus
    {
        public event Action<List<BallDto>> OnInit = delegate { };
        public event Action<List<int>> OnNextBallsChoosen = delegate { };
        public event Action<List<int>> OnBallsShuffled = delegate { };
        public void Init(List<BallDto> balls) => OnInit.Invoke(balls);
        public void NextBallsChoosen(List<int> ballIds) => OnNextBallsChoosen.Invoke(ballIds);
        public void BallsShuffled(List<int> ballIds) => OnBallsShuffled.Invoke(ballIds);
    }
}