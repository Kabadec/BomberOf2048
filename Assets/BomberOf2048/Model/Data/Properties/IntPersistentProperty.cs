using UnityEngine;

namespace BomberOf2048.Model.Data.Properties
{
    public class IntPersistentProperty : PrefsPersistentProperty<int>
    {
        public IntPersistentProperty(int defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override void Write(int value)
        {
            PlayerPrefs.SetInt(Key, value);
            PlayerPrefs.Save();
        }

        protected override int Read(int defaultValue)
        {
            return PlayerPrefs.GetInt(Key, defaultValue);
        }
    }
}