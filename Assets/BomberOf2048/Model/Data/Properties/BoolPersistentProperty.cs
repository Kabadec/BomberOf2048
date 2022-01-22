using UnityEngine;

namespace BomberOf2048.Model.Data.Properties
{
    public class BoolPersistentProperty : PrefsPersistentProperty<bool>
    {
        public BoolPersistentProperty(bool defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override void Write(bool value)
        {
            if(value)
                PlayerPrefs.SetInt(Key, 1);
            else
                PlayerPrefs.SetInt(Key, 0);
            
            PlayerPrefs.Save();
        }

        protected override bool Read(bool defaultValue)
        {
            var defValue = 0;
            if (defaultValue)
                defValue = 1;
            return PlayerPrefs.GetInt(Key, defValue) == 1;
        }
    }
}