using UnityEngine;

namespace BomberOf2048.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private SectionsRepository _sections;
        [SerializeField] private FieldRepository _fields;

        public SectionsRepository Sections => _sections;
        public FieldRepository Fields => _fields;
        

        private static DefsFacade _instance;

        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}