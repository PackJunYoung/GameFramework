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

        private void Start()
        {
            StartLiveBattle(1, 2);
        }        
        
        private void StartLiveBattle(int teamA, int teamB)
        {
            var unitInfos = GetUnitInfos(teamA, teamB);

            // 시뮬레이터 생성 (연산 준비만 완료)
            _simulator = new BattleSimulator(123, unitInfos);

            // 초기 유닛들 소환 (Visualizer)
            _visualizer.SpawnInitialUnits(unitInfos);
        }

        private void Update()
        {
            // [핵심] 이번 프레임(DeltaTime)만큼만 로직을 진행시킵니다.
            // 유저가 버튼을 눌렀다면 _inputQueue에 데이터가 담겨있을 것입니다.
            var dt = Time.deltaTime;
            var frameEvents = _simulator.Tick(dt);

            // 이번 틱(0.016초 등)에 발생한 사건만 즉시 연출합니다.
            _visualizer.PlayInstantEvents(frameEvents);
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