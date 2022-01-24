using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace BomberOf2048.Managers
{
    public class AdMobManager : MonoBehaviour
    {
        private InterstitialAd _interstitialAd;
        private string _interstitialUnitId = "ca-app-pub-3940256099942544/1033173712";
        private void Awake()
        {
            MobileAds.Initialize(initStatus => {});
        }

        private void Start()
        {
            _interstitialAd = new InterstitialAd(_interstitialUnitId);
            var adRequest = new AdRequest.Builder().Build();
            _interstitialAd.LoadAd(adRequest);
        }

        public void ShowAd()
        {
            if(_interstitialAd.IsLoaded())
                _interstitialAd.Show();
        }
    }
}