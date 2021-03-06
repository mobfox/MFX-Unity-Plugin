﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using GoogleMobileAds;
using GoogleMobileAds.Api;

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
	
	public Button btnNativeCallToAction;
	public Button btnNativeBlock;
	
	public Text   lblLog;
	
	//------------------------------------------------------------

	private int state = 0;
	
	private string asyncMoPubCommand = null;
	private string asyncMoPubParam   = null;
	
	private string asyncTextForLog   = null;
	
	//############################################################
	//#####   U I :                                          #####
	//############################################################

    // Start is called before the first frame update
    void Start()
    {
    	initMobfoxSDK();
    	
    	initMoPubSDK();
    	
    	initAdMobSDK();

        clearAllAds();
        
        updateButtons();
        
        addLog("Unity plugin version: "+MobFox.Instance.getUnityPluginVersion());
        addLog("Mobfox SDK version: "+MobFox.Instance.getNativeSDKVersion());
    }

    // Update is called once per frame
    void Update()
    {
    	if (asyncMoPubCommand!=null)
    	{
    		if (asyncMoPubCommand == "showMoPubInterstitial")
    		{	
    			if (asyncMoPubParam!=null)
    			{
 				    addLog("MoPub interstitial showing...");
    				MoPub.ShowInterstitialAd (asyncMoPubParam);
    				asyncMoPubParam = null;
    			}
    				
    			asyncMoPubCommand = null;
    			return;
    		}
    		
    		if (asyncMoPubCommand == "showMoPubRewarded")
    		{	
    			if (asyncMoPubParam!=null)
    			{
 				    addLog("MoPub rewareded showing...");
    				MoPub.ShowRewardedVideo (asyncMoPubParam);
    				asyncMoPubParam = null;
    			}
    				
    			asyncMoPubCommand = null;
    			return;
    		}

    		asyncMoPubCommand = null;
    	}
    
    	// AdMob native ad reading assets
    	if (this.unifiedNativeAdLoaded)
    	{
        	this.unifiedNativeAdLoaded = false;        	
        	updateAdMobNativeAd();
    	}
    	
    	// logging
    	if (asyncTextForLog != null)
	    {
    		reallyAddLog(asyncTextForLog);
    		asyncTextForLog = null;
	    }
    }
    
    void OnApplicationQuit()
    {
    	// destroy MoPub banners:
    	if (mCurrentMoPubSmallBannerHash != null)
    	{
    		MoPub.DestroyBanner(mCurrentMoPubSmallBannerHash);
    	}
    	if (mCurrentMoPubLargeBannerHash != null)
    	{
    		MoPub.DestroyBanner(mCurrentMoPubLargeBannerHash);
    	}
    	if (mCurrentMoPubVideoBannerHash != null)
    	{
    		MoPub.DestroyBanner(mCurrentMoPubVideoBannerHash);
    	}
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
			setBtnEnabled("Native"           , "lblNative"           , false);
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
    	lblLog.text = "";

    	// clear all native fields:
		MySetText(nativeTitle,        null);
		MySetText(nativeDescription,  null);
		MySetText(nativeCallToAction, null);
		MySetText(nativeRating,       null);
		MySetText(nativeSponsored,    null);
		MySetImage(nativeIcon,        null);
		MySetImage(nativeMainImage,   null);
    	hideMobfoxNative();
    	    	    	
		// release all ads
    	hideMobfoxBanner();
		MobFox.Instance.ReleaseMobFoxBanner();
		MobFox.Instance.ReleaseMobFoxInterstitial();
		MobFox.Instance.ReleaseMobFoxNative();

		if (mCurrentMoPubSmallBannerHash != null)
    	{
    		hideMoPubBanner(mCurrentMoPubSmallBannerHash);
    	}
    	if (mCurrentMoPubLargeBannerHash != null)
    	{
    		hideMoPubBanner(mCurrentMoPubLargeBannerHash);
    	}
    	if (mCurrentMoPubVideoBannerHash != null)
    	{
    		hideMoPubBanner(mCurrentMoPubVideoBannerHash);

    		MoPub.DestroyBanner(mCurrentMoPubVideoBannerHash);
    		mCurrentMoPubVideoBannerHash = null;
    	}

    	hideAdMobBanner();
    	if (adMobBannerView!=null)
    	{
    		adMobBannerView.Destroy();
    		adMobBannerView = null;
    	}
    	
    	if (adMobInterstitial!=null)
    	{
    		adMobInterstitial.Destroy();
    		adMobInterstitial = null;
    	}
    }
        
    //------------------------------------------------------------

    private void addLog(string message)
    {
        string time  = System.DateTime.Now.ToString("[HH:mm:ss] ");
        string toAdd = time + message + "\n";

    	if (asyncTextForLog == null)
    	{
    		asyncTextForLog = toAdd;
    	} else {
    		asyncTextForLog = asyncTextForLog + toAdd;
    	}

    	MobFox.Instance.Log(message);
    }
    
    private void reallyAddLog(string message)
    {
    	string txt = lblLog.text;
    	txt = txt + message;
    	lblLog.text = txt;    	
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
	    	startAdMobNative();
    		break;
    	}
    }


    public void btnNativeCallToActionPressed()
    {
    	switch (state)
    	{
    	case 0:	// Mobfox
		    handleMobfoxNativeClick();
    		break;
    	case 1:	// MoPub
    		handleMoPubNativeClick();
    		break;
    	case 2:	// AdMob
    		handleAdMobNativeClick();
    		break;
    	}
    }

    public void btnNativeBlockPressed()
    {
    	switch (state)
    	{
    	case 0:	// Mobfox
		    handleMobfoxNativeClick();
    		break;
    	case 1:	// MoPub
    		handleMoPubNativeClick();
    		break;
    	case 2:	// AdMob
    		handleAdMobNativeClick();
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
        MobFox.OnBannerReady          += onBannerLoaded;
        MobFox.OnBannerError          += onBannerError;
        MobFox.OnBannerShown          += onBannerShown;
        MobFox.OnBannerClicked        += onBannerClicked;
        MobFox.OnBannerFinished       += onBannerFinished;
        MobFox.OnBannerClosed         += onBannerClosed;
        
    	MobFox.OnInterstitialReady    += onInterLoaded;
    	MobFox.OnInterstitialError    += onInterError;
        MobFox.OnInterstitialShown    += onInterShown;
        MobFox.OnInterstitialClicked  += onInterClicked;
        MobFox.OnInterstitialFinished += onInterFinished;
        MobFox.OnInterstitialClosed   += onInterClosed;

        MobFox.OnNativeReady          += onNativeReady;
        MobFox.OnNativeError          += onNativeError;
        MobFox.OnNativeClicked        += onNativeClicked;

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
	    clearAllAds();
    
    	addLog("Loading Mobfox banner");
    	
		MobFox.Instance.RequestMobFoxBanner ( MobFoxBannerInventoryHash, 40, 130, 320, 50 );
		MobFox.Instance.setBannerRefresh(30);
    }
    
    public void startMobfoxLargeBanner()
    {
	    clearAllAds();
    
    	addLog("Loading Mobfox banner");
    
		MobFox.Instance.RequestMobFoxBanner ( MobFoxBannerInventoryHash, 50, 130, 300, 250 );
		MobFox.Instance.setBannerRefresh(30);
    }
    
    public void startMobfoxVideoBanner()
    {
	    clearAllAds();
    
    	addLog("Loading Mobfox banner");
    
		MobFox.Instance.RequestMobFoxBanner ( MobFoxVideoBannerInventoryHash, 50, 130, 300, 250 );
		MobFox.Instance.setBannerRefresh(0);
    }
        
    //-------------------------------------------------------------
    
    public void onBannerLoaded()
    {
    	addLog("Mobfox banner loaded");
	    showMobfoxBanner();
    }

    public void onBannerError( string msg)
    {
    	addLog("Mobfox banner err: "+msg);
    }

    public void onBannerShown()
    {
    	addLog("Mobfox banner shown");
    }

    public void onBannerClicked(string url)
    {
    	addLog("Mobfox banner clicked");
    }

    public void onBannerFinished()
    {
    	addLog("Mobfox banner finished");
    }

    public void onBannerClosed()
    {
    	addLog("Mobfox banner closed");
    }

    //=============================================================
    
    public void startMobfoxHtmlInterstitial()
    {
	    clearAllAds();
    
        addLog("Loading Mobfox interstitial");

		MobFox.Instance.RequestMobFoxInterstitial ( MobFoxInterstitialInventoryHash );
    }
    
    public void startMobfoxVideoInterstitial()
    {
	    clearAllAds();
    
        addLog("Loading Mobfox interstitial");

		MobFox.Instance.RequestMobFoxInterstitial ( MobFoxVideoInterstitialInventoryHash );
    }
    
    //-------------------------------------------------------------
    
    public void onInterLoaded()
    {
    	addLog("Mobfox interstitial loaded");

    	MobFox.Instance.ShowMobFoxInterstitial();
    }
    
    public void onInterError( string msg)
    {
    	addLog("Mobfox interstitial err: "+msg);
    }

    public void onInterShown()
    {
    	addLog("Mobfox interstitial shown");
    }

    public void onInterClicked(string url)
    {
    	addLog("Mobfox interstitial clicked");
    }

    public void onInterFinished()
    {
    	addLog("Mobfox interstitial finished");
    }

    public void onInterClosed()
    {
    	addLog("Mobfox interstitial closed");
    }

    //=============================================================

    public void startMobfoxNative()
    {
    	clearAllAds();

    	showMobfoxNative();

        addLog("Loading Mobfox native");
    
    	MobFox.Instance.setNativeAdContext      ( MobFox.NativeAdContext.CONTENT );
    	MobFox.Instance.setNativeAdPlacementType( MobFox.NativeAdPlacementType.ATOMIC );

    	MobFox.Instance.setNativeAdIconImage    ( true, 80 );
    	MobFox.Instance.setNativeAdMainImage    ( true, 1200, 627 );
    	MobFox.Instance.setNativeAdTitle        ( true, 100 );
    	MobFox.Instance.setNativeAdDesc         ( true, 200 );
    
		MobFox.Instance.RequestMobFoxNative ( MobFoxNativeInventoryHash );
    }
    
    private void handleMobfoxNativeClick()
    {
		MobFox.Instance.callToActionClicked();
    }
    
    //-------------------------------------------------------------
        
    public void onNativeError( string msg)
    {
    	addLog("Mobfox native err: "+msg);
    }

    public void onNativeReady(string msg)
    {
    	addLog("Mobfox native loaded");

    	MobFox.NativeInfo nativeInfo = JsonUtility.FromJson<MobFox.NativeInfo>(msg);

		MySetText(nativeTitle,        nativeInfo.title);
		MySetText(nativeDescription,  nativeInfo.desc);
		MySetText(nativeCallToAction, nativeInfo.ctatext);
		MySetText(nativeRating,       nativeInfo.rating);
		MySetText(nativeSponsored,    nativeInfo.sponsored);

		MySetImage(nativeIcon,        nativeInfo.iconImageUrl);
		MySetImage(nativeMainImage,   nativeInfo.mainImageUrl);
  	}
    
    public void onNativeClicked()
    {
    	addLog("Mobfox native clicked");
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
	private string MoPubBannerInventoryHash       = "234dd5a1b1bf4a5f9ab50431f9615784";	// Test iOS App / Banner Ad
	private string MoPubBannerLargeInvh           = "5f239da8a483458eb8e5983418994040";	// Test iOS App / Banner Large Ad
	private string MoPubBannerVideoInvh           = "62f37ebc4c0b40359a26af136d1e0866";	// Test iOS App / Banner Video
	private string MoPubInterstitialInventoryHash = "6aee1c416d44412ca9978b4355902d3f";	// Test iOS App / iOS Inter html
	private string MoPubInterVideoInvh            = "a5277fa1fd57418b867cfaa949df3b4a";	// Test iOS App / Fullscreen Ad-demoApp
	private string MoPubRewardedInventoryHash     = "e3d4c8701d4547e68e8f837fa4fe5122";	// Test iOS App / iOS Rewarded
    private string MoPubNativeInvh                = "ac0f139a2d9544fface76d06e27bc02a"; // Test iOS App / Native Ad
#endif

	private string mCurrentMoPubSmallBannerHash = null;
	private string mCurrentMoPubLargeBannerHash = null;
	private string mCurrentMoPubVideoBannerHash = null;

    //=============================================================
    
    private void initMoPubSDK()
    {
        var value = "bla bla";

		asyncMoPubCommand = null;

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

		string[] _bannerAdUnits = new string[] {MoPubBannerInventoryHash, MoPubBannerLargeInvh, MoPubBannerVideoInvh};
	 	MoPub.LoadBannerPluginsForAdUnits(_bannerAdUnits);

		string[] _interAdUnits = new string[] {MoPubInterstitialInventoryHash, MoPubInterVideoInvh};
	 	MoPub.LoadInterstitialPluginsForAdUnits(_interAdUnits);

		string[] _rewardedAdUnits = new string[] {MoPubRewardedInventoryHash};
	 	MoPub.LoadRewardedVideoPluginsForAdUnits(_rewardedAdUnits);

		// not implemented yet - MoPub does not support mediation in Unity
		//string[] _nativeAdUnits = new string[] {MoPubNativeInvh};
	 	//MoPub.LoadNativePluginsForAdUnits(_nativeAdUnits);
    }

    //============================================================
    
    public void hideMoPubBanner(string adUnitId)
    {
    	if (adUnitId!=null)
    	{
	  		MoPub.ShowBanner(adUnitId, false);
    		MoPub.SetAutorefresh(adUnitId, false);
    	}
    }
    
    public void showMoPubBanner(string adUnitId)
    {
    	if (adUnitId!=null)
    	{
	  		MoPub.ShowBanner (adUnitId, true);
    	}
    }
    
    //============================================================
    
    private void startMoPubSmallBanner()
    {
		clearAllAds();
		
        if (mCurrentMoPubSmallBannerHash == null)
    	{
			mCurrentMoPubSmallBannerHash = MoPubBannerInventoryHash;
    		MoPub.RequestBanner(mCurrentMoPubSmallBannerHash, MoPub.AdPosition.BottomCenter, MoPub.MaxAdSize.Width320Height50);
	
			addLog("Creating MoPub banner");
    	} else {
    	
    		MoPub.SetAutorefresh(mCurrentMoPubSmallBannerHash, true);
    		MoPub.ForceRefresh(mCurrentMoPubSmallBannerHash);
    	
			addLog("Reloading MoPub banner");
    	}
	}
	
    private void startMoPubLargeBanner()
    {
		clearAllAds();
		
        if (mCurrentMoPubLargeBannerHash == null)
    	{
			mCurrentMoPubLargeBannerHash = MoPubBannerLargeInvh;
	    	MoPub.RequestBanner(mCurrentMoPubLargeBannerHash, MoPub.AdPosition.BottomCenter, MoPub.MaxAdSize.Width300Height250);
	
			addLog("Creating MoPub banner");
    	} else {
    	
    		MoPub.SetAutorefresh(mCurrentMoPubLargeBannerHash, true);
    		MoPub.ForceRefresh(mCurrentMoPubLargeBannerHash);
    	
			addLog("Reloading MoPub banner");
    	}
	}
	
    private void startMoPubVideoBanner()
    {
		clearAllAds();
		
        if (mCurrentMoPubVideoBannerHash == null)
    	{
			mCurrentMoPubVideoBannerHash = MoPubBannerVideoInvh;
	    	MoPub.RequestBanner(mCurrentMoPubVideoBannerHash, MoPub.AdPosition.BottomCenter, MoPub.MaxAdSize.Width300Height250);
	
			addLog("Creating MoPub banner");
    	} else {
    	
    		MoPub.SetAutorefresh(mCurrentMoPubVideoBannerHash, true);
    		MoPub.ForceRefresh(mCurrentMoPubVideoBannerHash);
    	
			addLog("Reloading MoPub banner");
    	}
	}

    //------------------------------------------------------------
        
    void OnAdLoadedEvent(string adUnitId, float height)
    {    
		addLog("MoPub banner loaded");

    	showMoPubBanner(adUnitId);
    }
    
    void OnAdFailedEvent(string adUnitId, string errMsg)
    {
		addLog("MoPub banner load err: "+errMsg);

    	showMoPubBanner(adUnitId);
    }

    void OnAdClickedEvent(string adUnitId)
    {
		addLog("MoPub banner clicked");
    }

    void OnAdExpandedEvent(string adUnitId)
    {
		addLog("MoPub banner ad expanded");
    }

    void OnAdCollapsedEvent(string adUnitId)
    {
		addLog("MoPub banner ad collapsed");
    }

    //=============================================================
    
    private void startMoPubHtmlInterstitial()
    {
	    clearAllAds();
    
		addLog("Loading MoPub interstitial");

		MoPub.RequestInterstitialAd(MoPubInterstitialInventoryHash);
    }
    
	private void startMoPubVideoInterstitial()
	{
	    clearAllAds();
    
		addLog("Loading MoPub interstitial");

		MoPub.RequestInterstitialAd(MoPubInterVideoInvh);
	}
	
    //-------------------------------------------------------------
    
    void OnInterstitialLoadedEvent (string adUnitId)
    {
		addLog("MoPub interstitial loaded");

 		//MoPub.ShowInterstitialAd (adUnitId);
		asyncMoPubCommand = "showMoPubInterstitial";
		asyncMoPubParam   = adUnitId;
    }

	void OnInterstitialFailedEvent (string adUnitId, string errorCode)
    {
		addLog("MoPub interstitial error: "+errorCode);
    }

	void OnInterstitialDismissedEvent (string adUnitId)
    {
		addLog("MoPub interstitial closed");
    }

	void OnInterstitialExpiredEvent (string adUnitId)
    {
		addLog("MoPub interstitial expired");
    }

	void OnInterstitialShownEvent (string adUnitId)
    {
		addLog("MoPub interstitial shown");
    }

	void OnInterstitialClickedEvent (string adUnitId)
    {
		addLog("MoPub interstitial clicked");
    }

    //=============================================================
    
    private void startMoPubRewarded()
    {
	    clearAllAds();
    
    	addLog("Loading MoPub rewarded");

		MoPub.RequestRewardedVideo(MoPubRewardedInventoryHash);
    }
    
    //-------------------------------------------------------------
    
	void OnRewardedVideoLoadedEvent (string adUnitId)
    {
		addLog("MoPub rewarded loaded");

		//MoPub.ShowRewardedVideo(adUnitId);
		asyncMoPubCommand = "showMoPubRewarded";
		asyncMoPubParam   = adUnitId;
    }

	void OnRewardedVideoFailedEvent (string adUnitId, string errorMsg)
    {
		addLog("MoPub rewarded load failed: "+errorMsg);
    }

	void OnRewardedVideoExpiredEvent (string adUnitId)
    {
		addLog("MoPub rewarded expired");
    }

	void OnRewardedVideoShownEvent (string adUnitId)
    {
		addLog("MoPub rewarded shown");
    }

	void OnRewardedVideoClickedEvent (string adUnitId)
    {
		addLog("MoPub rewarded clicked");
    }

	void OnRewardedVideoFailedToPlayEvent (string adUnitId, string errorMsg)
    {
		addLog("MoPub rewarded play failed: "+errorMsg);
    }

	void OnRewardedVideoReceivedRewardEvent (string adUnitId, string label, float amount)
    {
		addLog("MoPub received reward: "+amount+" "+label);
    }

	void OnRewardedVideoClosedEvent (string adUnitId)
    {
		addLog("MoPub rewarded closed");
    }

	void OnRewardedVideoLeavingApplicationEvent (string adUnitId)
    {
		addLog("MoPub rewarded leaving app");
    }
    
    //=============================================================

	private void startMoPubNative()
	{
		clearAllAds();
		
    	showMobfoxNative();

		// not implemented yet - MoPub does not support mediation in Unity

	}

    private void handleMoPubNativeClick()
    {
		// not implemented yet - MoPub does not support mediation in Unity
    }

	//############################################################
	//#####   A D M O B :                                    #####
	//############################################################

    #if UNITY_ANDROID
        string AdMobBannerHash       = "ca-app-pub-8111915318550857/5234422920";
    	string AdMobBannerVideoInvh  = "ca-app-pub-6224828323195096/9231617215";   // sdk.mobfox.com.appcore
		string AdMobInterstitialHash = "ca-app-pub-8111915318550857/9385420926";
	    string AdMobInterVideoInvh   = "ca-app-pub-8111915318550857/7271416015";   // sdk.mobfox.com.appcore
		string AdMobRewardedHash     = "ca-app-pub-6224828323195096/1152622735";
	    string AdMobNativeInvh       = "ca-app-pub-6224828323195096/1268034150";   // Native Android For AdMob
	    //string AdMobNativeInvh       = "ca-app-pub-3940256099942544/2247696110";	// AdMob test ad
    #else
        string AdMobBannerHash       = "ca-app-pub-6224828323195096/7846687276";	// Test iOS - Test iOS Banner
        string AdMobBannerVideoInvh  = "ca-app-pub-6224828323195096/7835888455";	// Test iOS - Test iOS Banner Video
		string AdMobInterstitialHash = "ca-app-pub-6224828323195096/7876284361";	// Test iOS - Test iOS Inter		
		string AdMobInterVideoInvh   = "ca-app-pub-6224828323195096/4477534086";	// Test iOS - inter video
		string AdMobRewardedHash     = "ca-app-pub-6224828323195096/9409251358";	// Test iOS - rewarded_adunit_for_testing
	    string AdMobNativeInvh       = "ca-app-pub-6224828323195096/9365553005";	// Test iOS - AdMob Native iOS
	    //string AdMobNativeInvh       = "ca-app-pub-3940256099942544/3986624511";	// AdMob test ad

//#define ADMOB_HASH_BANNER_HTML  @"ca-app-pub-6224828323195096/5240875564"

    #endif

	private BannerView      adMobBannerView;
    private InterstitialAd  adMobInterstitial;
    private RewardedAd      adMobRewardedAd;
	private UnifiedNativeAd adMobNativeAd;

	private bool            unifiedNativeAdLoaded;

    //=============================================================

	private void initAdMobSDK()
	{
        #if UNITY_ANDROID
            string appId = "ca-app-pub-6224828323195096~8368193162";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-6224828323195096~3764142368";
        #else
            string appId = "unexpected_platform";
        #endif

        MobileAds.SetiOSAppPauseOnBackground(true);
    	
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
	}
	
    //============================================================
    
    public void hideAdMobBanner()
    {
    	if (adMobBannerView!=null)
    	{
    		adMobBannerView.Hide();
    	}
  	}
    
    public void showAdMobBanner()
    {
    	if (adMobBannerView!=null)
    	{
    		adMobBannerView.Show();
    	}
    }
    
    //============================================================

	// Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
#if UNITY_ANDROID
            .AddTestDevice("82109714761F90BAAD73679C21E34E56")
#else // UNITY_IOS
            .AddTestDevice("a7976a724a3aba85f2dd656fd180c203")
#endif
            //.AddKeyword("game")
            //.SetGender(Gender.Male)
            //.SetBirthday(new DateTime(1985, 1, 1))
            //.TagForChildDirectedTreatment(false)
            .Build();
    }
    
    //=============================================================

	private void startAdMobSmallBanner()
	{
	    clearAllAds();

		addLog("Loading AdMob banner...");

        adMobBannerView = new BannerView(AdMobBannerHash, AdSize.Banner, 40, 130);

        // Called when an ad request has successfully loaded.
        adMobBannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        adMobBannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        adMobBannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        adMobBannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        adMobBannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Load the banner with the request.
        adMobBannerView.LoadAd(CreateAdRequest());
	}
	
	private void startAdMobLargeBanner()
	{
	    clearAllAds();

		addLog("Loading AdMob banner...");

        adMobBannerView = new BannerView(AdMobBannerHash, AdSize.MediumRectangle, 50, 130);

        // Called when an ad request has successfully loaded.
        adMobBannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        adMobBannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        adMobBannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        adMobBannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        adMobBannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Load the banner with the request.
        adMobBannerView.LoadAd(CreateAdRequest());
	}
	
	private void startAdMobVideoBanner()
	{
	    clearAllAds();

		addLog("Loading AdMob banner...");

        adMobBannerView = new BannerView(AdMobBannerVideoInvh, AdSize.MediumRectangle, 50, 130);

        // Called when an ad request has successfully loaded.
        adMobBannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        adMobBannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        adMobBannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        adMobBannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        adMobBannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Load the banner with the request.
        adMobBannerView.LoadAd(CreateAdRequest());
	}
	
    //------------------------------------------------------------

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
		addLog("AdMob Banner loaded");

		showAdMobBanner();    	
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
		addLog("AdMob Banner error: "+args.Message);

    	showAdMobBanner();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
		addLog("AdMob Banner clicked");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
		addLog("AdMob Banner closed");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
		addLog("AdMob Banner clicked");
    }

    //=============================================================

	private void startAdMobHtmlInterstitial()
	{
	    clearAllAds();
    
		addLog("Loading AdMob interstitial...");

		// Initialize an InterstitialAd.
    	adMobInterstitial = new InterstitialAd(AdMobInterstitialHash);
    
        // Called when an ad request has successfully loaded.
    	adMobInterstitial.OnAdLoaded += OnInterstitialLoadedEvent;
    	// Called when an ad request failed to load.
    	adMobInterstitial.OnAdFailedToLoad += OnInterstitialFailedEvent;
    	// Called when an ad is shown.
    	adMobInterstitial.OnAdOpening += OnInterstitialOpened;
    	// Called when the ad is closed.
    	adMobInterstitial.OnAdClosed += OnInterstitialClosed;
    	// Called when the ad click caused the user to leave the application.
    	adMobInterstitial.OnAdLeavingApplication += OnInterstitialLeavingApplication;
  
    	// Load the interstitial with the request.
    	adMobInterstitial.LoadAd(CreateAdRequest());
	}
	
	private void startAdMobVideoInterstitial()
	{
	    clearAllAds();
    
		addLog("Loading AdMob interstitial...");

		// Initialize an InterstitialAd.
    	adMobInterstitial = new InterstitialAd(AdMobInterVideoInvh);
    
        // Called when an ad request has successfully loaded.
    	adMobInterstitial.OnAdLoaded += OnInterstitialLoadedEvent;
    	// Called when an ad request failed to load.
    	adMobInterstitial.OnAdFailedToLoad += OnInterstitialFailedEvent;
    	// Called when an ad is shown.
    	adMobInterstitial.OnAdOpening += OnInterstitialOpened;
    	// Called when the ad is closed.
    	adMobInterstitial.OnAdClosed += OnInterstitialClosed;
    	// Called when the ad click caused the user to leave the application.
    	adMobInterstitial.OnAdLeavingApplication += OnInterstitialLeavingApplication;
  
    	// Load the interstitial with the request.
    	adMobInterstitial.LoadAd(CreateAdRequest());
	}
	
    //-------------------------------------------------------------
    
    void OnInterstitialLoadedEvent(object sender, EventArgs args)
    {
		addLog("AdMob Interstitial loaded");
 		
 		if ((adMobInterstitial!=null) && (adMobInterstitial.IsLoaded()))
 		{
 			addLog("AdMob interstitial showing...");

    		adMobInterstitial.Show();
  		}
    }

	void OnInterstitialFailedEvent (object sender, AdFailedToLoadEventArgs args)
    {
		addLog("AdMob Interstitial error: "+args.Message);
    }

	public void OnInterstitialOpened(object sender, EventArgs args)
	{
    	addLog("AdMob Interstitial shown");
	}

	public void OnInterstitialClosed(object sender, EventArgs args)
	{
    	addLog("AdMob Interstitial closed");
	}

	public void OnInterstitialLeavingApplication(object sender, EventArgs args)
	{
    	addLog("AdMob Interstitial clicked");
	}

    //=============================================================

	private void startAdMobRewarded()
	{
	    clearAllAds();
    
		addLog("Loading AdMob rewarded...");
		
		adMobRewardedAd = new RewardedAd(AdMobRewardedHash);

        // Called when an ad request has successfully loaded.
        adMobRewardedAd.OnAdLoaded += OnRewardedAdLoaded;
        // Called when an ad request failed to load.
        adMobRewardedAd.OnAdFailedToLoad += OnRewardedAdFailedToLoad;
        // Called when an ad is shown.
        adMobRewardedAd.OnAdOpening += OnRewardedAdOpening;
        // Called when an ad request failed to show.
        adMobRewardedAd.OnAdFailedToShow += OnRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        adMobRewardedAd.OnUserEarnedReward += OnUserEarnedReward;
        // Called when the ad is closed.
        adMobRewardedAd.OnAdClosed += OnRewardedAdClosed;

		adMobRewardedAd.LoadAd(CreateAdRequest());
	}

    //-------------------------------------------------------------
    
    public void OnRewardedAdLoaded(object sender, EventArgs args)
    {
        addLog("AdMob Rewarded loaded");
 		
 		if ((adMobRewardedAd!=null) && (adMobRewardedAd.IsLoaded()))
 		{
 			addLog("AdMob rewarded showing...");

    		adMobRewardedAd.Show();
  		}
    }

    public void OnRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        addLog("AdMob Rewarded failed: " + args.Message);
    }

    public void OnRewardedAdOpening(object sender, EventArgs args)
    {
        addLog("AdMob Rewarded shown");
    }

    public void OnRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        addLog("AdMob Rewarded failed to show: " + args.Message);
    }

    public void OnRewardedAdClosed(object sender, EventArgs args)
    {
        addLog("AdMob Rewarded closed");
    }

    public void OnUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
		addLog("AdMob Got " + amount.ToString() + " " + type);
    }
	
    //=============================================================

	private void startAdMobNative()
	{
	    clearAllAds();
    		
    	showMobfoxNative();

		addLog("Loading AdMob native...");
	
	    this.unifiedNativeAdLoaded = false;
	
    	AdLoader adLoader = new AdLoader.Builder(AdMobNativeInvh)
        	.ForUnifiedNativeAd()
        	.Build();
    	adLoader.OnUnifiedNativeAdLoaded += this.HandleUnifiedNativeAdLoaded;
    	adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
    	adLoader.LoadAd(CreateAdRequest());
	}

    //-------------------------------------------------------------

    private void handleAdMobNativeClick()
    {
		// mytodo:
    }

    //-------------------------------------------------------------

	private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		addLog("AdMob native err: " + args.Message);
	}

	private void HandleUnifiedNativeAdLoaded(object sender, UnifiedNativeAdEventArgs args)
	{
		addLog("AdMob native loaded");

    	adMobNativeAd = args.nativeAd;
	    this.unifiedNativeAdLoaded = true;
	}

    private void updateAdMobNativeAd()
    {    	    
        addLog("AdMob native - rendering");

		//----- read assets and update display: ---------------

        // Get strings of native ad.
	    string headline    = this.adMobNativeAd.GetHeadlineText();	     
	    string description = this.adMobNativeAd.GetBodyText();
	    string cta         = this.adMobNativeAd.GetCallToActionText();
		string advertiser  = this.adMobNativeAd.GetAdvertiserText();
		double rating      = this.adMobNativeAd.GetStarRating();
		string strRating;
		if (rating<0.0)
		{
			strRating = "";
		} else {
			strRating = rating.ToString();
		}
			    
	    MySetText(nativeTitle,        headline);
		MySetText(nativeDescription,  description);
		MySetText(nativeCallToAction, cta);
		MySetText(nativeSponsored,    advertiser);
		MySetText(nativeRating,       strRating);

	    // Get Texture2D for icon asset of native ad.
    	Texture2D iconTexture = this.adMobNativeAd.GetIconTexture();
		if (iconTexture==null)
		{
    		addLog("AdMob native - no icon image");
			nativeIcon.enabled = false;
		} else {
			nativeIcon.enabled = true;
		    nativeIcon.texture = iconTexture;
		}
		
		Texture2D mainTexture = null;
		if (this.adMobNativeAd.GetImageTextures().Count > 0)
        {
    		mainTexture = this.adMobNativeAd.GetImageTextures()[0];
		}
    	if (mainTexture==null)
    	{
    		addLog("AdMob native - no main image");
    		nativeMainImage.enabled = false;
    	} else {
    		nativeMainImage.enabled = true;
			nativeMainImage.texture = mainTexture;
    	}

    	//----- register UI objects for clicking, etc.: -------
    	
    	nativeTitle.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(nativeTitle.gameObject))
	    {
        	addLog("AdMob native - did not register nativeTitle");
	    } else {
        	addLog("AdMob native - registered nativeTitle");
	    }

    	nativeDescription.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(nativeDescription.gameObject))
	    {
        	addLog("AdMob native - did not register nativeDescription");
	    } else {
        	addLog("AdMob native - registered nativeDescription");
	    }
    	nativeCallToAction.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(nativeCallToAction.gameObject))
	    {
        	addLog("AdMob native - did not register nativeCallToAction");
	    } else {
        	addLog("AdMob native - registered nativeCallToAction");
	    }
    	nativeRating.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(nativeRating.gameObject))
	    {
        	addLog("AdMob native - did not register nativeRating");
	    } else {
        	addLog("AdMob native - registered nativeRating");
	    }
    	nativeSponsored.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(nativeSponsored.gameObject))
	    {
        	addLog("AdMob native - did not register nativeSponsored");
	    } else {
        	addLog("AdMob native - registered nativeSponsored");
	    }
    	nativeIcon.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(nativeIcon.gameObject))
	    {
        	addLog("AdMob native - did not register nativeIcon");
	    } else {
        	addLog("AdMob native - registered nativeIcon");
	    }
    	nativeMainImage.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(nativeMainImage.gameObject))
	    {
        	addLog("AdMob native - did not register nativeMainImage");
	    } else {
        	addLog("AdMob native - registered nativeMainImage");
	    }


    	btnNativeCallToAction.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(btnNativeCallToAction.gameObject))
	    {
        	addLog("AdMob native - did not register btnNativeCallToAction");
	    } else {
        	addLog("AdMob native - registered btnNativeCallToAction");
	    }

    	btnNativeBlock.gameObject.AddComponent<BoxCollider>();
	    if (!this.adMobNativeAd.RegisterIconImageGameObject(btnNativeBlock.gameObject))
	    {
        	addLog("AdMob native - did not register btnNativeBlock");
	    } else {
        	addLog("AdMob native - registered btnNativeBlock");
	    }
    }

    //=============================================================
}
