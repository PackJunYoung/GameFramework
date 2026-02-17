using DG.Tweening;
using Pool;
using TMPro;
using UnityEngine;

namespace Battle.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private float _jumpHeight = 50f; // 위로 튀어오르는 높이

        private Sequence _sequence;

        public void Show(float damage, Vector3 worldPosition)
        {
            // 1. 초기화 (이전 트윈이 돌고 있다면 강제 종료)
            _sequence?.Kill();
        
            _text.text = damage.ToString("F0");
            _text.alpha = 1f;
        
            // 2. 월드 좌표 -> 스크린 좌표 변환
            var screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            transform.position = screenPos;
            transform.localScale = Vector3.one * 0.5f; // 작게 시작

            // 3. 연출 시퀀스 생성
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOScale(1.2f, 0.1f).SetEase(Ease.OutBack)) // 팝업 효과
                .Join(transform.DOMoveY(screenPos.y + _jumpHeight, _duration * 0.5f).SetEase(Ease.OutQuad)) // 점프
                .AppendInterval(_duration * 0.2f) // 잠깐 유지
                .Append(transform.DOMoveY(screenPos.y + _jumpHeight + 20f, _duration * 0.3f)) // 살짝 더 상승
                .Join(_text.DOFade(0, _duration * 0.3f)) // 페이드 아웃
                .OnComplete(() =>
                {
                    gameObject.Release();
                });
        }

        private void OnDestroy()
        {
            // 오브젝트가 파괴될 때 트윈도 함께 제거 (메모리 안전)
            _sequence?.Kill();
        }
    }
}