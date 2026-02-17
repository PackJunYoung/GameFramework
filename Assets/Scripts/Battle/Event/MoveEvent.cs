using UnityEngine;

namespace Battle
{
    public class MoveEvent : BattleEvent
    {
        public Vector3 position;

        public static MoveEvent New(float timestamp, int unitId, Vector3 position)
        {
            return new MoveEvent()
            {
                timestamp = timestamp,
                unitId = unitId,
                position = position
            };
        }
    }
}