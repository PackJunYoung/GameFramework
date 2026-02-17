using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle.Database
{
    [CreateAssetMenu(fileName = "UnitDatabase", menuName = "Battle/UnitDatabase")]
    public class UnitDatabase : ScriptableObject
    {
        public List<UnitData> units;

        public UnitData GetUnit(string unitId)
        {
            return units.FirstOrDefault(i => i.unitId == unitId);
        }
    }

    [Serializable]
    public class UnitData
    {
        public string unitId;
        public float hp, atk;
        public float moveSpeed, attackRange;
        public float preAttackDelay, hitAttackDelay, postAttackDelay;
        public GameObject prefab;
    }
}