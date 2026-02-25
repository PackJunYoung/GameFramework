using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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
        public float attackRange;
        public float moveSpeed;
        public float preAttackDelay, postAttackDelay;
        public AttackType attackType;
        public GameObject prefab;
        
        public float hp;                // 체력
        public float attack;            // 공격력
        public float pDefense;          // 물리 방어
        public float mDefense;          // 마법 방어
        public float attackSpeed;       // 공격 속도
        public float cooldownReduction; // 스킬 쿨타임 감소
        public float penetration;       // 관통
        public float criticalRate;      // 치명타 확률
        public float criticalDamage;    // 치명타 피해
        public float criticalResist;    // 치명타 저항
        public float accuracy;          // 명중률
        public float evasion;           // 회피율
        public float blockPenetration;  // 막기 관통
        public float blockRate;         // 막기 확률
    }
}