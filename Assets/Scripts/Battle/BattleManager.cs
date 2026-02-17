using System.Collections.Generic;
using System.Linq;
using Battle.Database;
using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private UnitDatabase _unitDatabase;
        [SerializeField] private TeamDatabase _teamDatabase;
        [SerializeField] private List<Transform> _teamPointsA;
        [SerializeField] private List<Transform> _teamPointsB;
        [SerializeField] private BattleVisualizer _visualizer;
        
        private BattleSimulator _simulator;
        private bool _isComplete;

        private void Start()
        {
            StartBattle(1, 2);
        }        
        
        private void StartBattle(int teamA, int teamB)
        {
            var unitInfos = GetUnitInfos(teamA, teamB);

            // 시뮬레이터 생성
            _simulator = new BattleSimulator(123, unitInfos);

            // 초기 유닛들 소환
            _visualizer.SpawnInitialUnits(unitInfos);
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

        private List<UnitState> GetUnitInfos(int teamA, int teamB)
        {
            var unitInfos = new List<UnitState>();
            var teamAData = _teamDatabase.teams.FirstOrDefault(i => i.teamId == teamA);
            var teamBData = _teamDatabase.teams.FirstOrDefault(i => i.teamId == teamB);
            if (teamAData == null || teamBData == null) 
                return unitInfos;

            var id = 0;
            for (var i = 0; i < teamAData.unitIds.Count; i++)
            {
                var unitId = teamAData.unitIds[i];
                var unitData = _unitDatabase.units.FirstOrDefault(data => data.unitId == unitId);
                if (unitData == null) continue;
                var unitState = new UnitState(id++, 0, _teamPointsA[i].position, unitData);
                unitInfos.Add(unitState);
            }

            for (var i = 0; i < teamBData.unitIds.Count; i++)
            {
                var unitId = teamBData.unitIds[i];
                var unitData = _unitDatabase.units.FirstOrDefault(data => data.unitId == unitId);
                if (unitData == null) continue;
                var unitState = new UnitState(id++, 1, _teamPointsB[i].position, unitData);
                unitInfos.Add(unitState);
            }

            return unitInfos;
        }
    }
}