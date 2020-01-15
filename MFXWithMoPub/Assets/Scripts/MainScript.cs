using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainScript : MonoBehaviour
{
	private string MoPubBannerInventoryHash             = "4ad212b1d0104c5998b288e7a8e35967";	// Mobfox test banner
	private string MoPubBannerLargeInventoryHash        = "bf453fccdfe74af0ab8f6a944d6ae97a";	// Mobfox test banner
    
	private string MobFoxInterstitialInventoryHash      = "3fd85a3e7a9d43ea993360a2536b7bbd";
	private string MobFoxVideoInterstitialInventoryHash = "562f11d6b8f2499dbd0d1ebfe3c17968";
	private string MobFoxRewardedInventoryHash          = "005491feb31848a0ae7b9daf4a46c701";
	
	private string MobFoxNativeInventoryHash            = "b146b367940a4c6da94e8143fb4b66e4";


	public Button btnSmallBanner;
	public Button btnLargeBanner;
	public Button btnVideoBanner;
	public Button btnHtmlInterstitial;
	public Button btnVideoInterstitial;
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
    	btnSmallBanner.enabled = false;
    	btnLargeBanner.enabled = false;
    	btnVideoBanner.enabled = false;
    	btnHtmlInterstitial.enabled = false;
    	btnVideoInterstitial.enabled = false;
    	btnNative.enabled = false;
    	
                        var value = "bla bla";


        // register for initialized callback event in the app
        MoPubManager.OnSdkInitializedEvent += OnSdkInitializedEvent;
        MoPubManager.OnAdLoadedEvent       += OnAdLoadedEvent;
        MoPubManager.OnAdFailedEvent       += OnAdFailedEvent;

        //MoPubManager.OnAdLoadedEvent       += OnAdLoadedEvent;
        //MoPubManager.OnAdLoadedEvent       += OnAdLoadedEvent;
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
        btnSmallBanner.enabled = true;
	    btnLargeBanner.enabled = true;
    	btnVideoBanner.enabled = true;
    	btnHtmlInterstitial.enabled = true;
	    btnVideoInterstitial.enabled = true;
    	btnNative.enabled = true;

		string[] _bannerAdUnits = new string[] {MoPubBannerInventoryHash, MoPubBannerLargeInventoryHash};
	 	MoPub.LoadBannerPluginsForAdUnits(_bannerAdUnits);

		btnSmallBanner.GetComponentInChildren<Text>().text = "la di da";
    }
    
    void OnAdLoadedEvent(string adUnitId, float height)
    {
    	btnSmallBanner.GetComponentInChildren<Text>().text = "WoooHooo";

    	showBanner();
    }
    
    void OnAdFailedEvent(string adUnitId, string errMsg)
    {
    	btnSmallBanner.GetComponentInChildren<Text>().text = errMsg;

    	showBanner();
    }

    //============================================================
    
    public void hideBanner()
    {
  		MoPub.ShowBanner (MoPubBannerInventoryHash, false);   // hides the banner
  	}
    
    public void showBanner()
    {
  		MoPub.ShowBanner (MoPubBannerLargeInventoryHash, true);   // shows the banner
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
		MoPub.DestroyBanner(MoPubBannerLargeInventoryHash);
    }
    
    //============================================================
    
    public void btnSmallBannerPressed()
    {
	    clearAllAds();

		btnSmallBanner.GetComponentInChildren<Text>().text = "Loading...";

    	MoPub.RequestBanner(MoPubBannerInventoryHash, MoPub.AdPosition.Centered, MoPub.MaxAdSize.Width320Height50);
    }
    
    public void btnLargeBannerPressed()
    {
	    clearAllAds();

    	MoPub.RequestBanner(MoPubBannerLargeInventoryHash, MoPub.AdPosition.Centered, MoPub.MaxAdSize.Width300Height250);
    }
    
    public void btnVideoBannerPressed()
    {
	    clearAllAds();

		//@@@MobFox.Instance.RequestMobFoxBanner ( MobFoxVideoBannerInventoryHash, 50, 100, 300, 250 );
		//@@@MobFox.Instance.setBannerRefresh(0);
    }
        
    //-------------------------------------------------------------
    
    public void onBannerLoaded()
    {
	    showBanner();
    }

    public void onBannerError( string msg)
    {
	    //@@@MobFox.Instance.Log(msg);
    }

    //=============================================================
    
    public void btnHtmlInterPressed()
    {
	    clearAllAds();
    
		//@@@MobFox.Instance.RequestMobFoxInterstitial ( MobFoxInterstitialInventoryHash );
    }
    
    public void btnVideoInterPressed()
    {
	    clearAllAds();
    
		//@@@MobFox.Instance.RequestMobFoxInterstitial ( MobFoxVideoInterstitialInventoryHash );
    }
    
    //-------------------------------------------------------------
    
    public void onInterLoaded()
    {
    	//@@@MobFox.Instance.ShowMobFoxInterstitial();
    }
    
    public void onInterError( string msg)
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
    
		//@@@MobFox.Instance.RequestMobFoxNative ( MobFoxNativeInventoryHash );
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
