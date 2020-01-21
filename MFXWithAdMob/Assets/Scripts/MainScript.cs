using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using GoogleMobileAds;
using GoogleMobileAds.Api;
 
public class MainScript : MonoBehaviour
{
    #if UNITY_ANDROID
        string AdMobBannerHash = "ca-app-pub-3940256099942544/6300978111";
    #else
        string AdMobBannerHash = "ca-app-pub-3940256099942544/2934735716";
    #endif

	//private string AdMobBannerHash             = "4ad212b1d0104c5998b288e7a8e35967";	// Mobfox test banner
	private string AdMobInterstitialHash       = "3fd85a3e7a9d43ea993360a2536b7bbd";
	private string AdMobRewardedHash           = "005491feb31848a0ae7b9daf4a46c701";

	public Text   txtTitle;

	public Button btnBanner;
	public Button btnInterstitial;
	public Button btnRewarded;
		
	private BannerView bannerView;
	
    // Start is called before the first frame update
    void Start()
    {
    	btnBanner.enabled = false;
    	btnInterstitial.enabled = false;
    	btnRewarded.enabled = false;    	
    	
        #if UNITY_ANDROID
            string appId = "ca-app-pub-6224828323195096~8368193162";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
        #else
            string appId = "unexpected_platform";
        #endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
    	
    	OnSdkInitializedEvent();

        clearAllAds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    //============================================================
    
    // create your handler
    void OnSdkInitializedEvent()
    {
    	// The SDK is initialized here. Ready to make ad requests.
        btnBanner.enabled = true;
    	btnInterstitial.enabled = true;
    	btnRewarded.enabled = true;
    	txtTitle.enabled = true;

		txtTitle.text = "Initialized";
    }

    //============================================================
    
    public void hideBanner()
    {
    	if (bannerView!=null)
    	{
    		bannerView.Hide();
    	}
  	}
    
    public void showBanner()
    {
    	if (bannerView!=null)
    	{
    		bannerView.Show();
    	}
    }
    
    public void clearAllAds()
    {
    	hideBanner();
    	
    	if (bannerView!=null)
    	{
    		bannerView.Destroy();
    		bannerView = null;
    	}
    }
    
    //============================================================
    
    public void btnBannerPressed()
    {
	    clearAllAds();

		txtTitle.text = "Loading banner...";
		
		Debug.LogError ("dbg-1");

        bannerView = new BannerView(AdMobBannerHash, AdSize.Banner, AdPosition.Center);

        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
    
    //------------------------------------------------------------

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
//    		((BannerView)sender).Hide();

		bannerView.Hide();
    		
//    	txtTitle.text = "Banner loaded";
    	
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
		txtTitle.text = "Banner error: "+args.Message;

    	showBanner();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
		txtTitle.text = "Banner clicked";
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
		txtTitle.text = "Banner closed";
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
		txtTitle.text = "Banner leaving app";
    }

    //=============================================================
    
    public void btnInterPressed()
    {
	    clearAllAds();
    
		txtTitle.text = "Loading interstitial...";

	     //@@@MoPub.RequestInterstitialAd(AdMobInterstitialHash);
    }
    
    //-------------------------------------------------------------
    
    void OnInterstitialLoadedEvent (string adUnitId)
    {
		txtTitle.text = "Interstitial loaded";

 		//@@@MoPub.ShowInterstitialAd (adUnitId);
    }

	void OnInterstitialFailedEvent (string adUnitId, string errorCode)
    {
		txtTitle.text = "Interstitial error: "+errorCode;
    }

	void OnInterstitialDismissedEvent (string adUnitId)
    {
		txtTitle.text = "Interstitial dismissed";
    }

	void OnInterstitialExpiredEvent (string adUnitId)
    {
		txtTitle.text = "Interstitial expired";
    }

	void OnInterstitialShownEvent (string adUnitId)
    {
		txtTitle.text = "Interstitial shown";
    }

	void OnInterstitialClickedEvent (string adUnitId)
    {
		txtTitle.text = "Interstitial clicked";
    }

    //=============================================================
    
    public void btnRewardedPressed()
    {
	    clearAllAds();
    
		txtTitle.text = "Loading rewarded...";

		//@@@MoPub.RequestRewardedVideo(AdMobRewardedHash);
    }
    
    //-------------------------------------------------------------
    
	void OnRewardedVideoLoadedEvent (string adUnitId)
    {
		txtTitle.text = "Rewarded loaded";
		
		//@@@MoPub.ShowRewardedVideo(adUnitId);
    }

	void OnRewardedVideoFailedEvent (string adUnitId, string errorMsg)
    {
		txtTitle.text = "Rewarded failed: "+errorMsg;
    }

	void OnRewardedVideoExpiredEvent (string adUnitId)
    {
		txtTitle.text = "Rewarded expired";
    }

	void OnRewardedVideoShownEvent (string adUnitId)
    {
		txtTitle.text = "Rewarded shown";
    }

	void OnRewardedVideoClickedEvent (string adUnitId)
    {
		txtTitle.text = "Rewarded clicked";
    }

	void OnRewardedVideoFailedToPlayEvent (string adUnitId, string errorMsg)
    {
		txtTitle.text = "Rewarded failed to play: "+errorMsg;
    }

	void OnRewardedVideoReceivedRewardEvent (string adUnitId, string label, float amount)
    {
		txtTitle.text = "Rewarded received reward: "+amount+" "+label+"/s";
    }

	void OnRewardedVideoClosedEvent (string adUnitId)
    {
		txtTitle.text = "Rewarded closed";
    }

	void OnRewardedVideoLeavingApplicationEvent (string adUnitId)
    {
		txtTitle.text = "Rewarded leaving app";
    }
}
