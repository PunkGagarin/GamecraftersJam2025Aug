namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleModel
    {
        public BattleState BattleState { get; private set; }

        public void SetBattleState(BattleState state) => BattleState = state;
    }
}