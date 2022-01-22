namespace BomberOf2048.Model.Data.Properties
{
    public abstract class PrefsPersistentProperty<TPropertyType> : PersistentProperty<TPropertyType>
    {
        protected readonly string Key;

        protected PrefsPersistentProperty(TPropertyType defaultValue, string key) : base(defaultValue)
        {
            Key = key;
        }
    }
}