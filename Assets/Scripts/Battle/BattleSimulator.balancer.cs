using System.Collections.Generic;
using Database;
using UnityEngine;

namespace Battle
{
    public partial class BattleSimulator
    {
        // 공격자 우위나 밸런스 튜닝을 위해 외부에서 조절 가능한 값들입니다.
         private const float accuracyWeight = 2.0f;         // 명중 가중치
         private const float criticalWeight = 2.0f;         // 치명타 가중치
         private const float blockPenetrationWeight = 2.0f; // 막기 관통 가중치
     
         // --- 밸런스 상수 (Constants) ---
         private const float defenseConstant = 50f;         // 방어력 효율 기준점
         private const float criticalConstant = 100f;       // 치명타 효율 기준점
         
         private HitResult Calculate(UnitState attacker, UnitState target, float skillMultiplier = 1.0f)
         {
             var result = new HitResult();
 
             // 1. 명중/회피 판정 (가중치 기반)
             var hitScore = attacker.Accuracy * accuracyWeight;
             var missChance = target.Evasion / (target.Evasion + hitScore);
             
             if (_random.NextDouble() < missChance)
             {
                 result.isMissed = true;
                 return result;
             }
 
             // 2. 레벨 비례 방어력 상수 (Pure Level Scaling Constant)
             // 보정 없이 레벨에 특정 계수(예: 50)만 곱합니다.
             // 레벨 1이면 상수 50, 레벨 100이면 상수 5000이 됩니다.
             var dynamicConstant = (1 + attacker.level) * defenseConstant; 
 
             // 3. 데미지 감소율 산출
             var defense = attacker.AttackType == AttackType.P ? target.PDefense : target.MDefense;
             var effectiveDefense = Mathf.Max(0, defense - attacker.Penetration);
             var damageReduction = dynamicConstant / (dynamicConstant + effectiveDefense);
 
             // 4. 막기 판정 (막기 관통 가중치 적용)
             var blockPenetrationScore = attacker.BlockPenetration * blockPenetrationWeight;
             var finalBlockChance = target.BlockRate / (target.BlockRate + blockPenetrationScore);
             
             result.isBlocked = _random.NextDouble() < finalBlockChance;
             var blockMultiplier = result.isBlocked ? 0.5f : 1.0f;
 
             // 5. 치명타 판정 (치명타 가중치 적용)
             var criticalScore = attacker.CriticalRate * criticalWeight;
             var finalCriticalChance = criticalScore / (criticalScore + target.CriticalResist);
             
             result.isCritical = _random.NextDouble() < finalCriticalChance;
             var criticalMultiplier = result.isCritical ? (1f + attacker.CriticalDamage / criticalConstant) : 1.0f;
 
             // 6. 최종 피해량 합산
             var baseDamage = attacker.Attack * skillMultiplier;
             var damageModifier = 1.0f + (attacker.DamageIncrease - target.DamageReduction);
             damageModifier = Mathf.Max(0.1f, damageModifier); 
 
             var calculatedDamage = baseDamage * damageReduction * blockMultiplier * criticalMultiplier * damageModifier;
             result.finalDamage = Mathf.Max(1f, calculatedDamage);
             result.lifeStealAmount = result.finalDamage * attacker.LiftSteal;
             result.reflectedDamage = result.finalDamage * target.ReflectDamage;
 
             return result;
         }
         
         private List<UnitState> GetTargetsInSector(UnitState attacker, float range, float sectorAngle)
         {
             var targets = new List<UnitState>();
             
             var mainTarget = GetTarget(attacker.targetId);
             if (mainTarget == null) return targets;

             // 공격 방향 벡터 = (메인 타겟 위치 - 공격자 위치).normalized
             var attackDirection = GetDirection(attacker.position, mainTarget.position);

             // 1. 우선 사거리(Radius) 안에 있는 적들만 먼저 후보로 뽑습니다.
             var candidates = GetTargetsInRange(attacker, range);
             
             foreach (var target in candidates)
             {
                 // 공격자에서 이 타겟으로의 방향
                 var dirToTarget = GetDirection(attacker.position, target.position);

                 // 3. 내적(Dot Product)을 이용한 각도 계산 (Vector3.Angle의 Pure C# 구현)
                 // 두 벡터의 내적값은 두 방향 사이의 코사인(cos) 값과 같습니다.
                 var dot = Vector3.Dot(attackDirection, dirToTarget);
        
                 // 내적값을 이용해 역코사인(Acos)을 구하면 라디안 각도가 나오고, 이를 도(Degree)로 변환합니다.
                 var angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

                 // 4. 부채꼴 판정
                 if (angle <= sectorAngle * 0.5f)
                 {
                     targets.Add(target);
                 }
             }

             return targets;
         }
         
         private List<UnitState> GetTargetsInRange(UnitState attacker, float range)
         {
             var targets = new List<UnitState>();

             // 전체 유닛 리스트를 순회 (Manager 등에서 관리하는 리스트)
             foreach (var unit in _unitList)
             {
                 // 1. 자기 자신 제외
                 if (unit.id == attacker.id) continue;

                 // 2. 살아있는 상태인지 확인
                 if (unit.currentActionState == UnitActionState.Die) continue;

                 // 3. 적군인지 확인 (팀이 다른 경우)
                 if (unit.teamId == attacker.teamId) continue;

                 // 4. 거리 계산 (성능을 위해 제곱 거리를 사용하기도 함)
                 // 사거리 안에 들어왔다면 리스트에 추가
                 if (Vector3.Distance(attacker.position, unit.position) <= range)
                 {
                     targets.Add(unit);
                 }
             }

             return targets;
         }
         
         // 방향 벡터를 구하는 유틸리티 함수 (Pure C#)
         private Vector3 GetDirection(Vector3 from, Vector3 to)
         {
             var dir = to - from;
             dir.y = 0; // 2D 평면 판정을 위해 높이값 제거
             return dir.normalized;
         }
    }
}