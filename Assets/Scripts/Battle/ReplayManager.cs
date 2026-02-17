using System.Collections.Generic;
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
        private Queue<List<BattleEvent>> _eventQueue = new Queue<List<BattleEvent>>();

        private const float STEP = 0.033f;

        private void Start()
        {
            StartReplay(1, 2);
        }

        private void StartReplay(int teamA, int teamB)
        {
            var unitInfos = _container.GetUnitInfos(teamA, teamB);

            _visualizer.SpawnInitialUnits(unitInfos);

            _simulator = new BattleSimulator(123, unitInfos);

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
            {
                var winner = _simulator.GetWinnerTeam();
                if (winner != 0)
                {
                    Debug.Log($"팀 {winner} 승리!!!");
                }
            }
        }
    }
}
