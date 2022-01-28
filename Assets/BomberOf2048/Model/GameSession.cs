using BomberOf2048.Components;
using BomberOf2048.Controllers;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using BomberOf2048.Utils.Disposables;
using UnityEngine;

namespace BomberOf2048.Model
{
    public class GameSession : Singleton<GameSession>
    {
        [SerializeField] private GameData _data;
        
        [Header("Score controller parameters")]
        [SerializeField] private float _firstLevelScore;
        [SerializeField] private float _nextLevelModifier;

        [Header("Prefabs and containers")]
        [SerializeField] private GameObject _sectionPrefab;
        [SerializeField] private GameObject _fieldContainer;
        [SerializeField] private GameObject _prefabSectionForAnim;

        [Header("Particles")]
        [SerializeField] private GameObject _boomParticles;

        [Header("LevelUp Particles")]
        [SerializeField] private LevelUpParticlesComponent _levelUpParticles;
        
        public GameData Data => _data;
        public MainController MainController { get; private set; }
        public FieldViewController FieldViewController { get; private set; }
        public AnimationController AnimationController { get; private set; }
        public ScoreController ScoreController { get; private set; }
        public BoomController BoomController { get; private set; }
        

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            Initialization();
            
            MainController = new MainController();
            _trash.Retain(MainController);
            
            FieldViewController = new FieldViewController(_sectionPrefab, _fieldContainer);
            _trash.Retain(FieldViewController);
            
            AnimationController = new AnimationController(_prefabSectionForAnim);
            _trash.Retain(AnimationController);

            ScoreController = new ScoreController(_firstLevelScore, _nextLevelModifier, _levelUpParticles);
            _trash.Retain(ScoreController);
            ScoreController.AddScore(0);

            BoomController = new BoomController(_boomParticles);

            MainController.SpawnStartSections();
        }
        
        private void Start()
        {
            FieldViewController.UpdateAllField();

            if (!_data.IsPlayerSeeTutorial.Value)
            {
                WindowUtils.CreateWindow("UI/InfoWindow");
                _data.IsPlayerSeeTutorial.Value = true;
            }
        }

        private void Initialization()
        {
            _data.Initialize();
        }
        
        private void OnDestroy()
        {
            _trash.Dispose();
        }
        
#if UNITY_EDITOR
        [ContextMenu("Screenshot")]
        private void Screenshot()
        {
            var currentDate = System.DateTime.Now;
            ScreenCapture.CaptureScreenshot($"BomberOf2048 [{currentDate.Hour}.{currentDate.Minute}.{currentDate.Second}] [{currentDate.Day}-{currentDate.Month}-{currentDate.Year}].png");
        }
#endif
        
    }
}