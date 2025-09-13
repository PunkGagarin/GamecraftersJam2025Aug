using System;
using System.Collections.Generic;
using Jam.Scripts.Gameplay.Rooms.Battle.Queue;

namespace Jam.Scripts.Gameplay.Battle.ShellGame
{
    public class ShellGameEventBus
    {
        public event Action OnShellGameFinished = delegate { };
        public event Action OnInit = delegate { };
        
        public void ShellGameFinishedInvoke() => OnShellGameFinished.Invoke();
        public void InitInvoke() => OnInit.Invoke();
    }
}