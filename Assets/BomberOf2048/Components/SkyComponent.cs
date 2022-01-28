using System;
using System.Collections.Generic;
using UnityEngine;

namespace BomberOf2048.Components
{
    public class SkyComponent : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _maxY;
        [SerializeField] private float _minY;

        private Transform[] _childs;

        private void Start()
        {
            var childsAndParent = new List<Transform>(GetComponentsInChildren<Transform>());
            if (childsAndParent == null) throw new ArgumentNullException(nameof(childsAndParent));
            childsAndParent.Remove(transform);
            
            _childs = childsAndParent.ToArray();
        }

        private void Update()
        {
            foreach (var child in _childs)
            {
                var pos = child.position;
                pos.y += _speed * Time.deltaTime;
                child.position = pos;


                if (!(child.position.y >= _maxY)) continue;
                
                var newPos = child.position;
                newPos.y = _minY;
                child.position = newPos;
            }
        }
    }
}