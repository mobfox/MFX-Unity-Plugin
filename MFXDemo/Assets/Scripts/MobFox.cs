using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class MobFox :MonoBehaviour
{
	[DllImport ( "__Internal" )]
	public static extern void _setGameObject (string gameObject);
	

	[DllImport ( "__Internal" )]
	public static extern void _setSubjectToGDPR (bool subjectToGDPR);

	[DllImport ( "__Internal" )]
	public static extern void _setGDPRConsentString (string consentString);

	[DllImport ( "__Internal" )]
	public static extern void _setDemoAge (string demoAge);

	[DllImport ( "__Internal" )]
	public static extern void _setDemoGender (string demoGender);

	[DllImport ( "__Internal" )]
	public static extern void _setDemoKeywords (string demoKeywords);

	[DllImport ( "__Internal" )]
	public static extern void _setLatitude (double latitude);

	[DllImport ( "__Internal" )]
	public static extern void _setLongitude (double longitude);



	[DllImport ( "__Internal" )]
	public static extern int _createBanner (string invh, float originX, float originY, float sizeWidth, float sizeHeight);

	[DllImport ( "__Internal" )]
	public static extern void _hideBanner ();

	[DllImport ( "__Internal" )]
	public static extern void _showBanner ();

	[DllImport ( "__Internal" )]
	public static extern void _setBannerRefresh (int intervalInSeconds);

	[DllImport ( "__Internal" )]
	public static extern void _setBannerFloorPrice (float floorPrice);

	[DllImport ( "__Internal" )]
	public static extern void _releaseBanner ();



	[DllImport ( "__Internal" )]
	public static extern void _createInterstitial (string invh);

	[DllImport ( "__Internal" )]
	public static extern void _showInterstitial ();

	[DllImport ( "__Internal" )]
	public static extern void _setInterstitialFloorPrice (float floorPrice);

	[DllImport ( "__Internal" )]
	public static extern void _releaseInterstitial ();



	[DllImport ( "__Internal" )]
	public static extern void _createNative (string invh);

	[DllImport ( "__Internal" )]
	public static extern void _setNativeFloorPrice (float floorPrice);
	
	[DllImport ( "__Internal" )]
	public static extern void _callToActionClicked ();

	[DllImport ( "__Internal" )]
	public static extern void _setNativeAdContext (string adContext);

	[DllImport ( "__Internal" )]
	public static extern void _setNativeAdPlacementType (string adType);

	[DllImport ( "__Internal" )]
	public static extern void _setNativeAdIconImage ( bool bRequired, int size );

	[DllImport ( "__Internal" )]
	public static extern void _setNativeAdMainImage ( bool bRequired, int width, int height );

	[DllImport ( "__Internal" )]
	public static extern void _setNativeAdTitle ( bool bRequired, int maxLength );

	[DllImport ( "__Internal" )]
	public static extern void _setNativeAdDesc ( bool bRequired, int maxLength );

	[DllImport ( "__Internal" )]
	public static extern void _releaseNative ();



	private const string MobFoxGameObjectName = "MobFoxObject";


	bool InterstitialReady = false;

	private const  string            pluginName = "com.mobfox.unity.plugin.MobFoxPlugin";
	private static AndroidJavaClass  _mobFoxPluginClass = null;
	private static AndroidJavaObject _mobFoxPluginInstance = null;
	private static AndroidJavaObject activityContext = null;


	//======================================================================================
	//======  I N I T                                                                 ======
	//======================================================================================

	public static Action
		OnSdkReady,
		
		OnBannerReady,
		OnBannerShown,
		OnBannerClosed,
		OnBannerFinished,
		
		OnInterstitialReady,
		OnInterstitialShown,
		OnInterstitialClosed,
		OnInterstitialFinished,
		
		OnNativeClicked;
	
	public static Action<string>
		OnBannerError,
		OnBannerClicked,
		
		OnInterstitialError,
		OnInterstitialClicked,
		
		OnNativeError,
		OnNativeReady;

	private static bool debug_mode = true;
	
	
	//======================================================================================

	public static AndroidJavaClass PluginClass
	{
		get {
			if (_mobFoxPluginClass==null)
			{
				_mobFoxPluginClass = new AndroidJavaClass(pluginName);
			}
			return _mobFoxPluginClass;
		}
	}

	public static AndroidJavaObject PluginInstance
	{
		get {
			if (_mobFoxPluginInstance==null)
			{
				_mobFoxPluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
			}
			return _mobFoxPluginInstance;
		}
	}

	//======================================================================================

	void ConnectToPlugin ()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			//===== Get current activity context: =====
			if (activityContext == null)
			{
				using (AndroidJavaClass activityClass = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" ))
				{
					activityContext = activityClass.GetStatic<AndroidJavaObject> ( "currentActivity" );
				}
			}

			if (activityContext != null)
			{
				activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
					{
						//===== Set context in plugin: =====
						PluginInstance.Call("setContext", activityContext);

						//===== Set game object in plugin: =====
						PluginInstance.Call( "setGameObject", MobFoxGameObjectName );
					} ) );
			}
		} else {
			_setGameObject ( MobFoxGameObjectName );
		}

		if (OnSdkReady != null)
		{
			OnSdkReady ( );
		}
	}

	//======================================================================================


	private static MobFox instance;

	public static MobFox Instance
	{
		get
		{
			CreateSingletone ( );
			return instance;
		}
	}

	private static void LogMessage (string message, bool error = false)
	{
		if (debug_mode)
		{
			if (error)
			{
				Debug.Log ( "MobFox :: " + message );
			}
			else
			{
				Debug.Log ( "MobFox :: " + message );
			}
			ShowToast ( message );
		}
	}

	static private void ShowToast (string message)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (activityContext != null)
			{
				activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
					{
						PluginInstance.Call ( "showMessage", message );
					} ) );
			}
		}
		else
		{
		}
	}

	public static void CreateSingletone ()
	{
		if (instance == null)
		{
			GameObject e_man = new GameObject ( MobFoxGameObjectName );
			e_man.AddComponent<MobFox> ( );
		}
	}


	static void SetInstance (MobFox _instance)
	{
		if (instance == null)
		{
			instance = _instance;
			DontDestroyOnLoad ( instance.gameObject );
			instance.ConnectToPlugin ( );
		}
	}


	void Awake ()
	{
		SetInstance ( this );
	}

	//======================================================================================
	//======  H e l p e r   F u n c t i o n s                                         ======
	//======================================================================================
	
	public void Log(string msg)
	{
		LogMessage(msg, false);
	}

	//======================================================================================
	//======  G L O B A L                                                             ======
	//======================================================================================

	public void setSubjectToGDPR (bool subjectToGDPR)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setSubjectToGDPR_Android ( subjectToGDPR );
		}
		else
		{
			setSubjectToGDPR_iPhone ( subjectToGDPR );
		}
	}

	//-------------------------------------------

	private void setSubjectToGDPR_iPhone (bool subjectToGDPR)
	{
		_setSubjectToGDPR(subjectToGDPR);
	}

	//-------------------------------------------

	private void setSubjectToGDPR_Android (bool subjectToGDPR)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setSubjectToGDPR", subjectToGDPR );
			}));
		}
	}

	//======================================================================================

	public void setGDPRConsentString (string consentString)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setGDPRConsentString_Android ( consentString );
		}
		else
		{
			setGDPRConsentString_iPhone ( consentString );
		}
	}

	//-------------------------------------------

	private void setGDPRConsentString_iPhone (string consentString)
	{
		_setGDPRConsentString (consentString);
	}

	//-------------------------------------------

	private void setGDPRConsentString_Android (string consentString)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setGDPRConsentString", consentString );
			}));
		}
	}

	//======================================================================================

	public void setDemoAge (string demoAge)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setDemoAge_Android ( demoAge );
		}
		else
		{
			setDemoAge_iPhone ( demoAge );
		}
	}

	//-------------------------------------------

	private void setDemoAge_iPhone (string demoAge)
	{
		_setDemoAge (demoAge);
	}

	//-------------------------------------------

	private void setDemoAge_Android (string demoAge)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setDemoAge", demoAge );
			}));
		}
	}

	//======================================================================================

	public void setDemoGender (string demoGender)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setDemoGender_Android ( demoGender );
		}
		else
		{
			setDemoGender_iPhone ( demoGender );
		}
	}

	//-------------------------------------------

	private void setDemoGender_iPhone (string demoGender)
	{
		_setDemoGender (demoGender);
	}

	//-------------------------------------------

	private void setDemoGender_Android (string demoGender)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setDemoGender", demoGender );
			}));
		}
	}

	//======================================================================================

	public void setDemoKeywords (string demoKeywords)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setDemoKeywords_Android ( demoKeywords );
		}
		else
		{
			setDemoKeywords_iPhone ( demoKeywords );
		}
	}

	//-------------------------------------------

	private void setDemoKeywords_iPhone (string demoKeywords)
	{
		_setDemoKeywords (demoKeywords);
	}

	//-------------------------------------------

	private void setDemoKeywords_Android (string demoKeywords)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setDemoKeywords", demoKeywords );
			}));
		}
	}

	//======================================================================================

	public void setLatitude (double latitude)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setLatitude_Android ( latitude );
		}
		else
		{
			setLatitude_iPhone ( latitude );
		}
	}

	//-------------------------------------------

	private void setLatitude_iPhone (double latitude)
	{
		_setLatitude (latitude);
	}

	//-------------------------------------------

	private void setLatitude_Android (double latitude)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setLatitude", latitude );
			}));
		}
	}


	//======================================================================================

	public void setLongitude (double longitude)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setLongitude_Android ( longitude );
		}
		else
		{
			setLongitude_iPhone ( longitude );
		}
	}

	//-------------------------------------------

	private void setLongitude_iPhone (double longitude)
	{
		_setLongitude (longitude);
	}

	//-------------------------------------------

	private void setLongitude_Android (double longitude)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setLongitude", longitude );
			}));
		}
	}

	//======================================================================================
	//======  B A N N E R                                                             ======
	//======================================================================================
	
	public void RequestMobFoxBanner (string banner_inventory, int left, int top, int width, int height)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			ShowMobFoxBanner_Android ( banner_inventory, left, top, width, height );
		}
		else
		{
			ShowMobFoxBanner_iPhone ( banner_inventory, left, top, width, height );
		}
	}
	
	//-------------------------------------------

	private void ShowMobFoxBanner_iPhone (string banner_inventory, int left, int top, int width, int height)
	{
		_createBanner ( banner_inventory, left, top, width, height );
	}

	//-------------------------------------------

	private void ShowMobFoxBanner_Android (string banner_inventory, int left, int top, int width, int height)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "createBanner", banner_inventory, left, top, width, height );
				} ) );
		}
	}

	//======================================================================================

	public void setBannerRefresh (int intervalInSeconds)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setBannerRefresh_Android ( intervalInSeconds );
		}
		else
		{
			setBannerRefresh_iPhone ( intervalInSeconds );
		}
	}

	//-------------------------------------------

	private void setBannerRefresh_iPhone (int intervalInSeconds)
	{
		_setBannerRefresh(intervalInSeconds);
	}

	//-------------------------------------------

	private void setBannerRefresh_Android (int intervalInSeconds)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setBannerRefresh", intervalInSeconds );
			}));
		}
	}

	//======================================================================================

	public void setBannerFloorPrice (float floorPrice)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setBannerFloorPrice_Android ( floorPrice );
		}
		else
		{
			setBannerFloorPrice_iPhone ( floorPrice );
		}
	}

	//-------------------------------------------

	private void setBannerFloorPrice_iPhone (float floorPrice)
	{
		_setBannerFloorPrice(floorPrice);
	}

	//-------------------------------------------

	private void setBannerFloorPrice_Android (float floorPrice)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setBannerFloorPrice", floorPrice );
			}));
		}
	}

	//======================================================================================

	public void HideMobFoxBanner ()
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			HideMobFoxBanner_Android ( );
		}
		else
		{
			HideMobFoxBanner_iPhone ( );
		}
	}

	//-------------------------------------------

	private void HideMobFoxBanner_iPhone ()
	{
		_hideBanner ( );
	}

	//-------------------------------------------

	private void HideMobFoxBanner_Android ()
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "hideBanner" );
				} ) );
		}
	}

	//======================================================================================

	public void ShowMobFoxBanner ()
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			UnhideMobFoxBanner_Android ( );
		}
		else
		{
			UnhideMobFoxBanner_iPhone ( );
		}
	}

	//-------------------------------------------

	private void UnhideMobFoxBanner_iPhone ()
	{
		_showBanner ( );
	}

	//-------------------------------------------

	private void UnhideMobFoxBanner_Android ()
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "showBanner" );
				} ) );
		}
	}

	//======================================================================================

	public void ReleaseMobFoxBanner ()
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			ReleaseMobFoxBanner_Android ( );
		}
		else
		{
			ReleaseMobFoxBanner_iPhone ( );
		}
	}

	//-------------------------------------------

	private void ReleaseMobFoxBanner_iPhone ()
	{
		_releaseBanner ( );
	}

	//-------------------------------------------

	private void ReleaseMobFoxBanner_Android ()
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "releaseBanner" );
				} ) );
		}
	}

	//======================================================================================

	public void bannerReady (string msg)
	{
		if (OnBannerReady != null)
		{
			OnBannerReady ( );
		}
	}

	public void bannerShown (string msg)
	{
		if (OnBannerShown != null)
		{
			OnBannerShown ( );
		}
	}

	public void bannerError (string msg)
	{
		if (OnBannerError != null)
		{
			OnBannerError ( msg );
		}
	}

	public void bannerClosed (string msg)
	{
		if (OnBannerClosed != null)
		{
			OnBannerClosed ( );
		}
	}

	public void bannerClicked (string msg)
	{
		if (OnBannerClicked != null)
		{
			OnBannerClicked ( msg );
		}
	}

	public void bannerFinished (string msg)
	{
		if (OnBannerFinished != null)
		{
			OnBannerFinished ( );
		}
	}

	//======================================================================================
	//======  I N T E R S T I T I A L                                                 ======
	//======================================================================================

	public void RequestMobFoxInterstitial (string interstitial_inventory)
	{
		//bCreateAndShowInterstitial = false;
		InterstitialReady = false;

		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			CreateMobFoxInterstitial_Android ( interstitial_inventory );
		}
		else
		{
			CreateMobFoxInterstitial_iPhone ( interstitial_inventory );
		}
	}

	//======================================================================================

	private void CreateMobFoxInterstitial_iPhone (string interstitial_inventory)
	{
		_createInterstitial ( interstitial_inventory );
	}

	private void CreateMobFoxInterstitial_Android (string interstitial_inventory)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "createInterstitial", interstitial_inventory );
				} ) );
		}
	}

	//======================================================================================

	private void ShowMobFoxInterstitial_iPhone ()
	{
		_showInterstitial ( );
	}

	private void ShowMobFoxInterstitial_Android ()
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "showInterstitial" );
				} ) );
		}
	}

	public void ShowMobFoxInterstitial ()
	{
		if (InterstitialReady)
		{
			ConnectToPlugin ( );

			if (Application.platform == RuntimePlatform.Android)
			{
				ShowMobFoxInterstitial_Android ( );
			}
			else
			{
				ShowMobFoxInterstitial_iPhone ( );
			}
		}
		else
		{
			//LogMessage ( "## MobFoxInterstitial - not ready! ##" );
		}
	}

	//======================================================================================

	public void setInterstitialFloorPrice (float floorPrice)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setInterstitialFloorPrice_Android ( floorPrice );
		}
		else
		{
			setInterstitialFloorPrice_iPhone ( floorPrice );
		}
	}

	//-------------------------------------------

	private void setInterstitialFloorPrice_iPhone (float floorPrice)
	{
		_setInterstitialFloorPrice(floorPrice);
	}

	//-------------------------------------------

	private void setInterstitialFloorPrice_Android (float floorPrice)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setInterstitialFloorPrice", floorPrice );
			}));
		}
	}

	//======================================================================================

	public void ReleaseMobFoxInterstitial ()
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			ReleaseMobFoxInterstitial_Android ( );
		}
		else
		{
			ReleaseMobFoxInterstitial_iPhone ( );
		}
	}

	//-------------------------------------------

	private void ReleaseMobFoxInterstitial_iPhone ()
	{
		_releaseInterstitial ( );
	}

	//-------------------------------------------

	private void ReleaseMobFoxInterstitial_Android ()
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "releaseInterstitial" );
				} ) );
		}
	}

	//======================================================================================

	public void interstitialReady (string msg)
	{
		InterstitialReady = true;
		if (OnInterstitialReady != null)
		{
			OnInterstitialReady ( );
		}
	}

	public void interstitialShown (string msg)
	{
		if (OnInterstitialShown != null)
		{
			OnInterstitialShown ( );
		}
	}

	public void interstitalError (string msg)
	{
		if (OnInterstitialError != null)
		{
			OnInterstitialError ( msg );
		}
	}

	public void interstitialClosed (string msg)
	{
		if (OnInterstitialClosed != null)
		{
			OnInterstitialClosed ( );
		}
	}

	public void interstitialClicked (string msg)
	{
		if (OnInterstitialClicked != null)
		{
			OnInterstitialClicked (  msg );
		}
	}

	public void interstitialFinished (string msg)
	{
		if (OnInterstitialFinished != null)
		{
			OnInterstitialFinished ( );
		}
	}

	//======================================================================================
	//======  N A T I V E                                                             ======
	//======================================================================================

	public void RequestMobFoxNative (string native_inventory)
	{
		ConnectToPlugin ( );
		
		if (Application.platform == RuntimePlatform.Android)
		{
			CreateMobFoxNative_Android ( native_inventory );
		}
		else
		{
			CreateMobFoxNative_iPhone ( native_inventory );
		}
	}

	//-------------------------------------------

	private void CreateMobFoxNative_iPhone (string native_inventory)
	{
		_createNative ( native_inventory );
	}

	//-------------------------------------------

	private void CreateMobFoxNative_Android (string native_inventory)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "createNative", native_inventory );
				} ) );
		}
	}

	//======================================================================================

	public void setNativeFloorPrice (float floorPrice)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setNativeFloorPrice_Android ( floorPrice );
		}
		else
		{
			setNativeFloorPrice_iPhone ( floorPrice );
		}
	}

	//-------------------------------------------

	private void setNativeFloorPrice_iPhone (float floorPrice)
	{
		_setNativeFloorPrice(floorPrice);
	}

	//-------------------------------------------

	private void setNativeFloorPrice_Android (float floorPrice)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setNativeFloorPrice", floorPrice );
			}));
		}
	}

	//======================================================================================

	public void callToActionClicked ()
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			callToActionClicked_Android (  );
		}
		else
		{
			callToActionClicked_iPhone (  );
		}
	}

	//-------------------------------------------

	private void callToActionClicked_iPhone ()
	{
		_callToActionClicked();
	}

	//-------------------------------------------

	private void callToActionClicked_Android ()
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "callToActionClicked" );
			}));
		}
	}

	//======================================================================================

	public void setNativeAdContext( NativeAdContext adContext)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setNativeAdContext_Android ( adContext );
		}
		else
		{
			setNativeAdContext_iPhone ( adContext );
		}
	}

	//-------------------------------------------

	private void setNativeAdContext_iPhone (NativeAdContext adContext)
	{
		if (adContext == NativeAdContext.CONTENT)
		{
			_setNativeAdContext("content");
		}
		if (adContext == NativeAdContext.SOCIAL)
		{
			_setNativeAdContext("social");
		}
		if (adContext == NativeAdContext.PRODUCT)
		{
			_setNativeAdContext("product");
		}
	}

	//-------------------------------------------

	private void setNativeAdContext_Android (NativeAdContext adContext)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setNativeAdContext", adContext );
			}));
		}
	}

	//======================================================================================

	public void setNativeAdPlacementType( NativeAdPlacementType adType)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setNativeAdPlacementType_Android ( adType );
		}
		else
		{
			setNativeAdPlacementType_iPhone ( adType );
		}
	}

	//-------------------------------------------

	private void setNativeAdPlacementType_iPhone (NativeAdPlacementType adType)
	{
		if (adType == NativeAdPlacementType.IN_FEED)
		{
			_setNativeAdPlacementType("in_feed");
		}
		if (adType == NativeAdPlacementType.ATOMIC)
		{
			_setNativeAdPlacementType("atomic");
		}
		if (adType == NativeAdPlacementType.OUTSIDE)
		{
			_setNativeAdPlacementType("outside");
		}
		if (adType == NativeAdPlacementType.RECOMMENDATION)
		{
			_setNativeAdPlacementType("recommendation");
		}
	}

	//-------------------------------------------

	private void setNativeAdPlacementType_Android (NativeAdPlacementType adType)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setNativeAdPlacementType", adType );
			}));
		}
	}

	//======================================================================================

	public void setNativeAdIconImage( bool bRequired, int size )
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setNativeAdIconImage_Android ( bRequired, size );
		}
		else
		{
			setNativeAdIconImage_iPhone ( bRequired, size );
		}
	}

	//-------------------------------------------

	private void setNativeAdIconImage_iPhone ( bool bRequired, int size )
	{
		_setNativeAdIconImage( bRequired, size );
	}

	//-------------------------------------------

	private void setNativeAdIconImage_Android ( bool bRequired, int size )
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setNativeAdIconImage", bRequired, size );
			}));
		}
	}

	//======================================================================================

	public void setNativeAdMainImage( bool bRequired, int width, int height )
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setNativeAdMainImage_Android ( bRequired, width, height );
		}
		else
		{
			setNativeAdMainImage_iPhone ( bRequired, width, height );
		}
	}

	//-------------------------------------------

	private void setNativeAdMainImage_iPhone ( bool bRequired, int width, int height )
	{
		_setNativeAdMainImage( bRequired, width, height );
	}

	//-------------------------------------------

	private void setNativeAdMainImage_Android ( bool bRequired, int width, int height )
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setNativeAdMainImage", bRequired, width, height );
			}));
		}
	}

	//======================================================================================

	public void setNativeAdTitle( bool bRequired, int maxLength )
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setNativeAdTitle_Android ( bRequired, maxLength );
		}
		else
		{
			setNativeAdTitle_iPhone ( bRequired, maxLength );
		}
	}

	//-------------------------------------------

	private void setNativeAdTitle_iPhone ( bool bRequired, int maxLength )
	{
		_setNativeAdTitle( bRequired, maxLength );
	}

	//-------------------------------------------

	private void setNativeAdTitle_Android ( bool bRequired, int maxLength )
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setNativeAdTitle", bRequired, maxLength );
			}));
		}
	}

	//======================================================================================

	public void setNativeAdDesc( bool bRequired, int maxLength )
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setNativeAdDesc_Android ( bRequired, maxLength );
		}
		else
		{
			setNativeAdDesc_iPhone ( bRequired, maxLength );
		}
	}

	//-------------------------------------------

	private void setNativeAdDesc_iPhone ( bool bRequired, int maxLength )
	{
		_setNativeAdDesc( bRequired, maxLength );
	}

	//-------------------------------------------

	private void setNativeAdDesc_Android ( bool bRequired, int maxLength )
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setNativeAdDesc", bRequired, maxLength );
			}));
		}
	}
	
	//======================================================================================

	public void ReleaseMobFoxNative ()
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			ReleaseMobFoxNative_Android ( );
		}
		else
		{
			ReleaseMobFoxNative_iPhone ( );
		}
	}

	//-------------------------------------------

	private void ReleaseMobFoxNative_iPhone ()
	{
		_releaseNative ( );
	}

	//-------------------------------------------

	private void ReleaseMobFoxNative_Android ()
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
				{
					PluginInstance.Call ( "releaseNative" );
				} ) );
		}
	}

	//======================================================================================

	/*
	public void registerNativeForInteraction (string nativeGameObjectId)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			registerNativeForInteraction_Android ( nativeGameObjectId );
		}
		else
		{
			registerNativeForInteraction_iPhone ( nativeGameObjectId );
		}
	}

	//-------------------------------------------

	private void registerNativeForInteraction_iPhone (string nativeGameObjectId)
	{
	}

	//-------------------------------------------

	private void registerNativeForInteraction_Android (string nativeGameObjectId)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "registerNativeForInteraction", nativeGameObjectId );
			}));
		}
	}
	*/
	//======================================================================================

	public void nativeError (string msg)
	{
		if (OnNativeError != null)
		{
			OnNativeError ( msg );
		}
	}

	public void nativeReady (string msg)
	{
		if (OnNativeReady != null)
		{
			OnNativeReady ( msg );
		}
	}

	public void nativeClicked (string msg)
	{
		if (OnNativeClicked != null)
		{
			OnNativeClicked ( );
		}
	}

	//======================================================================================

	public class NativeInfo
	{
	    public string title;
    	public string desc;
	    public string rating;
	    public string sponsored;
	    public string ctatext;
	    public string iconImageUrl;
	    public string mainImageUrl;
	    public string clickUrl;
	}
	
	public enum NativeAdContext {CONTENT, SOCIAL, PRODUCT};
	
	public enum NativeAdPlacementType {IN_FEED, ATOMIC, OUTSIDE, RECOMMENDATION};	
}