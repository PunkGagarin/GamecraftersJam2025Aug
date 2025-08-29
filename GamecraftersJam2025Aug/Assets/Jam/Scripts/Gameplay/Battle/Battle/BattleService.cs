namespace Jam.Scripts.Gameplay.Battle
{
    public class BattleService
    {
    
        private BattleModel _battleModel;
    
        public void StartBattle()
        {
            _battleModel = new BattleModel();
        }
        
        public void SetBattleState(BattleState state) => _battleModel.SetBattleState(state);
    }
}