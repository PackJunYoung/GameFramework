using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BattleVisualizer _visualizer;
        [SerializeField] private BattleContainer _container;
        
        private BattleSimulator _simulator;
        private bool _isComplete;

        private void Start()
        {
            StartBattle(1, 2);
        }        
        
        private void StartBattle(int teamA, int teamB)
        {
            var unitInfos = _container.GetUnitInfos(teamA, teamB);

            // 초기 유닛들 소환
            _visualizer.SpawnInitialUnits(unitInfos);
            
            // 시뮬레이터 생성
            _simulator = new BattleSimulator(123, unitInfos);
        }

        private void Update()
        {
            if (_isComplete) 
                return;
            
            var dt = Time.deltaTime;
            var frameEvents = _simulator.Tick(dt);

            _visualizer.PlayEvents(frameEvents);

            var winnerTeam = _simulator.GetWinnerTeam();
            if (winnerTeam != 0)
            {
                _isComplete = true;
                Debug.Log($"팀 {winnerTeam} 승리!!!");
            }
        }
    }
}