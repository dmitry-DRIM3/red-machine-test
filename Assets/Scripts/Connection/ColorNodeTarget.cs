using System;
using Events;
using UnityEngine;

namespace Connection
{
    public class ColorNodeTarget : MonoBehaviour
    {
        [SerializeField] private Color targetColor;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ColorNode colorNode;
        
        public bool IsCompleted => targetColor == colorNode.Color;

        public event Action<ColorNodeTarget, bool> TargetCompletionChangeEvent;


        private void Awake()
        {
            colorNode.ColorChangedEvent += OnColorChanged;
        }

        private void OnDestroy()
        {
            colorNode.ColorChangedEvent -= OnColorChanged;
        }

        private void OnColorChanged(Color currentColor)
        {
            Debug.Log(AreColorsEqual(currentColor, targetColor));
            TargetCompletionChangeEvent?.Invoke(this, AreColorsEqual(currentColor, targetColor));
        }

        private void OnValidate()
        {
            spriteRenderer.color = targetColor;
        }

        private bool AreColorsEqual(Color c1, Color c2)
        {
            return Mathf.Round(c1.r * 255) == Mathf.Round(c2.r * 255) &&
                   Mathf.Round(c1.g * 255) == Mathf.Round(c2.g * 255) &&
                   Mathf.Round(c1.b * 255) == Mathf.Round(c2.b * 255) &&
                   Mathf.Round(c1.a * 255) == Mathf.Round(c2.a * 255);
        }
    }
}
