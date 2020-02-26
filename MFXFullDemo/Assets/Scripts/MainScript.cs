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
			setBtnEnabled("Native"           , "lblNative"           , true);
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
		MobFox.Instance.ReleaseMobFoxBanner();
		MobFox.Instance.ReleaseMobFoxInterstitial();
		MobFox.Instance.ReleaseMobFoxNative();
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
    		break;
    	case 2:	// AdMob
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
    		break;
    	case 2:	// AdMob
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
    		break;
    	case 2:	// AdMob
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
    		break;
    	case 2:	// AdMob
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
    		break;
    	case 2:	// AdMob
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
    		break;
    	case 2:	// AdMob
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
    		break;
    	case 2:	// AdMob
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
    
    public void hideBanner()
    {
    	MobFox.Instance.HideMobFoxBanner();
    }
    
    public void showBanner()
    {
    	MobFox.Instance.ShowMobFoxBanner();
    }
    
    public void hideNative()
    {
    	NativeBlock.SetActive(false);
    }
    
    public void showNative()
    {
    	NativeBlock.SetActive(true);
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
	    showBanner();
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
    	showNative();
    
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

    private string mopubBannerInvh          = "4ad212b1d0104c5998b288e7a8e35967";    // Android MobFox Adapter / Test Hash Banner(DONT CHANGE)
    private string mopubBannerLargeInvh     = "9cae0a5af35c4a96b033a67f4b49160c";    // Android MobFox Adapter / Test Hash Large Banner(DONT CHANGE)
    private string mopubBannerVideoInvh     = "ce377e29b9e94ca484efbbf8201f7e36";    // Android MobFox Adapter / Test Hash Video Banner(DONT CHANGE)
    private string mopubInterstitialInvh    = "3fd85a3e7a9d43ea993360a2536b7bbd";    // Android MobFox Adapter / Test Hash Interstitial(DONT CHANGE)
    private string mopubInterVideoInvh      = "6ee6c2cf27074af8a1a7117f8b21b0d9";    // Android MobFox Adapter / Test Hash Inter Video (DONT CHANGE)
    private string mopubNativeInvh          = "e2758ffdaf0d426aa19a633bab6bbc3a";    // Android MobFox Adapter / Test Hash Native (DONT CHANGE)
    private string mopubRewardedInvh        = "005491feb31848a0ae7b9daf4a46c701";    // Android MobFox Adapter / Test Hash Rewarded (DONT CHANGE)
    

    //=============================================================
}
