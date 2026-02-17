using System.Collections.Generic;
using System.Linq;
using Database;
using UnityEngine;

namespace Battle
{
    public class ReplayManager : MonoBehaviour
    {
        [SerializeField] private BattleVisualizer _visualizer;
        [SerializeField] private BattleContainer _container;
        [SerializeField] private float _playbackSpeed;

        private BattleSimulator _simulator;
        private bool _isSimulationFinished;
        private float _playbackTimer;

        // 시뮬레이션 결과(이벤트들)를 저장할 큐
        private Queue<List<BattleEvent>> _eventQueue = new Queue<List<BattleEvent>>();

        private const float STEP = 0.033f;

        private void Start()
        {
            StartReplay(1, 2);
        }

        private void StartReplay(int teamA, int teamB)
        {
            // 1. 유닛 정보 생성
            var unitInfos = GetUnitInfos(teamA, teamB);

            // 2. 비주얼라이저에 초기 위치 전달 (시뮬레이션 돌리기 전의 위치)
            _visualizer.SpawnInitialUnits(unitInfos);

            // 3. 시뮬레이터 생성
            _simulator = new BattleSimulator(123, unitInfos);

            // 4. 전투가 끝날 때까지 미리 시뮬레이션 수행
            var totalSimulatedTime = 0f;

            while (_simulator.GetWinnerTeam() == 0 && totalSimulatedTime < 180f) // 최대 3분 제한
            {
                var frameEvents = _simulator.Tick(STEP);
                _eventQueue.Enqueue(frameEvents);
                
                totalSimulatedTime += STEP;
            }

            _isSimulationFinished = true;
            Debug.Log($"시뮬레이션 완료: 총 {totalSimulatedTime:F2}초 분량");
        }

        private void Update()
        {
            // 시뮬레이션이 끝나지 않았거나, 더 이상 재생할 프레임이 없으면 중단
            if (!_isSimulationFinished || _eventQueue.Count == 0)
                return;

            _playbackTimer += Time.deltaTime * _playbackSpeed;

            while (_playbackTimer >= STEP && _eventQueue.Count > 0)
            {
                var eventsToPlay = _eventQueue.Dequeue();
                if (eventsToPlay.Count > 0)
                {
                    _visualizer.PlayEvents(eventsToPlay);
                }

                _playbackTimer -= STEP;
            }

            if (_eventQueue.Count == 0)
                CheckWinner();
        }

        private void CheckWinner()
        {
            var winner = _simulator.GetWinnerTeam();
            if (winner != 0)
            {
                Debug.Log($"팀 {winner} 승리!!!");
                enabled = false; // 업데이트 중지
            }
        }

        // GetUnitInfos 및 기타 헬퍼 메서드는 기존과 동일...
        private List<UnitState> GetUnitInfos(int teamA, int teamB)
        {
            var teamPoints = _container.GetComponentsInChildren<TeamPoint>();
            var teamPointA = teamPoints.FirstOrDefault(i => i.Index == teamA);
            var teamPointB = teamPoints.FirstOrDefault(i => i.Index == teamB);
            if (teamPointA == null || teamPointB == null)
                throw new MissingReferenceException();

            var teamDatabase = DataManager.GetTeamDatabase();
            var unitDatabase = DataManager.GetUnitDatabase();
            var unitInfos = new List<UnitState>();
            var teamAData = teamDatabase.teams.FirstOrDefault(i => i.teamId == teamA);
            var teamBData = teamDatabase.teams.FirstOrDefault(i => i.teamId == teamB);
            if (teamAData == null || teamBData == null) 
                return unitInfos;

            var id = 0;
            for (var i = 0; i < teamAData.unitIds.Count; i++)
            {
                var unitId = teamAData.unitIds[i];
                var unitData = unitDatabase.units.FirstOrDefault(data => data.unitId == unitId);
                if (unitData == null) continue;
                var unitState = new UnitState(id++, 0, teamPointA.GetPoint(i).position, unitData);
                unitInfos.Add(unitState);
            }

            for (var i = 0; i < teamBData.unitIds.Count; i++)
            {
                var unitId = teamBData.unitIds[i];
                var unitData = unitDatabase.units.FirstOrDefault(data => data.unitId == unitId);
                if (unitData == null) continue;
                var unitState = new UnitState(id++, 1, teamPointB.GetPoint(i).position, unitData);
                unitInfos.Add(unitState);
            }

            return unitInfos;
        }
    }
}
