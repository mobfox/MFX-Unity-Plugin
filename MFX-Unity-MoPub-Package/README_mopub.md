# Mobfox Under MoPub in Unity
Thanks for electing to show Mobfox ads in your Unity app.

<!-- toc -->

* [Prerequisites](#prerequisites)
* [Other use cases - Mobfox or AdMob](#other-use-cases---mobfox-or-admob)
* [Installation](#installation)
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

In it you will find a demo application (under the **MFXWithMoPub** directory) for displaying **MoPub** banner, interstitial, and rewarded ads.

If you already have an existing project, or want to create a new one,
first integrate **MoPub** SDK following the instructions at [MoPub website](https://developers.mopub.com/publishers/unity/integrate/).

The Mobfox Unity Plugin for MoPub comes in the form of **mfx-unity-mopub.unitypackage** under the 'MFX-Unity-MoPub-Package' directory.

To add it to your project, follow the steps below:

1. With your project open in Unity Editor, select '**Assets => Import Package => Custom Package...**'
2. In the dialog opened, select '**mfx-unity-mopub.unitypackage**', and click '**Open**'

Done. Now you should be able to see Mobfox ads mediated via your MoPub account.
 
# Support

For any problems or questions not covered by the instructions below, contact <sdk_support@mobfox.com>.

If you want to report bugs or technical issues with this SDK please do it on [Github](https://github.com/mobfox/MFX-Unity-Plugin/issues).
