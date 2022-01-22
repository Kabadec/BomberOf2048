using System;
using BomberOf2048.Components.Sections;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Model.Data.Properties;
using BomberOf2048.Model.Definitions;
using BomberOf2048.Utils;
using BomberOf2048.Widgets;
using UnityEngine;

namespace BomberOf2048.Controllers
{
    public class FieldViewController : MonoBehaviour
    {
        
        [Space]
        //[SerializeField] private SpriteRenderer _bgField;
        [SerializeField] private GameObject _sectionPrefab;
        [SerializeField] private GameObject _fieldContainer;

        //[Header("Sizes and Spaces")]
        //[SerializeField] private float _spaceBehindSections;
        //[SerializeField] private float _leftRightUpDownSpace;
        
        
        public float SectionSize { get; private set; }
        public Vector3[,] SectionsPos { get; private set; }
        

        private GameData GameData => Singleton<GameSession>.Instance.Data;
        
        private int FieldSize => GameData.FieldSize;
        
        private SectionComponent[] _sectionWidgets;

        private void Start()
        {
            SectionSize = DefsFacade.I.Fields.GetSectionScale(FieldSize);
            SectionsPos = new Vector3[FieldSize, FieldSize];
            
            for (var i = 0; i < FieldSize; i++)
            {
                for (var j = 0; j < FieldSize; j++)
                {
                    SectionsPos[i, j] = DefsFacade.I.Fields.GetSectionPos(FieldSize, i, j);
                }
            }
            
            BuildField();
            UpdateAllField();
        }
        
        
        [ContextMenu("Update All Field")]
        public void UpdateAllField()
        {
            for (var i = 0; i < FieldSize; i++)
            {
                for (var j = 0; j < FieldSize; j++)
                {
                    UpdateSection(i, j);
                    UpdateSectionCoordinates(i, j);
                }
            }
        }
        
        public void UpdateSection(int x, int y)
        {
            var value = GameData.GameField[x, y].Value;
            var section = GetSection(x, y);
            var def = DefsFacade.I.Sections.Get(value);
            section.SetView(def);
        }

        public void UpdateSectionCoordinates(int x, int y)
        {
            var section = GetSection(x, y);
            section.SetCoordinates(x, y);
        }

        private void BuildField()
        {
            var childs = _fieldContainer.GetComponentsInChildren<SectionComponent>();
            if (childs.Length < FieldSize * FieldSize)
            {
                var numChildForInstantiate = FieldSize * FieldSize - childs.Length;
                for (var i = 0; i < numChildForInstantiate; i++)
                {
                    Instantiate(_sectionPrefab, Vector3.zero, Quaternion.identity, _fieldContainer.transform);
                }
            }
            else if (childs.Length > FieldSize * FieldSize)
            {
                for (var i = FieldSize * FieldSize; i < childs.Length; i++)
                {
                    childs[i].gameObject.SetActive(false);
                }
            }
            
            _sectionWidgets = _fieldContainer.GetComponentsInChildren<SectionComponent>();
            
            for (var i = 0; i < FieldSize; i++)
            {
                for (var j = 0; j < FieldSize; j++)
                {
                    var section = GetSection(j, i);
                    section.SetSize(SectionSize);
                    section.transform.position = SectionsPos[j, i];
                }
            }
        }

        private SectionComponent GetSection(int x, int y)
        {
            var number = x + y * FieldSize;
            return _sectionWidgets[number];
        }

        // private float GetSectionSize()
        // {
        //     var bgSize = _bgField.size.x - _leftRightUpDownSpace * 2;
        //     var sectionSize = (bgSize - (FieldSize - 1) * _spaceBehindSections) / FieldSize;
        //     return sectionSize;
        // }
        //
        // private Vector3 GetSectionPos(int x, int y)
        // {
        //     var bgSize = _bgField.size.x - _leftRightUpDownSpace * 2;
        //     var bgPos = _bgField.transform.position;
        //     var sectionSize = GetSectionSize();
        //     var pos = Vector3.zero;
        //
        //     pos.x = bgPos.x - bgSize / 2 + sectionSize / 2 + _spaceBehindSections * x + sectionSize * x;
        //     pos.y = bgPos.y - bgSize / 2 + sectionSize / 2 + _spaceBehindSections * y + sectionSize * y;
        //
        //     return pos;
        // }
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            //BuildField();
        }
#endif
        
    }
    [Serializable]
    public class SectionData
    {
        private int _value = 0;

        public int Value => _value;
    }
}