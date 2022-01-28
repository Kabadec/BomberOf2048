using System;
using UnityEngine;

namespace BomberOf2048.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/Sections", fileName = "Sections")]
    public class SectionsRepository : ScriptableObject
    {
        [SerializeField] protected SectionDef[] _collection;

        public SectionDef Get(int value)
        {
            return _collection[value];
        }
    }


    [Serializable]
    public struct SectionDef
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private bool _isEffectEnable;
        
        public Sprite Sprite => _sprite;
        public bool IsEffectEnable => _isEffectEnable;
    }
}