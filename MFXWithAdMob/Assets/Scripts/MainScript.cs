using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using GoogleMobileAds;
using GoogleMobileAds.Api;
 
public class MainScript : MonoBehaviour
{
	private string MoPubBannerInventoryHash             = "4ad212b1d0104c5998b288e7a8e35967";	// Mobfox test banner
	private string MoPubInterstitialInventoryHash       = "3fd85a3e7a9d43ea993360a2536b7bbd";
	private string MoPubRewardedInventoryHash           = "005491feb31848a0ae7b9daf4a46c701";

	//private string MoPubBannerInventoryHash             = "b195f8dd8ded45fe847ad89ed1d016da";	// MoPub test banner
	//private string MoPubInterstitialInventoryHash       = "24534e1901884e398f1253216226017e";	// MoPub test Interstitial
	//private string MoPubRewardedInventoryHash           = "920b6145fb1546cf8b5cf2ac34638bb7";	// MoPub test Rewarded

	public Text   txtTitle;

	public Button btnBanner;
	public Button btnInterstitial;
	public Button btnRewarded;
	
    // Start is called before the first frame update
    void Start()
    {
    	btnBanner.enabled = false;
    	btnInterstitial.enabled = false;
    	btnRewarded.enabled = false;
    	
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { 
        		//OnSdkInitializedEvent();
        	});

		/*
        MoPubManager.OnAdLoadedEvent       += OnAdLoadedEvent;
        MoPubManager.OnAdFailedEvent       += OnAdFailedEvent;
        MoPubManager.OnAdClickedEvent      += OnAdClickedEvent;
        MoPubManager.OnAdExpandedEvent     += OnAdExpandedEvent;
        MoPubManager.OnAdCollapsedEvent    += OnAdCollapsedEvent;

        MoPubManager.OnInterstitialLoadedEvent    += OnInterstitialLoadedEvent;
        MoPubManager.OnInterstitialFailedEvent    += OnInterstitialFailedEvent;
        MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;
        MoPubManager.OnInterstitialExpiredEvent   += OnInterstitialExpiredEvent;
        MoPubManager.OnInterstitialShownEvent     += OnInterstitialShownEvent;
        MoPubManager.OnInterstitialClickedEvent   += OnInterstitialClickedEvent;
        
        MoPubManager.OnRewardedVideoLoadedEvent             += OnRewardedVideoLoadedEvent;
        MoPubManager.OnRewardedVideoFailedEvent             += OnRewardedVideoFailedEvent;
        MoPubManager.OnRewardedVideoExpiredEvent            += OnRewardedVideoExpiredEvent;
        MoPubManager.OnRewardedVideoShownEvent              += OnRewardedVideoShownEvent;
        MoPubManager.OnRewardedVideoClickedEvent            += OnRewardedVideoClickedEvent;
        MoPubManager.OnRewardedVideoFailedToPlayEvent       += OnRewardedVideoFailedToPlayEvent;
        MoPubManager.OnRewardedVideoReceivedRewardEvent     += OnRewardedVideoReceivedRewardEvent;
        MoPubManager.OnRewardedVideoClosedEvent             += OnRewardedVideoClosedEvent;
        MoPubManager.OnRewardedVideoLeavingApplicationEvent += OnRewardedVideoLeavingApplicationEvent;

        MoPub.SdkConfiguration mopConfig = new MoPub.SdkConfiguration ();
        mopConfig.LogLevel = MoPub.LogLevel.Debug;
		mopConfig.AdUnitId = MoPubBannerInventoryHash;
		
        // Specify the mediated networks you are using here:
		mopConfig.MediatedNetworks = new MoPub.MediatedNetwork[]
            {
                // Example using a custom network adapter:
                new MoPub.MediatedNetwork
                {
                    // Specify the class name that implements the AdapterConfiguration interface.
                #if UNITY_ANDROID
                    AdapterConfigurationClassName = "com.mopub.mobileads.MobFoxAdapterConfiguration",  // include the full package name.
                #else // UNITY_IOS
                    AdapterConfigurationClassName = "MobFoxAdapterConfiguration",
                #endif

                    // Specify the class name that implements the MediationSettings interface.
                    // Note: Custom network mediation settings are currently not supported on Android.
                #if UNITY_IOS
                    MediationSettingsClassName = "MobFoxMoPubNativeAdRendererSettings",
                #endif

                    // Fill in settings and configuration options the same way as for supported networks:
                    
                    NetworkConfiguration = new Dictionary<string,string> {
                        { "key1", value },
                        { "key2", value },
                    },

                #if UNITY_IOS  // See note above.
                    MediationSettings    = new Dictionary<string,object> {
                        { "key1", value },
                        { "key2", value },
                    },
                #endif

                    MoPubRequestOptions  = new Dictionary<string,string> {
                        { "key1", value },
                        { "key2", value },
                    },
                }
            };

		MoPub.InitializeSdk (mopConfig);
		*/

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

		/*
		string[] _bannerAdUnits = new string[] {MoPubBannerInventoryHash};
	 	MoPub.LoadBannerPluginsForAdUnits(_bannerAdUnits);

		string[] _interAdUnits = new string[] {MoPubInterstitialInventoryHash};
	 	MoPub.LoadInterstitialPluginsForAdUnits(_interAdUnits);

		string[] _rewardedAdUnits = new string[] {MoPubRewardedInventoryHash};
	 	MoPub.LoadRewardedVideoPluginsForAdUnits(_rewardedAdUnits);
	 	*/

		txtTitle.text = "Initialized";
    }

    //============================================================
    
    public void hideBanner()
    {
  		//@@@MoPub.ShowBanner (MoPubBannerInventoryHash, false);   // hides the banner
  	}
    
    public void showBanner()
    {
  		//@@@MoPub.ShowBanner (MoPubBannerInventoryHash, true);   // shows the banner
    }
    
    public void clearAllAds()
    {
    	hideBanner();
    			
		// release all ads
		//@@@MoPub.DestroyBanner(MoPubBannerInventoryHash);
    }
    
    //============================================================
    
    public void btnBannerPressed()
    {
	    clearAllAds();

		txtTitle.text = "Loading banner...";

    	//@@@MoPub.RequestBanner(MoPubBannerInventoryHash, MoPub.AdPosition.Centered, MoPub.MaxAdSize.Width320Height50);
    }
    
    //------------------------------------------------------------
        
    void OnAdLoadedEvent(string adUnitId, float height)
    {
		txtTitle.text = "Banner loaded";

    	showBanner();
    }
    
    void OnAdFailedEvent(string adUnitId, string errMsg)
    {
		txtTitle.text = "Banner error: "+errMsg;

    	showBanner();
    }

    void OnAdClickedEvent(string adUnitId)
    {
		txtTitle.text = "Banner clicked";
    }

    void OnAdExpandedEvent(string adUnitId)
    {
		txtTitle.text = "Banner expanded";
    }

    void OnAdCollapsedEvent(string adUnitId)
    {
		txtTitle.text = "Banner collapsed";
    }

    //=============================================================
    
    public void btnInterPressed()
    {
	    clearAllAds();
    
		txtTitle.text = "Loading interstitial...";

	     //@@@MoPub.RequestInterstitialAd(MoPubInterstitialInventoryHash);
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

		//@@@MoPub.RequestRewardedVideo(MoPubRewardedInventoryHash);
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
