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
	/*
    #if UNITY_ANDROID
        string AdMobBannerHash       = "ca-app-pub-3940256099942544/6300978111";
		string AdMobInterstitialHash = "ca-app-pub-3940256099942544/1033173712";
		string AdMobRewardedHash     = "ca-app-pub-3940256099942544/5224354917";
    #else
        string AdMobBannerHash       = "ca-app-pub-3940256099942544/2934735716";
		string AdMobInterstitialHash = "ca-app-pub-3940256099942544/4411468910";
		string AdMobRewardedHash     = "ca-app-pub-3940256099942544/1712485313";
    #endif
    */

    #if UNITY_ANDROID
        string AdMobBannerHash       = "ca-app-pub-8111915318550857/5234422920";
		string AdMobInterstitialHash = "ca-app-pub-8111915318550857/9385420926";
		string AdMobRewardedHash     = "ca-app-pub-6224828323195096/1152622735";
    #else
        //string AdMobBannerHash       = "ca-app-pub-6224828323195096/5240875564";
		//string AdMobInterstitialHash = "ca-app-pub-6224828323195096/7876284361";
		//string AdMobRewardedHash     = "ca-app-pub-6224828323195096/9409251358";
		
        string AdMobBannerHash       = "ca-app-pub-6224828323195096/7846687276";
		string AdMobInterstitialHash = "ca-app-pub-6224828323195096/7876284361";
		string AdMobRewardedHash     = "ca-app-pub-6224828323195096/9409251358";
    #endif

	private string txtToShow = "";
	private string txtReward = null;
	private string asyncCommand = null;

	public  Text   txtTitle;
	public  Button btnBanner;
	public  Button btnInterstitial;
	public  Button btnRewarded;
		
	private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-6224828323195096~8368193162";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-6224828323195096~3764142368";
            //string appId = "ca-app-pub-3940256099942544~1458002511";
        #else
            string appId = "unexpected_platform";
        #endif

		asyncCommand = null;

    	btnBanner.enabled = false;
    	btnInterstitial.enabled = false;
    	btnRewarded.enabled = false;    	

        clearAllAds();
        
        MobileAds.SetiOSAppPauseOnBackground(true);
    	
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
    	
    	OnSdkInitializedEvent();
    }

    // Update is called once per frame
    void Update()
    {
    	if (asyncCommand!=null)
    	{
    		if (asyncCommand == "showInterstitial")
    		{	
 				if ((interstitial!=null) && (interstitial.IsLoaded()))
 				{
 				    txtTitle.text = "Interstitial loaded, showing...";

    				interstitial.Show();
  				}
    				
    			asyncCommand = null;
    			return;
    		}
    		
    		if (asyncCommand == "showRewarded")
    		{	
 				if ((rewardedAd!=null) && (rewardedAd.IsLoaded()))
 				{
 				    txtTitle.text = "Rewarded loaded, showing...";

    				rewardedAd.Show();
  				}
    				
    			asyncCommand = null;
    			return;
    		}

    		asyncCommand = null;
    	}

    	if (txtReward!=null)
    	{
	        txtTitle.text = txtToShow + " | " + txtReward;
    	} else {
    	    txtTitle.text = txtToShow;
    	}    	
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

		txtToShow = "Initialized";
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
    	txtReward = null;
    
    	hideBanner();
    	
    	if (bannerView!=null)
    	{
    		bannerView.Destroy();
    		bannerView = null;
    	}
    	
    	if (interstitial!=null)
    	{
    		interstitial.Destroy();
    		interstitial = null;
    	}
    }
    
    //============================================================

	// Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            //.AddKeyword("game")
            //.SetGender(Gender.Male)
            //.SetBirthday(new DateTime(1985, 1, 1))
            //.TagForChildDirectedTreatment(false)
            .Build();
    }
    
    //============================================================
    
    public void btnBannerPressed()
    {
	    clearAllAds();

		txtToShow = "Loading banner...";

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

        // Load the banner with the request.
        bannerView.LoadAd(CreateAdRequest());
    }
    
    //------------------------------------------------------------

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");

    	txtToShow = "Banner loaded";

		showBanner();    	
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
		txtToShow = "Banner error: "+args.Message;

    	showBanner();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
		txtToShow = "Banner clicked";
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
		txtToShow = "Banner closed";
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
		txtToShow = "Banner leaving app";
    }

    //=============================================================
    
    public void btnInterPressed()
    {
	    clearAllAds();
    
		txtToShow = "Loading interstitial...";

		// Initialize an InterstitialAd.
    	interstitial = new InterstitialAd(AdMobInterstitialHash);
    
        // Called when an ad request has successfully loaded.
    	interstitial.OnAdLoaded += OnInterstitialLoadedEvent;
    	// Called when an ad request failed to load.
    	interstitial.OnAdFailedToLoad += OnInterstitialFailedEvent;
    	// Called when an ad is shown.
    	interstitial.OnAdOpening += OnInterstitialOpened;
    	// Called when the ad is closed.
    	interstitial.OnAdClosed += OnInterstitialClosed;
    	// Called when the ad click caused the user to leave the application.
    	interstitial.OnAdLeavingApplication += OnInterstitialLeavingApplication;
  
    	// Load the interstitial with the request.
    	interstitial.LoadAd(CreateAdRequest());
    }
    
    //-------------------------------------------------------------
    
    void OnInterstitialLoadedEvent(object sender, EventArgs args)
    {
		txtToShow = "Interstitial loaded";
 		
 		asyncCommand = "showInterstitial";
    }

	void OnInterstitialFailedEvent (object sender, AdFailedToLoadEventArgs args)
    {
		txtToShow = "Interstitial error: "+args.Message;
    }

	public void OnInterstitialOpened(object sender, EventArgs args)
	{
    	txtToShow = "Interstitial opened";
	}

	public void OnInterstitialClosed(object sender, EventArgs args)
	{
    	txtToShow = "Interstitial closed";
	}

	public void OnInterstitialLeavingApplication(object sender, EventArgs args)
	{
    	txtToShow = "Interstitial leaving app";
	}

    //=============================================================
    
    public void btnRewardedPressed()
    {
	    clearAllAds();
    
		txtToShow = "Loading rewarded...";
		
		rewardedAd = new RewardedAd(AdMobRewardedHash);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += OnRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += OnRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += OnRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += OnRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += OnUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += OnRewardedAdClosed;

		rewardedAd.LoadAd(CreateAdRequest());
    }
    
    //-------------------------------------------------------------
    
    public void OnRewardedAdLoaded(object sender, EventArgs args)
    {
        txtToShow = "Rewarded loaded";
 		
 		asyncCommand = "showRewarded";
    }

    public void OnRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        txtToShow = "Rewarded failed: " + args.Message;
    }

    public void OnRewardedAdOpening(object sender, EventArgs args)
    {
        txtToShow = "Rewarded opening";
    }

    public void OnRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        txtToShow = "Rewarded failed to show: " + args.Message;
    }

    public void OnRewardedAdClosed(object sender, EventArgs args)
    {
        txtToShow = "Rewarded closed";
    }

    public void OnUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        txtReward = "Got " + amount.ToString() + " " + type;
    }
}
