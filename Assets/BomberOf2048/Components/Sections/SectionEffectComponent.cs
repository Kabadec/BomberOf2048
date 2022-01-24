using System;
using BomberOf2048.Utils;
using UnityEngine;

namespace BomberOf2048.Components.Sections
{
    public class SectionEffectComponent : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;

        private Color LerpColor => Singleton<SectionEffectLerpColorComponent>.Instance.LerpColor;

        private void OnEnable()
        {
            _renderer.color = LerpColor;
        }

        private void Update()
        {
            _renderer.color = LerpColor;
        }
    }
}