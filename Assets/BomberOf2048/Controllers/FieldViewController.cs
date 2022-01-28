using System;
using BomberOf2048.Components.Sections;
using BomberOf2048.Model;
using BomberOf2048.Model.Data;
using BomberOf2048.Model.Definitions;
using BomberOf2048.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BomberOf2048.Controllers
{
    public class FieldViewController : IDisposable
    {
        public float SectionScale { get; private set; }
        public Vector3[,] SectionsPos { get; private set; }
        
        
        private readonly GameObject _sectionPrefab;
        private readonly GameObject _fieldContainer;
        private SectionComponent[] _sectionWidgets;
        private GameData GameData => Singleton<GameSession>.Instance.Data;
        private int FieldSize => GameData.FieldSize;
        
        public FieldViewController(GameObject sectionPrefab, GameObject fieldContainer)
        {
            _sectionPrefab = sectionPrefab;
            _fieldContainer = fieldContainer;
            
            SectionScale = DefsFacade.I.Fields.GetSectionScale(FieldSize);
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
        
        private void BuildField()
        {
            var childs = _fieldContainer.GetComponentsInChildren<SectionComponent>();
            if (childs.Length < FieldSize * FieldSize)
            {
                var numChildForInstantiate = FieldSize * FieldSize - childs.Length;
                for (var i = 0; i < numChildForInstantiate; i++)
                {
                    Object.Instantiate(_sectionPrefab, Vector3.zero, Quaternion.identity, _fieldContainer.transform);
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
                    section.SetScale(SectionScale);
                    section.transform.position = SectionsPos[j, i];
                }
            }
        }

        private SectionComponent GetSection(int x, int y)
        {
            var number = x + y * FieldSize;
            return _sectionWidgets[number];
        }
        
        private void UpdateSectionCoordinates(int x, int y)
        {
            var section = GetSection(x, y);
            section.SetCoordinates(x, y);
        }
        
        public void Dispose()
        {
            
        }
    }
}