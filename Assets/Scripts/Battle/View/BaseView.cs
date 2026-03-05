using DG.Tweening;
using UnityEngine;

namespace Battle.View
{
    public abstract class BaseView : MonoBehaviour
    {
        private int _id;
        private MeshRenderer _meshRenderer;
        
        private Tween _flashTween;
        private Color _defaultColor;

        public int Id => _id;
        
        protected void Initialize(int id)
        {
            _id = id;
            _meshRenderer = GetComponent<MeshRenderer>();
            _defaultColor = _meshRenderer.material.color;
        }
        
        protected void FlashView()
        {
            if (_flashTween != null && _flashTween.IsActive())
            {
                _flashTween.Kill();
                _meshRenderer.material.color = _defaultColor;
            }

            _flashTween = _meshRenderer.material.DOColor(Color.red, 0.1f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InSine);
        }
    }
}