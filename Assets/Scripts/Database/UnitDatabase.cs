using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Attack;
using Battle.Stat;
using UnityEngine;

namespace Database
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

    public enum AttackType
    {
        P = 0,  // 물리 공격
        M,      // 마법 공격
    }

    [Serializable]
    public class UnitData
    {
        public string unitId;        
        public AttackType attackType;
        public StatSet statSet;
        public AttackPattern baseAttackPattern;
        public AttackPattern specialAttackPattern;
        public GameObject prefab;
    }
}