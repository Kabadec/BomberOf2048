using BomberOf2048.Model.Definitions;
using UnityEngine;

namespace BomberOf2048.Components.Sections
{
    public class SectionComponent : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private GameObject _effect;
        [SerializeField] private float _size = 2f;

        public int[] CoordinatesSection { get; private set; } = {-1, -1};

        public void SetView(SectionDef sectionDef)
        {
            _sprite.sprite = sectionDef.Sprite;
            _effect.SetActive(sectionDef.IsEffectEnable);
        }
        public void SetSize(float size)
        {
            _sprite.transform.localScale = new Vector3(size, size, 1f);
            _effect.transform.localScale = new Vector3(size, size, 1f);

            _size = size;
        }

        public void SetCoordinates(int x, int y)
        {
            CoordinatesSection[0] = x;
            CoordinatesSection[1] = y;
        }
        
        [ContextMenu("Set Size")]
        private void SetSize()
        {
            _sprite.transform.localScale = new Vector3(_size, _size, 1f);
            _effect.transform.localScale = new Vector3(_size, _size, 1f);
        }
    }
}