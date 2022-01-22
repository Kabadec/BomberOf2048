using System;
using System.Collections.Generic;

namespace BomberOf2048.Utils
{
    public class Lock
    {
        private readonly List<Object> _retained = new List<object>();

        public void Retain(Object item)
        {
            _retained.Add(item);
        }

        public void Release(object item)
        {
            _retained.Remove(item);
        }

        public bool IsLocked => _retained.Count > 0;
    }
}