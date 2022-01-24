using BomberOf2048.Controllers;
using BomberOf2048.Model.Data;
using BomberOf2048.Utils;
using BomberOf2048.Utils.Disposables;
using GoogleMobileAds.Api;
using UnityEngine;

namespace BomberOf2048.Model
{
    public class GameSession : Singleton<GameSession>
    {
        [SerializeField] private GameData _data;

        [SerializeField] private GameObject _sectionPrefab;
        [SerializeField] private GameObject _fieldContainer;
        [SerializeField] private GameObject _prefabSectionForAnim;

        [Header("Particles")]
        [SerializeField] private GameObject _boomParticles;



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

            ScoreController = new ScoreController();
            _trash.Retain(ScoreController);
            ScoreController.AddScore(0);

            BoomController = new BoomController(_boomParticles);

            MainController.Initialize();
        }

        private void Start()
        {
            FieldViewController.UpdateAllField();
        }

        private void Initialization()
        {
            _data.Initialize();
        }


        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}