using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainScript : MonoBehaviour
{
	public Button   btnMobfox;
	public Image    imgMobfox;
	
	public Button   btnMoPub;
	public Image    imgMoPub;
	
	public Button   btnAdMob;
	public Image    imgAdMob;

	//------------------------------------------------------------

	public Button btnSmallBanner;
	public Text   lblSmallBanner;
	public Button btnLargeBanner;
	public Text   lblLargeBanner;
	public Button btnVideoBanner;
	public Text   lblVideoBanner;
	public Button btnHtmlInterstitial;
	public Text   lblHtmlInterstitial;
	public Button btnVideoInterstitial;
	public Text   lblVideoInterstitial;
	public Button btnRewarded;
	public Text   lblRewarded;
	public Button btnNative;
	public Text   lblNative;
	
	public Text   nativeTitle;
	public Text   nativeDescription;
	public Text   nativeRating;
	public Text   nativeSponsored;
	public Text   nativeCallToAction;
	public RawImage  nativeIcon;
	public RawImage  nativeMainImage;
	
	public GameObject NativeBlock;
	
	//------------------------------------------------------------

	private int state = 0;
	
	//############################################################
	//#####   U I :                                          #####
	//############################################################

    // Start is called before the first frame update
    void Start()
    {
    	initMobfoxSDK();
    	
    	initMoPubSDK();

        clearAllAds();
        
        updateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //------------------------------------------------------------
    
    private void updateButtons()
    {
    	switch (state)
    	{
    	case 0:	// Mobfox
			imgMobfox.sprite = Resources.Load<Sprite>("mobfox_logo");
			imgMoPub.sprite  = Resources.Load<Sprite>("mopub_logo_grey");
			imgAdMob.sprite  = Resources.Load<Sprite>("admob_logo_grey");
			
			setBtnEnabled("SmallBanner"      , "lblSmallBanner"      , true);
			setBtnEnabled("LargeBanner"      , "lblLargeBanner"      , true);
			setBtnEnabled("VideoBanner"      , "lblVideoBanner"      , true);
			setBtnEnabled("HtmlInterstitial" , "lblHtmlInterstitial" , true);
			setBtnEnabled("VideoInterstitial", "lblVideoInterstitial", true);
			setBtnEnabled("Rewarded"         , "lblRewarded"         , false);
			setBtnEnabled("Native"           , "lblNative"           , true);
    		break;
    	case 1:	// MoPub
			imgMobfox.sprite = Resources.Load<Sprite>("mobfox_logo_grey");
			imgMoPub.sprite  = Resources.Load<Sprite>("mopub_logo");
			imgAdMob.sprite  = Resources.Load<Sprite>("admob_logo_grey");
			
			setBtnEnabled("SmallBanner"      , "lblSmallBanner"      , true);
			setBtnEnabled("LargeBanner"      , "lblLargeBanner"      , true);
			setBtnEnabled("VideoBanner"      , "lblVideoBanner"      , true);
			setBtnEnabled("HtmlInterstitial" , "lblHtmlInterstitial" , true);
			setBtnEnabled("VideoInterstitial", "lblVideoInterstitial", true);
			setBtnEnabled("Rewarded"         , "lblRewarded"         , true);
			setBtnEnabled("Native"           , "lblNative"           , false);
    		break;
    	case 2:	// AdMob
			imgMobfox.sprite = Resources.Load<Sprite>("mobfox_logo_grey");
			imgMoPub.sprite  = Resources.Load<Sprite>("mopub_logo_grey");
			imgAdMob.sprite  = Resources.Load<Sprite>("admob_logo");
			
			setBtnEnabled("SmallBanner"      , "lblSmallBanner"      , true);
			setBtnEnabled("LargeBanner"      , "lblLargeBanner"      , true);
			setBtnEnabled("VideoBanner"      , "lblVideoBanner"      , true);
			setBtnEnabled("HtmlInterstitial" , "lblHtmlInterstitial" , true);
			setBtnEnabled("VideoInterstitial", "lblVideoInterstitial", true);
			setBtnEnabled("Rewarded"         , "lblRewarded"         , true);
			setBtnEnabled("Native"           , "lblNative"           , true);
    		break;
    	}
    }
    
    //------------------------------------------------------------
    
    private void setBtnEnabled(string btnTag, string lblTag, bool bOn)
    {
    	Button btn = GameObject.FindGameObjectWithTag(btnTag).GetComponent<Button>();
    	Text   lbl = GameObject.FindGameObjectWithTag(lblTag).GetComponent<Text>();

    	btn.interactable = bOn;
    	lbl.color        = bOn?Color.black:Color.gray;
    }
    
    //------------------------------------------------------------
    
    public void clearAllAds()
    {
    	hideMobfoxBanner();
    	hideMobfoxNative();
    	
    	hideMoPubBanner();
    	
    	// clear all native fields:
		MySetText(nativeTitle,        null);
		MySetText(nativeDescription,  null);
		MySetText(nativeCallToAction, null);
		MySetText(nativeRating,       null);
		MySetText(nativeSponsored,    null);

		MySetImage(nativeIcon,        null);
		MySetImage(nativeMainImage,   null);
		
		// release all ads
		MobFox.Instance.ReleaseMobFoxBanner();
		MobFox.Instance.ReleaseMobFoxInterstitial();
		MobFox.Instance.ReleaseMobFoxNative();
		
		MoPub.DestroyBanner(MoPubBannerInventoryHash);
    }
        
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
    
    //############################################################
	//#####   B u t t o n   p r e s s e s                    #####
	//############################################################

    public void btnMobfoxPressed()
    {
	    clearAllAds();	    
	    state = 0;
	    updateButtons();
    }

    public void btnMoPubPressed()
    {
	    clearAllAds();	    
	    state = 1;
	    updateButtons();
    }

    public void btnAdMobPressed()
    {
	    clearAllAds();	    
	    state = 2;
	    updateButtons();
    }

	//------------------------------------------------------------

    public void btnSmallBannerPressed()
    {
	    clearAllAds();
    
    	switch (state)
    	{
    	case 0:	// Mobfox
	    	startMobfoxSmallBanner();
    		break;
    	case 1:	// MoPub
	    	startMoPubSmallBanner();
    		break;
    	case 2:	// AdMob
	    	startAdMobSmallBanner();
    		break;
    	}
    }
    
    public void btnLargeBannerPressed()
    {
	    clearAllAds();
	    
    	switch (state)
    	{
    	case 0:	// Mobfox
		    startMobfoxLargeBanner();
    		break;
    	case 1:	// MoPub
		    startMoPubLargeBanner();
    		break;
    	case 2:	// AdMob
		    startAdMobLargeBanner();
    		break;
    	}
    }
    
    public void btnVideoBannerPressed()
    {
	    clearAllAds();

    	switch (state)
    	{
    	case 0:	// Mobfox
		    startMobfoxVideoBanner();
    		break;
    	case 1:	// MoPub
		    startMoPubVideoBanner();
    		break;
    	case 2:	// AdMob
		    startAdMobVideoBanner();
    		break;
    	}
    }

    public void btnHtmlInterPressed()
    {
	    clearAllAds();
    
    	switch (state)
    	{
    	case 0:	// Mobfox
		    startMobfoxHtmlInterstitial();
    		break;
    	case 1:	// MoPub
		    startMoPubHtmlInterstitial();
    		break;
    	case 2:	// AdMob
		    startAdMobHtmlInterstitial();
    		break;
    	}
    }
    
    public void btnVideoInterPressed()
    {
	    clearAllAds();
    
    	switch (state)
    	{
    	case 0:	// Mobfox
		    startMobfoxVideoInterstitial();
    		break;
    	case 1:	// MoPub
		    startMoPubVideoInterstitial();
    		break;
    	case 2:	// AdMob
		    startAdMobVideoInterstitial();
    		break;
    	}
    }
    
    public void btnRewardedPressed()
    {
	    clearAllAds();
    
    	switch (state)
    	{
    	case 0:	// Mobfox
	    	// NOP
    		break;
    	case 1:	// MoPub
		    startMoPubRewarded();
    		break;
    	case 2:	// AdMob
		    startAdMobRewarded();
    		break;
    	}
    }
    
    public void btnNativePressed()
    {
	    clearAllAds();
    
    	switch (state)
    	{
    	case 0:	// Mobfox
	    	startMobfoxNative();
    		break;
    	case 1:	// MoPub
	    	startMoPubNative();
    		break;
    	case 2:	// AdMob
	    	startAdMobbNative();
    		break;
    	}
    }

	//############################################################
	//#####   M O B F O X :                                  #####
	//############################################################

	private string MobFoxBannerInventoryHash            = "fe96717d9875b9da4339ea5367eff1ec";
	private string MobFoxVideoBannerInventoryHash       = "80187188f458cfde788d961b6882fd53";
	private string MobFoxInterstitialInventoryHash      = "267d72ac3f77a3f447b32cf7ebf20673";
	private string MobFoxVideoInterstitialInventoryHash = "80187188f458cfde788d961b6882fd53";
	private string MobFoxNativeInventoryHash            = "d8bd50e4ba71a708ad224464bdcdc237"; 
    
    public void hideMobfoxBanner()
    {
    	MobFox.Instance.HideMobFoxBanner();
    }
    
    public void showMobfoxBanner()
    {
    	MobFox.Instance.ShowMobFoxBanner();
    }
    
    public void hideMobfoxNative()
    {
    	NativeBlock.SetActive(false);
    }
    
    public void showMobfoxNative()
    {
    	NativeBlock.SetActive(true);
    }
    
    //============================================================
    
    private void initMobfoxSDK()
    {
    	MobFox.CreateSingletone ( );
    
    	// set listeners:
        MobFox.OnBannerReady       += onBannerLoaded;
        MobFox.OnBannerError       += onBannerError;
        
    	MobFox.OnInterstitialReady += onInterLoaded;
    	MobFox.OnInterstitialError += onInterError;

        MobFox.OnNativeReady       += onNativeReady;
        MobFox.OnNativeError       += onNativeError;
        
        MobFox.Instance.setDemoAge("32");
        MobFox.Instance.setDemoGender("male");
        MobFox.Instance.setDemoKeywords("basketball,tennis");
        MobFox.Instance.setLatitude(32.455666);
        MobFox.Instance.setLongitude(32.455666);
        
        MobFox.Instance.setCOPPA(true);
    }

    //============================================================
    
    public void startMobfoxSmallBanner()
    {
		MobFox.Instance.RequestMobFoxBanner ( MobFoxBannerInventoryHash, 40, 92, 320, 50 );
		MobFox.Instance.setBannerRefresh(0);
    }
    
    public void startMobfoxLargeBanner()
    {
		MobFox.Instance.RequestMobFoxBanner ( MobFoxBannerInventoryHash, 50, 92, 300, 250 );
		MobFox.Instance.setBannerRefresh(0);
    }
    
    public void startMobfoxVideoBanner()
    {
		MobFox.Instance.RequestMobFoxBanner ( MobFoxVideoBannerInventoryHash, 50, 92, 300, 250 );
		MobFox.Instance.setBannerRefresh(0);
    }
        
    //-------------------------------------------------------------
    
    public void onBannerLoaded()
    {
	    showMobfoxBanner();
    }

    public void onBannerError( string msg)
    {
    }

    //=============================================================
    
    public void startMobfoxHtmlInterstitial()
    {
	    clearAllAds();
    
		MobFox.Instance.RequestMobFoxInterstitial ( MobFoxInterstitialInventoryHash );
    }
    
    public void startMobfoxVideoInterstitial()
    {
	    clearAllAds();
    
		MobFox.Instance.RequestMobFoxInterstitial ( MobFoxVideoInterstitialInventoryHash );
    }
    
    //-------------------------------------------------------------
    
    public void onInterLoaded()
    {
    	MobFox.Instance.ShowMobFoxInterstitial();
    }
    
    public void onInterError( string msg)
    {
    }

    //=============================================================

    public void startMobfoxNative()
    {
    	showMobfoxNative();
    
    	MobFox.Instance.setNativeAdContext      ( MobFox.NativeAdContext.CONTENT );
    	MobFox.Instance.setNativeAdPlacementType( MobFox.NativeAdPlacementType.ATOMIC );

    	MobFox.Instance.setNativeAdIconImage    ( true, 80 );
    	MobFox.Instance.setNativeAdMainImage    ( true, 1200, 627 );
    	MobFox.Instance.setNativeAdTitle        ( true, 100 );
    	MobFox.Instance.setNativeAdDesc         ( true, 200 );
    
		MobFox.Instance.RequestMobFoxNative ( MobFoxNativeInventoryHash );
    }

    public void btnNativeCallToActionPressed()
    {
	    MobFox.Instance.callToActionClicked();
    }
    
    //-------------------------------------------------------------
        
    public void onNativeError( string msg)
    {
    }

    public void onNativeReady(string msg)
    {
    	MobFox.NativeInfo nativeInfo = JsonUtility.FromJson<MobFox.NativeInfo>(msg);

		MySetText(nativeTitle,        nativeInfo.title);
		MySetText(nativeDescription,  nativeInfo.desc);
		MySetText(nativeCallToAction, nativeInfo.ctatext);
		MySetText(nativeRating,       nativeInfo.rating);
		MySetText(nativeSponsored,    nativeInfo.sponsored);

		MySetImage(nativeIcon,        nativeInfo.iconImageUrl);
		MySetImage(nativeMainImage,   nativeInfo.mainImageUrl);
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

	//############################################################
	//#####   M O P U B :                                    #####
	//############################################################

#if UNITY_ANDROID
	private string MoPubBannerInventoryHash       = "4ad212b1d0104c5998b288e7a8e35967";	// Android MobFox Adapter / Test Hash Banner(DONT CHANGE)
    private string MoPubBannerLargeInvh           = "9cae0a5af35c4a96b033a67f4b49160c"; // Android MobFox Adapter / Test Hash Large Banner(DONT CHANGE)
    private string MoPubBannerVideoInvh           = "ce377e29b9e94ca484efbbf8201f7e36"; // Android MobFox Adapter / Test Hash Video Banner(DONT CHANGE)
	private string MoPubInterstitialInventoryHash = "3fd85a3e7a9d43ea993360a2536b7bbd";	// Android MobFox Adapter / Test Hash Interstitial(DONT CHANGE)
    private string MoPubInterVideoInvh            = "6ee6c2cf27074af8a1a7117f8b21b0d9"; // Android MobFox Adapter / Test Hash Inter Video (DONT CHANGE)
    private string MoPubNativeInvh                = "e2758ffdaf0d426aa19a633bab6bbc3a"; // Android MobFox Adapter / Test Hash Native (DONT CHANGE)
	private string MoPubRewardedInventoryHash     = "005491feb31848a0ae7b9daf4a46c701";	// Android MobFox Adapter / Test Hash Rewarded (DONT CHANGE)
#else
	private string MoPubBannerInventoryHash             = "234dd5a1b1bf4a5f9ab50431f9615784";	// Mobfox test banner
	private string MoPubInterstitialInventoryHash       = "a5277fa1fd57418b867cfaa949df3b4a";
	private string MoPubRewardedInventoryHash           = "e3d4c8701d4547e68e8f837fa4fe5122";
#endif

	private string mCurrentMoPubBannerHash = "";

    //=============================================================
    
    private void initMoPubSDK()
    {
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
                    //MediationSettingsClassName = "MobFoxMoPubNativeAdRendererSettings",
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
    }

    //============================================================
    
    // create your handler
    void OnSdkInitializedEvent(string adUnitId)
    {
    	// The SDK is initialized here. Ready to make ad requests.
		_ShowAndroidToastMessage("MoPub initialized");

		string[] _bannerAdUnits = new string[] {MoPubBannerInventoryHash, MoPubBannerLargeInvh, MoPubBannerVideoInvh};
	 	MoPub.LoadBannerPluginsForAdUnits(_bannerAdUnits);

		string[] _interAdUnits = new string[] {MoPubInterstitialInventoryHash, MoPubInterVideoInvh};
	 	MoPub.LoadInterstitialPluginsForAdUnits(_interAdUnits);

		string[] _rewardedAdUnits = new string[] {MoPubRewardedInventoryHash};
	 	MoPub.LoadRewardedVideoPluginsForAdUnits(_rewardedAdUnits);

		string[] _nativeAdUnits = new string[] {MoPubNativeInvh};
	 	// mytodo: MoPub.LoadNativePluginsForAdUnits(_nativeAdUnits);
    }

    //============================================================
    
    public void hideMoPubBanner()
    {
  		MoPub.ShowBanner (mCurrentMoPubBannerHash, false);   // hides the banner
  	}
    
    public void showMoPubBanner()
    {
  		MoPub.ShowBanner (mCurrentMoPubBannerHash, true);   // shows the banner
    }
    
    //============================================================
    
    private void startMoPubSmallBanner()
    {
	    clearAllAds();

		mCurrentMoPubBannerHash = MoPubBannerInventoryHash;
    	MoPub.RequestBanner(mCurrentMoPubBannerHash, MoPub.AdPosition.Centered, MoPub.MaxAdSize.Width320Height50);

		_ShowAndroidToastMessage("Loading MoPub banner");
    }
    
    private void startMoPubLargeBanner()
	{
	    clearAllAds();

		mCurrentMoPubBannerHash = MoPubBannerLargeInvh;
    	MoPub.RequestBanner(mCurrentMoPubBannerHash, MoPub.AdPosition.Centered, MoPub.MaxAdSize.Width300Height250);

		_ShowAndroidToastMessage("Loading MoPub banner");
	}
	
	private void startMoPubVideoBanner()
	{
	    clearAllAds();

		mCurrentMoPubBannerHash = MoPubBannerVideoInvh;
    	MoPub.RequestBanner(mCurrentMoPubBannerHash, MoPub.AdPosition.Centered, MoPub.MaxAdSize.Width300Height250);

		_ShowAndroidToastMessage("Loading MoPub banner");
	}

    //------------------------------------------------------------
        
    void OnAdLoadedEvent(string adUnitId, float height)
    {    
		_ShowAndroidToastMessage("MoPub banner loaded");

    	showMoPubBanner();
    }
    
    void OnAdFailedEvent(string adUnitId, string errMsg)
    {
		_ShowAndroidToastMessage("MoPub banner load err: "+errMsg);

    	showMoPubBanner();
    }

    void OnAdClickedEvent(string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub banner clicked");
    }

    void OnAdExpandedEvent(string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub banner ad expanded");
    }

    void OnAdCollapsedEvent(string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub banner ad collapsed");
    }

    //=============================================================
    
    private void startMoPubHtmlInterstitial()
    {
	    clearAllAds();
    
		_ShowAndroidToastMessage("Loading MoPub interstitial");

	     MoPub.RequestInterstitialAd(MoPubInterstitialInventoryHash);
    }
    
	private void startMoPubVideoInterstitial()
	{
	    clearAllAds();
    
		_ShowAndroidToastMessage("Loading MoPub interstitial");

	     MoPub.RequestInterstitialAd(MoPubInterVideoInvh);
	}
	
    //-------------------------------------------------------------
    
    void OnInterstitialLoadedEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub interstitial loaded");

 		MoPub.ShowInterstitialAd (adUnitId);
    }

	void OnInterstitialFailedEvent (string adUnitId, string errorCode)
    {
		_ShowAndroidToastMessage("MoPub interstitial error: "+errorCode);
    }

	void OnInterstitialDismissedEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub interstitial loaded");
    }

	void OnInterstitialExpiredEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub interstitial expired");
    }

	void OnInterstitialShownEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub interstitial shown");
    }

	void OnInterstitialClickedEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub interstitial clicked");
    }

    //=============================================================
    
    private void startMoPubRewarded()
    {
	    clearAllAds();
    
    	_ShowAndroidToastMessage("Loading MoPub rewarded");

		MoPub.RequestRewardedVideo(MoPubRewardedInventoryHash);
    }
    
    //-------------------------------------------------------------
    
	void OnRewardedVideoLoadedEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub rewarded loaded");

		MoPub.ShowRewardedVideo(adUnitId);
    }

	void OnRewardedVideoFailedEvent (string adUnitId, string errorMsg)
    {
		_ShowAndroidToastMessage("MoPub rewarded load failed: "+errorMsg);
    }

	void OnRewardedVideoExpiredEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub rewarded expired");
    }

	void OnRewardedVideoShownEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub rewarded shown");
    }

	void OnRewardedVideoClickedEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub rewarded clicked");
    }

	void OnRewardedVideoFailedToPlayEvent (string adUnitId, string errorMsg)
    {
		_ShowAndroidToastMessage("MoPub rewarded play failed: "+errorMsg);
    }

	void OnRewardedVideoReceivedRewardEvent (string adUnitId, string label, float amount)
    {
		_ShowAndroidToastMessage("MoPub received reward: "+amount+" "+label);
    }

	void OnRewardedVideoClosedEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub rewarded closed");
    }

	void OnRewardedVideoLeavingApplicationEvent (string adUnitId)
    {
		_ShowAndroidToastMessage("MoPub rewarded leaving app");
    }
    
    //=============================================================

	private void startMoPubNative()
	{
		// mytodo: not implemented yet - MoPub does not support mediation in Unity
	}

	//############################################################
	//#####   A D M O B :                                    #####
	//############################################################

	private void initAdMobSDK()
	{
	}
	
    //=============================================================

	private void startAdMobSmallBanner()
	{
	}
	
	private void startAdMobLargeBanner()
	{
	}
	
	private void startAdMobVideoBanner()
	{
	}
	
	private void startAdMobHtmlInterstitial()
	{
	}
	
	private void startAdMobVideoInterstitial()
	{
	}
	
	private void startAdMobRewarded()
	{
	}
	
	private void startAdMobbNative()
	{
	}

    //=============================================================
}
