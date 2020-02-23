using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainScript : MonoBehaviour
{
	private string MobFoxBannerInventoryHash            = "fe96717d9875b9da4339ea5367eff1ec";
	private string MobFoxVideoBannerInventoryHash       = "80187188f458cfde788d961b6882fd53";
	private string MobFoxInterstitialInventoryHash      = "267d72ac3f77a3f447b32cf7ebf20673";
	private string MobFoxVideoInterstitialInventoryHash = "80187188f458cfde788d961b6882fd53";
	//private string MobFoxNativeInventoryHash            = "a764347547748896b84e0b8ccd90fd62";	// "normal" test hash
	//private string MobFoxNativeInventoryHash            = "d8bd50e4ba71a708ad224464bdcdc237";	// "new" test hash
	private string MobFoxNativeInventoryHash            = "d8bd50e4ba71a708ad224464bdcdc237";   //"d22bf35c596809155ec8520d283a9b09";	// "live" hash


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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //============================================================
    
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
    
    //============================================================
    
    public void btnSmallBannerPressed()
    {
	    clearAllAds();
    
		MobFox.Instance.RequestMobFoxBanner ( MobFoxBannerInventoryHash, 40, 100, 320, 50 );
		MobFox.Instance.setBannerRefresh(0);
    }
    
    public void btnLargeBannerPressed()
    {
	    clearAllAds();

		MobFox.Instance.RequestMobFoxBanner ( MobFoxBannerInventoryHash, 50, 100, 300, 250 );
		MobFox.Instance.setBannerRefresh(0);
    }
    
    public void btnVideoBannerPressed()
    {
	    clearAllAds();

		MobFox.Instance.RequestMobFoxBanner ( MobFoxVideoBannerInventoryHash, 50, 100, 300, 250 );
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
    
    public void btnHtmlInterPressed()
    {
	    clearAllAds();
    
		MobFox.Instance.RequestMobFoxInterstitial ( MobFoxInterstitialInventoryHash );
    }
    
    public void btnVideoInterPressed()
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

    public void btnNativePressed()
    {
	    clearAllAds();

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

    //=============================================================
}
