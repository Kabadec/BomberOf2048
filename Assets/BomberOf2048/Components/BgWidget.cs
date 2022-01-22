using System;
using UnityEngine;

namespace BomberOf2048.Widgets
{
    public class BgWidget : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            var aspectRatio = (float)Screen.height / Screen.width;
            var spriteSize = _spriteRenderer.size;
            
            var spriteSizeY = aspectRatio * spriteSize.x;
            var newSpriteSize = new Vector2(spriteSize.x, spriteSizeY);

            _spriteRenderer.size = newSpriteSize;
        }
    }
}