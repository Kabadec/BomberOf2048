using System;
using UnityEngine;

namespace BomberOf2048.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/Field", fileName = "Field")]
    public class FieldRepository : ScriptableObject
    {
        [SerializeField] private FieldDef[] _collection;
        
        public Vector3 GetSectionPos(int fieldSize, int x, int y)
        {
            foreach (var fieldDef in _collection)
            {
                if (fieldDef.FieldSize == fieldSize)
                {
                    var startPos = fieldDef.StartPos;
                    var toNextPos = fieldDef.ToNextPos;
                    return new Vector3(startPos.x + x * toNextPos.x, startPos.y + y * toNextPos.y - 5f, 0f);
                }
            }
            Debug.LogError("Have not that field size");
            return Vector3.zero;
        }

        public float GetSectionScale(int fieldSize)
        {
            foreach (var fieldDef in _collection)
            {
                if (fieldDef.FieldSize == fieldSize)
                {
                    return fieldDef.SectionScale;
                }
            }
            Debug.LogError("Have not that field size");
            return -1f;
        }
    }


    [Serializable]
    public struct FieldDef
    {
        [SerializeField] private int _fieldSize;
        [SerializeField] private Vector2 _startPos;
        [SerializeField] private Vector2 _toNextPos;
        [SerializeField] private float _sectionScale;

        public int FieldSize => _fieldSize;
        public Vector2 StartPos => _startPos;

        public Vector2 ToNextPos => _toNextPos;

        public float SectionScale => _sectionScale;
    }
    
}