# Mobfox Under AdMob in Unity
Thanks for electing to show Mobfox ads in your Unity app.

<!-- toc -->

* [Prerequisites](#prerequisites)
* [Other use cases - Mobfox or MoPub](#other-use-cases---mobfox-or-mopub)
* [Installation](#installation)
* [Important note - iOS](#important-note---ios)
* [Support](#support)

<!-- toc stop -->

# Prerequisites

* Mobfox Unity Plugins works on **Android** devices with OS version **4.4.x (19)** and up.
* Mobfox Unity Plugins requires an internet access.
* You will need a [Mobfox](https://mobfox.atlassian.net/wiki/spaces/PUMD/pages/354549848/Setup+MobFox+Account) account.
* In order to build **64bit on Android**, you need to use Unity editor version **2019.3.0** and up.
* In case you have build errors, make sure you have **Unity ads** updated to version **3.4.4** - open "**Window => Package Manager**" in Unity, select "**Ads**", and update to **3.4.4**.

# Other use cases - Mobfox or MoPub?

This document describes the use of **Mobfox** under **AdMob** mediation in Unity.

* If you are trying to show **Mobfox** ads directly - use [the MFX-Unity-Plugin](../MFX-Unity-Package/README_mobfox.md).
* If you are already showing **MoPub** ads, and want to add mediation to Mobfox - use the [MFX-Unity-MoPub-Plugin](../MFX-Unity-MoPub-Package/README_mopub.md).

# Installation

Clone or download **MFX-Unity-Plugin** from [here](https://github.com/mobfox/MFX-Unity-Plugin), and extract it on your computer.

In it you will find a demo application (under the **MFXFullDemo** directory) for how the Mobfox Unity Plugin and/or adapters (AdMob & MoPub) can be used. You can use this as a basis, and build your app on it.

If you already have an existing project, or want to create a new one,
first integrate **AdMob** SDK following the instructions at [AdMob website](https://developers.google.com/admob/unity/start).

The Mobfox Unity Plugin for AdMob comes in the form of **mfx-unity-admob.unitypackage** under the 'MFX-Unity-AdMob-Package' directory.

To add it to your project, follow the steps below:

1. With your project open in Unity Editor, select '**Assets => Import Package => Custom Package...**'
2. In the dialog opened, select '**mfx-unity-admob.unitypackage**', and click '**Open**'

Done. Now you should be able to see Mobfox ads mediated via your AdMob account.

# Important note - iOS

After building the iOS version - remember you need to first:

* Open the iOS project in XCode.
* In "**Build Settings**", set "**Enable Modules**" to **YES** for **all** modules.

# Support

For any problems or questions not covered by the instructions below, contact <sdk_support@mobfox.com>.

If you want to report bugs or technical issues with this SDK please do it on [Github](https://github.com/mobfox/MFX-Unity-Plugin/issues).
