# Mobfox Under AdMob in Unity
Thanks for electing to show Mobfox ads in your Unity app.

<!-- toc -->

* [Prerequisites](#prerequisites)
* [Other use cases - Mobfox or MoPub](#other-use-cases---mobfox-or-mopub)
* [Installation](#installation)
* [Support](#support)

<!-- toc stop -->

# Prerequisites

* Mobfox Unity Plugins works on **Android** devices with OS version **4.4.x (19)** and up.
* Mobfox Unity Plugins requires an internet access.
* You will need a [Mobfox](https://mobfox.atlassian.net/wiki/spaces/PUMD/pages/354549848/Setup+MobFox+Account) account.

# Other use cases - Mobfox or MoPub?

This document describes the use of **Mobfox** under **AdMob** mediation in Unity.

* If you are trying to show **Mobfox** ads directly - use [the MFX-Unity-Plugin](../MFX-Unity-Package/README_mobfox.md).
* If you are already showing **MoPub** ads, and want to add mediation to Mobfox - use the [MFX-Unity-MoPub-Plugin](../MFX-Unity-MoPub-Package/README_mopub.md).

# Installation

Clone or download **MFX-Unity-Plugin** from [here](https://github.com/mobfox/MFX-Unity-Plugin), and extract it on your computer.

In it you will find a demo application (under the **MFXWithAdMob** directory) for displaying **AdMob** banner, interstitial, and rewarded ads.

If you already have an existing project, or want to create a new one,
first integrate **AdMob** SDK following the instructions at [AdMob website](https://developers.google.com/admob/unity/start).

The Mobfox Unity Plugin for AdMob comes in the form of **mfx-unity-admob.unitypackage** under the 'MFX-Unity-AdMob-Package' directory.

To add it to your project, follow the steps below:

1. With your project open in Unity Editor, select '**Assets => Import Package => Custom Package...**'
2. In the dialog opened, select '**mfx-unity-admob.unitypackage**', and click '**Open**'

Done. Now you should be able to see Mobfox ads mediated via your AdMob account.

# Support

For any problems or questions not covered by the instructions below, contact <sdk_support@mobfox.com>.

If you want to report bugs or technical issues with this SDK please do it on [Github](https://github.com/mobfox/MFX-Unity-Plugin/issues).
