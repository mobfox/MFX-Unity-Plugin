# Mobfox and Unity
Thanks for electing to show Mobfox ads in your Unity app.

<!-- toc -->

* [What's new](#whats-new)
* [Prerequisites](#prerequisites)
* [Use cases - what plugin do I use?](#use-cases---what-plugin-do-i-use)
* [Support](#support)

<!-- toc stop -->

# What's new

**Version 4.2.1**

* New Mobfox native SDK and adapters (Android 4.2.1, iOS 4.2.2).
* Native ads under AdMob.
* Fixes: watermark location, banner cleanup, callbacks inside interstitial/rewarded.
* Added functions for querying version of Unity plugin and/or Mobfox native SDK.

# Prerequisites

* Mobfox Unity Plugins works on **Android** devices with OS version **4.4.x (19)** and up.
* Mobfox Unity Plugins requires an internet access.
* You will need a [Mobfox](https://mobfox.atlassian.net/wiki/spaces/PUMD/pages/354549848/Setup+MobFox+Account) account.
* In order to build **64bit on Android**, you need to use Unity editor version **2019.3.0** and up.
* In case you have build errors, make sure you have **Unity ads** updated to version **3.4.4** - open "**Window => Package Manager**" in Unity, select "**Ads**", and update to **3.4.4**.

# Use cases - what plugin do I use?

There are 3 use cases for using Mobfox in Unity:

* If you are trying to show **Mobfox** ads directly - use [the MFX-Unity-Plugin](MFX-Unity-Package/README_mobfox.md).
* If you are already showing **MoPub** ads, and want to add mediation to Mobfox - use the [MFX-Unity-MoPub-Plugin](MFX-Unity-MoPub-Package/README_mopub.md).
* If you are already showing **AdMob** ads, and want to add mediation to Mobfox - use the [MFX-Unity-AdMob-Plugin](MFX-Unity-AdMob-Package/README_admob.md).
 
# Support

For any problems or questions not covered by the instructions below, contact <sdk_support@mobfox.com>.

If you want to report bugs or technical issues with this SDK please do it on [Github](https://github.com/mobfox/MFX-Unity-Plugin/issues).
