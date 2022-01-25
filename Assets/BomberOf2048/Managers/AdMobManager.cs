using System;
using System.Collections;
using BomberOf2048.Utils;
using GoogleMobileAds.Api;
using UnityEngine;

namespace BomberOf2048.Managers
{
    public class AdMobManager : Singleton<AdMobManager>
    {
        [SerializeField] private float _adDelay = 60;
        private InterstitialAd _interstitialAd;
        private BannerView _bannerView;
        private string _interstitialUnitId = "ca-app-pub-3940256099942544/1033173712";
        private string _bannerUnitId = "ca-app-pub-3940256099942544/6300978111";

        private Coroutine _coroutine;
        private void Awake()
        {
            MobileAds.Initialize(initStatus => {});
        }

        private void Start()
        {
            _interstitialAd = new InterstitialAd(_interstitialUnitId);
            var adRequest = new AdRequest.Builder().Build();
            _interstitialAd.LoadAd(adRequest);
            
            _interstitialAd.OnAdClosed += HandleOnAdClosed;

            _coroutine = StartCoroutine(AdCoroutine());
            
            
            
            _bannerView = new BannerView(_bannerUnitId, AdSize.Banner, AdPosition.Bottom);
            AdRequest request = new AdRequest.Builder().Build();
            _bannerView.LoadAd(request);
        }
        

        public void RequestAd()
        {
            var adRequest = new AdRequest.Builder().Build();
            _interstitialAd.LoadAd(adRequest);
        }

        public void ShowAds()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            ShowAd();
        }
        
        private void ShowAd()
        {
            if(_interstitialAd.IsLoaded())
                _interstitialAd.Show();
        }
        
        private void HandleOnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleAdClosed event received");
            RequestAd();
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(AdCoroutine());
        }

        private IEnumerator AdCoroutine()
        {
            yield return new WaitForSeconds(_adDelay);
            ShowAd();
        }

        private void OnDestroy()
        {
            _interstitialAd.OnAdClosed -= HandleOnAdClosed;
        }
    }
}