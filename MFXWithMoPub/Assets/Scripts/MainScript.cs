using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainScript : MonoBehaviour
{
	private string MoPubBannerInventoryHash             = "4ad212b1d0104c5998b288e7a8e35967";	// Mobfox test banner
	private string MoPubInterstitialInventoryHash       = "3fd85a3e7a9d43ea993360a2536b7bbd";
	private string MoPubRewardedInventoryHash           = "005491feb31848a0ae7b9daf4a46c701";
	private string MoPubNativeInventoryHash             = "b146b367940a4c6da94e8143fb4b66e4";

	public Text   txtTitle;

	public Button btnBanner;
	public Button btnInterstitial;
	public Button btnRewarded;
	public Button btnNative;
	
	public Text   nativeTitle;
	public Text   nativeDescription;
	public Text   nativeRating;
	public Text   nativeSponsored;
	public Text   nativeCallToAction;
	public RawImage  nativeIcon;
	public RawImage  nativeMainImage;
	
	public GameObject NativeBlock;
	
    // Start is called before the first frame update
    void Start()
    {
    	btnBanner.enabled = false;
    	btnInterstitial.enabled = false;
    	btnRewarded.enabled = false;
    	btnNative.enabled = false;
    	
        var value = "bla bla";


        // register for initialized callback event in the app
        MoPubManager.OnSdkInitializedEvent += OnSdkInitializedEvent;
        
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
        
        //MoPubManager.OnAdLoadedEvent       += OnAdLoadedEvent;
        //MoPubManager.OnAdLoadedEvent       += OnAdLoadedEvent;



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

        clearAllAds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    //============================================================
    
    // create your handler
    void OnSdkInitializedEvent(string adUnitId)
    {
    	// The SDK is initialized here. Ready to make ad requests.
        btnBanner.enabled = true;
    	btnInterstitial.enabled = true;
    	btnRewarded.enabled = true;
    	btnNative.enabled = true;
    	txtTitle.enabled = true;

		string[] _bannerAdUnits = new string[] {MoPubBannerInventoryHash};
	 	MoPub.LoadBannerPluginsForAdUnits(_bannerAdUnits);

		string[] _interAdUnits = new string[] {MoPubInterstitialInventoryHash};
	 	MoPub.LoadInterstitialPluginsForAdUnits(_interAdUnits);

		string[] _rewardedAdUnits = new string[] {MoPubRewardedInventoryHash};
	 	MoPub.LoadRewardedVideoPluginsForAdUnits(_rewardedAdUnits);

		txtTitle.text = "Initialized";
    }

    //============================================================
    
    public void hideBanner()
    {
  		MoPub.ShowBanner (MoPubBannerInventoryHash, false);   // hides the banner
  	}
    
    public void showBanner()
    {
  		MoPub.ShowBanner (MoPubBannerInventoryHash, true);   // shows the banner
    }
    
    public void hideNative()
    {
    	NativeBlock.SetActive(false);
    }
    
    public void showNative()
    {
    	NativeBlock.SetActive(true);
    }
    
    public void clearAllAds()
    {
    	hideBanner();
    	hideNative();
    	
    	// clear all native fields:
		MySetText(nativeTitle,        null);
		MySetText(nativeDescription,  null);
		MySetText(nativeCallToAction, null);
		MySetText(nativeRating,       null);
		MySetText(nativeSponsored,    null);

		MySetImage(nativeIcon,        null);
		MySetImage(nativeMainImage,   null);
		
		// release all ads
		MoPub.DestroyBanner(MoPubBannerInventoryHash);
    }
    
    //============================================================
    
    public void btnBannerPressed()
    {
	    clearAllAds();

		txtTitle.text = "Loading banner...";

    	MoPub.RequestBanner(MoPubBannerInventoryHash, MoPub.AdPosition.Centered, MoPub.MaxAdSize.Width320Height50);
    }
    
    //------------------------------------------------------------
        
    void OnAdLoadedEvent(string adUnitId, float height)
    {
		txtTitle.text = "Banner loaded";

    	showBanner();
    }
    
    void OnAdFailedEvent(string adUnitId, string errMsg)
    {
		txtTitle.text = errMsg;

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

	     MoPub.RequestInterstitialAd(MoPubInterstitialInventoryHash);
    }
    
    //-------------------------------------------------------------
    
    void OnInterstitialLoadedEvent (string adUnitId)
    {
		txtTitle.text = "Interstitial loaded";

 		MoPub.ShowInterstitialAd (adUnitId);
    }

	void OnInterstitialFailedEvent (string adUnitId, string errorCode)
    {
		txtTitle.text = errorCode;
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
    
		//@@@MobFox.Instance.RequestMobFoxInterstitial ( MoPubInterstitialInventoryHash );
    }
    
    //-------------------------------------------------------------
    
    public void onRewardedLoaded()
    {
    	//@@@MobFox.Instance.ShowMobFoxInterstitial();
    }
    
    public void onRewardedError( string msg)
    {
	    //@@@MobFox.Instance.Log(msg);
    }

    //=============================================================

    public void btnNativePressed()
    {
	    clearAllAds();

    	showNative();
    
    	//@@@MobFox.Instance.setNativeAdContext      ( MobFox.NativeAdContext.CONTENT );
    	//@@@MobFox.Instance.setNativeAdPlacementType( MobFox.NativeAdPlacementType.ATOMIC );

    	//@@@MobFox.Instance.setNativeAdIconImage    ( true, 80 );
    	//@@@MobFox.Instance.setNativeAdMainImage    ( true, 1200, 627 );
    	//@@@MobFox.Instance.setNativeAdTitle        ( true, 100 );
    	//@@@MobFox.Instance.setNativeAdDesc         ( true, 200 );
    
		//@@@MobFox.Instance.RequestMobFoxNative ( MoPubNativeInventoryHash );
    }

    public void btnNativeCallToActionPressed()
    {
	    //@@@MobFox.Instance.callToActionClicked();
    }
    
    //-------------------------------------------------------------
        
    public void onNativeError( string msg)
    {
	    //@@@MobFox.Instance.Log(msg);
    }

    public void onNativeReady(string msg)
    {
    	//@@@MobFox.NativeInfo nativeInfo = JsonUtility.FromJson<MobFox.NativeInfo>(msg);

		//@@@MySetText(nativeTitle,        nativeInfo.title);
		//@@@MySetText(nativeDescription,  nativeInfo.desc);
		//@@@MySetText(nativeCallToAction, nativeInfo.ctatext);
		//@@@MySetText(nativeRating,       nativeInfo.rating);
		//@@@MySetText(nativeSponsored,    nativeInfo.sponsored);

		//@@@MySetImage(nativeIcon,        nativeInfo.iconImageUrl);
		//@@@MySetImage(nativeMainImage,   nativeInfo.mainImageUrl);
    }
    
    //-------------------------------------------------------------

	private void MySetText(Text trg, string txt)
	{
		if (txt==null)
		{
			trg.enabled = false;
		} else {
			trg.enabled = true;
	    	trg.text = txt;
		}
	}
	
	private void MySetImage(RawImage trg, string mediaUrl)
	{
		if (mediaUrl==null)
		{
			trg.enabled = false;
		} else {
			trg.enabled = true;
		    StartCoroutine(DownloadImage(trg, mediaUrl));
		}
	}
    
    IEnumerator DownloadImage(RawImage trg, string mediaUrl)
	{   
    	UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
    	yield return request.SendWebRequest();
    	if(request.isNetworkError || request.isHttpError) 
        	Debug.Log(request.error);
    	else
        	trg.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
	} 

    //=============================================================
}
