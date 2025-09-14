using System;

namespace Jam.Scripts.Gameplay.Rooms.Battle.ShellGame
{
    public class ShellGameEventBus
    {
        public event Action OnShellGameFinished = delegate { };
        public event Action OnInit = delegate { };
        
        public void ShellGameFinishedInvoke() => OnShellGameFinished.Invoke();
        public void InitInvoke() => OnInit.Invoke();
    }
}