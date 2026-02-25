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
    }
}