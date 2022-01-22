using System;
using UnityEngine;
using UnityEngine.UI;
//using AdColony;

namespace BomberOf2048.Managers
{
    public class AdcolonyManager : MonoBehaviour
    {
        [SerializeField] private Text _message;
        [SerializeField] private string _appID;
        [SerializeField] private string[] _zoneIds;

        //private InterstitialAd _ad = null;


        private void Start()
        {
            //ConfigureAdcolony();
            //RequestVideo();
        }

        // private void ConfigureAdcolony()
        // {
        //     Ads.Configure(_appID, null, _zoneIds);
        //     Ads.OnRequestInterstitial += (ad =>
        //     {
        //         _ad = ad;
        //         _message.text += "OnRequestInterstitial ";
        //     });
        //
        //     Ads.OnExpiring += (ad =>
        //     {
        //         _message.text += "OnExpiring ";
        //         Ads.RequestInterstitialAd(_zoneIds[0], null);
        //     });
        // }
        //
        // public void RequestVideo()
        // {
        //     AdOptions adOptions = new AdOptions();
        //     adOptions.ShowPrePopup = true;
        //     adOptions.ShowPostPopup = true;
        //     Ads.RequestInterstitialAd(_zoneIds[0], adOptions);
        // }
        //
        // public void Show()
        // {
        //     if (_ad != null)
        //     {
        //         Ads.ShowAd(_ad);
        //     }
        //     else
        //     {
        //         _message.text = "null ad";
        //     }
        // }
    }
}