using System;
using System.Collections;
using BomberOf2048.Components;
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

            ScoreController = new ScoreController(_levelUpParticles);
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


        // private void OnDrawGizmos()
        // {
        //     var prevColor = Gizmos.color;
        //     Gizmos.color = Color.green;
        //
        //
        //
        //     var sectionSize = 1.13020833333f;//AnimationController.SectionScale;
        //     var halfSectionSize = sectionSize / 2;
        //
        //     
        //     var columnPos = AnimationController.SectionsPos[0, 0];
        //     var startX = columnPos.x - halfSectionSize;
        //     var endX = columnPos.x + halfSectionSize;
        //     
        //     
        //     var linePos = AnimationController.SectionsPos[0, 0];
        //     var startY = linePos.y - halfSectionSize;
        //     var endY = linePos.y + halfSectionSize;
        //     
        //    Gizmos.DrawLine(new Vector3(startX, startY, 0f), new Vector3(endX, endY, 0f));
        //    Gizmos.DrawLine(new Vector3(startX, startY, 0f), new Vector3(endX - sectionSize, endY, 0f));
        //    Gizmos.DrawLine(new Vector3(startX, startY, 0f), new Vector3(endX, endY - sectionSize, 0f));
        //    Gizmos.DrawLine(new Vector3(startX + sectionSize, startY, 0f), new Vector3(endX, endY, 0f));
        //    Gizmos.DrawLine(new Vector3(startX, startY + sectionSize, 0f), new Vector3(endX, endY, 0f));
        //    
        //    Gizmos.color = prevColor;
        //
        // }

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