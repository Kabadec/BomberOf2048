using UnityEngine;

namespace BomberOf2048.Components
{
    public class BgComponent : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            var aspectRatio = (float)Screen.height / Screen.width;
            var spriteSize = _spriteRenderer.size;
            
            var spriteSizeY = aspectRatio * spriteSize.x;
            var newSpriteSize = new Vector2(spriteSize.x, (spriteSizeY + 2));

            _spriteRenderer.size = newSpriteSize;
        }
    }
}