using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class MobFox :MonoBehaviour
{
#if UNITY_IOS
	[DllImport ( "__Internal" )]
	public static extern void _setGameObject (string gameObject);
	

	[DllImport ( "__Internal" )]
	public static extern void _showMessage (string message);


	[DllImport ( "__Internal" )]
	public static extern void _setCOPPA (bool subjectToCOPPA);

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
#endif

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
#if UNITY_IOS
			_setGameObject ( MobFoxGameObjectName );
#endif
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
#if UNITY_IOS
			_showMessage ( message );
#endif
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
	
	public string getUnityPluginVersion()
	{
		return "4.1.6.1";
	}
	
	//======================================================================================
	//======  iOS functions                                                           ======
	//======================================================================================

#if UNITY_IOS

	private void setCOPPA_iPhone (bool subjectToCOPPA)
	{
		_setCOPPA(subjectToCOPPA);
	}

	//-------------------------------------------

	private void setDemoAge_iPhone (string demoAge)
	{
		_setDemoAge (demoAge);
	}

	//-------------------------------------------

	private void setDemoGender_iPhone (string demoGender)
	{
		_setDemoGender (demoGender);
	}

	//-------------------------------------------

	private void setDemoKeywords_iPhone (string demoKeywords)
	{
		_setDemoKeywords (demoKeywords);
	}

	//-------------------------------------------

	private void setLatitude_iPhone (double latitude)
	{
		_setLatitude (latitude);
	}

	//-------------------------------------------

	private void setLongitude_iPhone (double longitude)
	{
		_setLongitude (longitude);
	}
	
	//-------------------------------------------

	private void ShowMobFoxBanner_iPhone (string banner_inventory, int left, int top, int width, int height)
	{
		_createBanner ( banner_inventory, left, top, width, height );
	}

	//-------------------------------------------

	private void setBannerRefresh_iPhone (int intervalInSeconds)
	{
		_setBannerRefresh(intervalInSeconds);
	}
	
	//-------------------------------------------

	private void setBannerFloorPrice_iPhone (float floorPrice)
	{
		_setBannerFloorPrice(floorPrice);
	}

	//-------------------------------------------

	private void HideMobFoxBanner_iPhone ()
	{
		_hideBanner ( );
	}

	//-------------------------------------------

	private void UnhideMobFoxBanner_iPhone ()
	{
		_showBanner ( );
	}

	//-------------------------------------------

	private void ReleaseMobFoxBanner_iPhone ()
	{
		_releaseBanner ( );
	}

	//-------------------------------------------

	private void CreateMobFoxInterstitial_iPhone (string interstitial_inventory)
	{
		_createInterstitial ( interstitial_inventory );
	}

	//-------------------------------------------

	private void ShowMobFoxInterstitial_iPhone ()
	{
		_showInterstitial ( );
	}

	//-------------------------------------------

	private void setInterstitialFloorPrice_iPhone (float floorPrice)
	{
		_setInterstitialFloorPrice(floorPrice);
	}

	//-------------------------------------------

	private void ReleaseMobFoxInterstitial_iPhone ()
	{
		_releaseInterstitial ( );
	}

	//-------------------------------------------

	private void CreateMobFoxNative_iPhone (string native_inventory)
	{
		_createNative ( native_inventory );
	}

	//-------------------------------------------

	private void setNativeFloorPrice_iPhone (float floorPrice)
	{
		_setNativeFloorPrice(floorPrice);
	}

	//-------------------------------------------

	private void callToActionClicked_iPhone ()
	{
		_callToActionClicked();
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

	private void setNativeAdIconImage_iPhone ( bool bRequired, int size )
	{
		_setNativeAdIconImage( bRequired, size );
	}

	//-------------------------------------------

	private void setNativeAdMainImage_iPhone ( bool bRequired, int width, int height )
	{
		_setNativeAdMainImage( bRequired, width, height );
	}

	//-------------------------------------------

	private void setNativeAdTitle_iPhone ( bool bRequired, int maxLength )
	{
		_setNativeAdTitle( bRequired, maxLength );
	}

	//-------------------------------------------

	private void setNativeAdDesc_iPhone ( bool bRequired, int maxLength )
	{
		_setNativeAdDesc( bRequired, maxLength );
	}

	//-------------------------------------------

	private void ReleaseMobFoxNative_iPhone ()
	{
		_releaseNative ( );
	}

#endif

	//======================================================================================
	//======  Android functions                                                       ======
	//======================================================================================

	private void setCOPPA_Android (bool subjectToCOPPA)
	{
		if (activityContext != null)
		{
			activityContext.Call ( "runOnUiThread", new AndroidJavaRunnable ( () =>
			{
				PluginInstance.Call ( "setCOPPA", subjectToCOPPA );
			}));
		}
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

	//-------------------------------------------

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

	//-------------------------------------------

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
	//======  G L O B A L                                                             ======
	//======================================================================================

	public void setCOPPA (bool subjectToCOPPA)
	{
		ConnectToPlugin ( );

		if (Application.platform == RuntimePlatform.Android)
		{
			setCOPPA_Android ( subjectToCOPPA );
		}
		else
		{
#if UNITY_IOS
			setCOPPA_iPhone ( subjectToCOPPA );
#endif
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
#if UNITY_IOS
			setDemoAge_iPhone ( demoAge );
#endif
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
#if UNITY_IOS
			setDemoGender_iPhone ( demoGender );
#endif
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
#if UNITY_IOS
			setDemoKeywords_iPhone ( demoKeywords );
#endif
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
#if UNITY_IOS
			setLatitude_iPhone ( latitude );
#endif
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
#if UNITY_IOS
			setLongitude_iPhone ( longitude );
#endif
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
#if UNITY_IOS
			ShowMobFoxBanner_iPhone ( banner_inventory, left, top, width, height );
#endif
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
#if UNITY_IOS
			setBannerRefresh_iPhone ( intervalInSeconds );
#endif
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
#if UNITY_IOS
			setBannerFloorPrice_iPhone ( floorPrice );
#endif
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
#if UNITY_IOS
			HideMobFoxBanner_iPhone ( );
#endif
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
#if UNITY_IOS
			UnhideMobFoxBanner_iPhone ( );
#endif
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
#if UNITY_IOS
			ReleaseMobFoxBanner_iPhone ( );
#endif
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
#if UNITY_IOS
			CreateMobFoxInterstitial_iPhone ( interstitial_inventory );
#endif
		}
	}

	//======================================================================================

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
#if UNITY_IOS
				ShowMobFoxInterstitial_iPhone ( );
#endif
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
#if UNITY_IOS
			setInterstitialFloorPrice_iPhone ( floorPrice );
#endif
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
#if UNITY_IOS
			ReleaseMobFoxInterstitial_iPhone ( );
#endif
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
#if UNITY_IOS
			CreateMobFoxNative_iPhone ( native_inventory );
#endif
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
#if UNITY_IOS
			setNativeFloorPrice_iPhone ( floorPrice );
#endif
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
#if UNITY_IOS
			callToActionClicked_iPhone (  );
#endif
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
#if UNITY_IOS
			setNativeAdContext_iPhone ( adContext );
#endif
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
#if UNITY_IOS
			setNativeAdPlacementType_iPhone ( adType );
#endif
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
#if UNITY_IOS
			setNativeAdIconImage_iPhone ( bRequired, size );
#endif
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
#if UNITY_IOS
			setNativeAdMainImage_iPhone ( bRequired, width, height );
#endif
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
#if UNITY_IOS
			setNativeAdTitle_iPhone ( bRequired, maxLength );
#endif
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
#if UNITY_IOS
			setNativeAdDesc_iPhone ( bRequired, maxLength );
#endif
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
#if UNITY_IOS
			ReleaseMobFoxNative_iPhone ( );
#endif
		}
	}

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