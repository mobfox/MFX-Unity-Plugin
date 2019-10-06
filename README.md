# MFX-Unity-Plugin
Thanks for selecting Unity plugin for Mobfox Unity Plugin.

<!-- toc -->

* [Prerequisites](#prerequisites)
* [Installation](#installation)
* [Usage](#usage)
* [Support](#support)

<!-- toc stop -->

# Prerequisites

* Mobfox Unity Plugin works on **Android** devices with OS version **4.4.x (19)** and up.
* Mobfox Unity Plugin requires an internet access.
* You will need a [Mobfox](https://mobfox.atlassian.net/wiki/spaces/PUMD/pages/354549848/Setup+MobFox+Account) account.

# Installation

Clone or download **MFX-Unity-Plugin** from [here](https://github.com/mobfox/MFX-Unity-Plugin), and extract it on your computer.

In it you will find a demo application (under the **MFXDemo** directory) for how the Mobfox Unity Plugin can be used. You can use this as a basis, and build you app on it.

If you already have an existing project, or want to create a new one,
The Mobfox Unity Plugin comes in the form of **bazz_bridge_sdk.aar**. You should receive it from us, along with
the App ID.

You can also download it from here: https://dl.dropboxusercontent.com/s/vijxkoh6xf9w2r7/bazz_bridge_sdk.aar

To add it to your project, follow the steps below:

1. With your project open in Android Studio, select '**File => New => New Module**'
2. In the dialog opened, select '**Import .JAR/.AAR Package**', and click '**Next**'
3. In the '**File name**' line - browse to the '**bazz_bridge_sdk**' folder of the demo project, or to where you saved the file from the link above, and select the **aar** file. Click on '**Finish**'
4. After Gradle Sync is done, go to your project settings: right-click on the project module, and select '**Open Module Settings**'.
5. Select your **app** module in the left pane, and click the '**Dependencies**' tab. Click the **plus (+)** sign, and select '**Module dependency**'. Select '**:bazz_bridge_sdk**', and click '**OK**' twice.

Done. Now you can start integrating the Mobfox Unity Plugin as explained in the next sections.

# Usage


# Support

For any problems or questions not covered by the instructions below, contact <sdk_support@mobfox.com>.

If you want to report bugs or technical issues with this SDK please do it on [Github](https://github.com/mobfox/MFX-Unity-Plugin/issues).
