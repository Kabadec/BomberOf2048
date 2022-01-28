using System;
using System.Collections;
using BomberOf2048.Utils;
using GoogleMobileAds.Api;
using UnityEngine;

namespace BomberOf2048.Managers
{
    public class AdMobManager : Singleton<AdMobManager>
    {
        [SerializeField] private float _adDelay;
        [SerializeField] private string _interstitialUnitId;
        [SerializeField] private string _bannerUnitId;
        
        private InterstitialAd _interstitialAd;
        private BannerView _bannerView;
        
        private Coroutine _coroutine;
        
        private void Awake()
        {
            MobileAds.Initialize(initStatus => {});
        }

        private void Start()
        {
            _interstitialAd = new InterstitialAd(_interstitialUnitId);
            RequestAd();
            
            _interstitialAd.OnAdClosed += HandleOnAdClosed;

            _coroutine = StartCoroutine(AdCoroutine());
            
            _bannerView = new BannerView(_bannerUnitId, AdSize.Banner, AdPosition.Bottom);
            AdRequest request = new AdRequest.Builder().Build();
            _bannerView.LoadAd(request);
        }

        public void ShowAds()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            ShowAd();
            
            _coroutine = StartCoroutine(AdCoroutine());
        }
        
        private void RequestAd()
        {
            var adRequest = new AdRequest.Builder().Build();
            _interstitialAd.LoadAd(adRequest);
        }

        private void ShowAd()
        {
            if(_interstitialAd.IsLoaded())
                _interstitialAd.Show();
            else
                RequestAd();
        }

        private IEnumerator AdCoroutine()
        {
            yield return new WaitForSeconds(_adDelay);
            
            ShowAd();
            
            _coroutine = StartCoroutine(AdCoroutine());
        }
        
        private void HandleOnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleAdClosed event received");
            RequestAd();
        }
        
        private void OnDestroy()
        {
            _interstitialAd.OnAdClosed -= HandleOnAdClosed;
        }
    }
}