using BomberOf2048.Model.Definitions;
using UnityEngine;

namespace BomberOf2048.Components.Sections
{
    public class SectionComponent : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private GameObject _effect;
        [SerializeField] private float _scale = 1f;

        public int[] CoordinatesSection { get; private set; } = {-1, -1};

        public void SetView(SectionDef sectionDef)
        {
            _sprite.sprite = sectionDef.Sprite;
            _effect.SetActive(sectionDef.IsEffectEnable);
        }
        public void SetScale(float scale)
        {
            _sprite.transform.localScale = new Vector3(scale, scale, 1f);
            _effect.transform.localScale = new Vector3(scale, scale, 1f);

            _scale = scale;
        }

        public void SetCoordinates(int x, int y)
        {
            CoordinatesSection[0] = x;
            CoordinatesSection[1] = y;
        }
        
        [ContextMenu("Set Size")]
        private void SetScale()
        {
            _sprite.transform.localScale = new Vector3(_scale, _scale, 1f);
            _effect.transform.localScale = new Vector3(_scale, _scale, 1f);
        }
    }
}