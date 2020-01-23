# Mobfox Under MoPub in Unity
Thanks for electing to show Mobfox ads in your Unity app.

<!-- toc -->

* [Prerequisites](#prerequisites)
* [Other use cases - Mobfox or AdMob](#other-use-cases---mobfox-or-admob)
* [Installation](#installation)
* [Usage](#usage)
  * [Initializing the SDK](#initializing-the-sdk)
  * [Configuring ads mediation](#configuring-ads-mediation)
  * [Setting up listeners](#setting-up-listeners)
  * [Banner ads](#banner-ads)
  * [Interstitial ads](#interstitial-ads)
  * [Native ads](#native-ads)
  * [COPPA compliance](#coppa-compliance)
* [Support](#support)

<!-- toc stop -->

# Prerequisites

* Mobfox Unity Plugins works on **Android** devices with OS version **4.4.x (19)** and up.
* Mobfox Unity Plugins requires an internet access.
* You will need a [Mobfox](https://mobfox.atlassian.net/wiki/spaces/PUMD/pages/354549848/Setup+MobFox+Account) account.

# Other use cases - Mobfox or AdMob?

This document describes the use of **Mobfox** under **MoPub** mediation in Unity.

* If you are trying to show **Mobfox** ads directly - use [the MFX-Unity-Plugin](../MFX-Unity-Package/README_mobfox.md).
* If you are already showing **AdMob** ads, and want to add mediation to Mobfox - use the [MFX-Unity-AdMob-Plugin](../MFX-Unity-AdMob-Package/README_admob.md).

# Installation

Clone or download **MFX-Unity-Plugin** from [here](https://github.com/mobfox/MFX-Unity-Plugin), and extract it on your computer.

In it you will find a demo application (under the **MFXDemo** directory) for how the Mobfox Unity Plugin can be used. You can use this as a basis, and build you app on it.

If you already have an existing project, or want to create a new one,
The Mobfox Unity Plugin comes in the form of **mfx-unity.unitypackage** under the 'MFX-Unity-Package' directory.

To add it to your project, follow the steps below:

1. With your project open in Unity Editor, select '**Assets => Import Package => Custom Package...**'
2. In the dialog opened, select '**mfx-unity.unitypackage**', and click '**Open**'

Done. Now you can start integrating the Mobfox Unity Plugin as explained in the next sections.

# Usage

You an look at the **MainSript.cs** sript in the MFXDemo project to see examples of the ode to use for the plugin.

## Initializing the SDK

On your main **MonoBehaviour** class 'start' function, call:

    	MobFox.CreateSingletone( );


## Configuring ads mediation

Next you can define some global settings to refine your ad tergeting - you can define age, gender, keywords, and location (all these are optional):

```java
    MobFox.Instance.setDemoAge("32");
    MobFox.Instance.setDemoGender("male");
    MobFox.Instance.setDemoKeywords("basketball,tennis");
    MobFox.Instance.setLatitude(32.455666);
    MobFox.Instance.setLongitude(32.455666);
```
 
## Setting up listeners

The SDK supports several callbacks for events related to the ads, you can define them as follows:

```java
    // set listeners:
    MobFox.OnBannerReady       += onBannerLoaded;
    MobFox.OnBannerError       += onBannerError;
        
    MobFox.OnInterstitialReady += onInterLoaded;
    MobFox.OnInterstitialError += onInterError;

    MobFox.OnNativeReady       += onNativeReady;
    MobFox.OnNativeError       += onNativeError;
```
 
The callbacks may look like this:

```java
    //-------------------------------------------------------------
    
    public void onBannerLoaded()
    {
        MobFox.Instance.ShowMobFoxBanner();
    }

    public void onBannerError(string msg)
    {
        MobFox.Instance.Log(msg);
    }

    //-------------------------------------------------------------
    
    public void onInterLoaded()
    {
    	MobFox.Instance.ShowMobFoxInterstitial();
    }
    
    public void onInterError( string msg)
    {
        MobFox.Instance.Log(msg);
    }

    //-------------------------------------------------------------
        
    public void onNativeError( string msg)
    {
        MobFox.Instance.Log(msg);
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
```

## Banner ads

To create a banner ad call:

```java
    // possible dimensions: 320x50 / 300x250
    MobFox.Instance.RequestMobFoxBanner( "<banner inventory hash code>", x, y, width, height );
```
You can configure refresh rate (0 means no refresh), and floor price:

```java
    MobFox.Instance.setBannerRefresh(10);
		
    MobFox.Instance.setBannerFloorPrice(0.05);
```
To hide or show the banner ad:

```java
    MobFox.Instance.HideMobFoxBanner();
    	
    MobFox.Instance.ShowMobFoxBanner();
```
And to deallocate (release) the ad:

```java
    MobFox.Instance.ReleaseMobFoxBanner();
```
 
## Interstitial ads

To create a interstitial ad call:

```java
    MobFox.Instance.RequestMobFoxInterstitial( "<interstitial inventory hash code>" );
```
You can floor price:

```java
    MobFox.Instance.setInterstitialFloorPrice(0.05);
```
To show the interstitial ad:

```java    	
    MobFox.Instance.ShowMobFoxInterstitial();
```
And to deallocate (release) the ad:

```java
    MobFox.Instance.ReleaseMobFoxInterstitial();
```
 
## Native ads

Before you load a native ad, you can configure type of ad and required fields, and the floor price:

```java		
    MobFox.Instance.setNativeAdContext      ( MobFox.NativeAdContext.CONTENT );
    MobFox.Instance.setNativeAdPlacementType( MobFox.NativeAdPlacementType.ATOMIC );

    MobFox.Instance.setNativeAdIconImage    ( true, size );
    MobFox.Instance.setNativeAdMainImage    ( true, width, height );
    MobFox.Instance.setNativeAdTitle        ( true, maxLength );
    MobFox.Instance.setNativeAdDesc         ( true, maxLength );

    MobFox.Instance.setNativeFloorPrice(0.05);
```
To create a native ad call:

```java
    MobFox.Instance.RequestMobFoxNative ( "<native inventory hash code>" );
```
When the SDK returns the ad creatives, you can display them using the following convenience methods:

```java
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
```
When the user clicks on the ad:

```java
    MobFox.Instance.callToActionClicked();
```
And to deallocate (release) the ad:

```java
    MobFox.Instance.ReleaseMobFoxNative();
```
 
## COPPA compliance

If your app is COPPA compliant, you can use the following call to inform our SDK:

```java
    MobFox.Instance.setCOPPA(true/false);
```
 
# Support

For any problems or questions not covered by the instructions below, contact <sdk_support@mobfox.com>.

If you want to report bugs or technical issues with this SDK please do it on [Github](https://github.com/mobfox/MFX-Unity-Plugin/issues).
