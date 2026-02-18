using DG.Tweening;
using UnityEngine;

namespace Battle.View
{
    public abstract class BaseView : MonoBehaviour
    {
        private int _id;
        private MeshRenderer _meshRenderer;
        
        private Tween _flashTween;

        public int Id => _id;
        
        protected void Initialize(int id)
        {
            _id = id;
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        
        protected void FlashView()
        {
            if (_flashTween != null && _flashTween.IsActive())
            {
                _flashTween.Kill();
                _meshRenderer.material.color = Color.white;
            }

            _flashTween = _meshRenderer.material.DOColor(Color.red, 0.1f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InSine);
        }
    }
}