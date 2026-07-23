using LizardKit.DebugButton;
using UnityEngine;
using UnityEngine.UI;

namespace BrothelGame
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class PixelPerfectUI : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private RectTransform _parentRectTransform;
        private Image _image;
        private Vector2 _lastParentSize;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            _parentRectTransform = _rectTransform.parent as RectTransform;

            if (_parentRectTransform == null)
            {
                Debug.LogError("PixelPerfectUI requires a parent RectTransform to function!");
                enabled = false;
                return;
            }

            UpdatePixelPerfectSize();
        }

        private void Update()
        {
            if (!_parentRectTransform) return;
            if (_parentRectTransform.rect.size != _lastParentSize)
            {
                UpdatePixelPerfectSize();
            }
        }

        private void UpdatePixelPerfectSize()
        {
            if (!_image) return;
            if (!_image.sprite) return;

            var parentSize = _parentRectTransform.rect.size;
            _lastParentSize = parentSize;

            // Get original sprite size in pixels
            var spriteSize = _image.sprite.rect.size;

            // Calculate max integer scale that fits within the parent
            var scaleX = parentSize.x / spriteSize.x;
            var scaleY = parentSize.y / spriteSize.y;
            var pixelScale = Mathf.Min(scaleX, scaleY); // Maintain aspect ratio

            // Round down to the nearest integer to avoid fractional pixels
            var roundedScale = Mathf.FloorToInt(pixelScale);
            if (roundedScale < 1) roundedScale = 1; // Ensure at least 1x scale

            // Calculate final size using pixel-perfect scaling
            var finalSize = spriteSize * roundedScale;

            // Ensure it doesn't exceed parent bounds
            finalSize.x = Mathf.Min(finalSize.x, parentSize.x);
            finalSize.y = Mathf.Min(finalSize.y, parentSize.y);

            // Apply the final size
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, finalSize.x);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalSize.y);

            // Ensure it stays centered within the parent
            _rectTransform.anchoredPosition = Vector2.zero;
        }

        [Button]
        public void Recalc()
        {
            UpdatePixelPerfectSize();
        }
    }
}
