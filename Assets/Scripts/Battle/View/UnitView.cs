using Battle.DamageText;
using Battle.Event;
using Battle.HUD;
using Pool;
using UnityEngine;

namespace Battle.View
{
    public class UnitView : BaseView
    {
        private Vector3 _logicPosition;

        // 초기화
        public void Initialize(UnitState state)
        {
            Initialize(state.id);
            
            _logicPosition = state.position;
            HUDManager.Instance.AttachHPBar(this, state.MaxHp);
        }
        
        // 목표 위치로 이동
        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _logicPosition, Time.deltaTime * 15f);
        }

        // 목표 위치를 변경
        public void PlayMove(Vector3 newPos)
        {
            _logicPosition = newPos;
        }

        // 피격
        public void PlayHit(HitEvent hit)
        {
            FlashView();
            DamageTextManager.Instance.Spawn(hit.damage, transform.position);
            HUDManager.Instance.UpdateHP(Id, hit.curHp, hit.maxHp);
        }
        
        // 사망
        public void PlayDie()
        {
            HUDManager.Instance.DetachHPBar(Id);
            gameObject.Release();
        }
    }
}
