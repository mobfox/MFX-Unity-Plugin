# MFX-Unity-Plugin
Thanks for selecting Unity plugin for Mobfox SDK.

<!-- toc -->

* [Prerequisites](#prerequisites)
* [Installation](#installation)
* [Usage](#usage)

<!-- toc stop -->

# Prerequisites

* BAZZ-Bridge SDK works on **Android** devices with OS version **4.0.1** and up.
* BAZZ-Bridge SDK requires a connected android device (i.e. **data** or **WiFi** connection) to work.
* You will need a developer account with BAZZ. Please [Contact Us](mailto:support@bazz.co) for details.

# Installation

Clone or download BAZZ-Bridge-SDK from here, and extract it on your computer.
In it you will find a demo application for how the BAZZ-Bridge SDK can be used.

You can import this project to Android Studio, and use it as a reference
or base to make your alterations.

If you already have an existing project, or want to create a new one,
The BAZZ-Bridge SDK comes in the form of **bazz_bridge_sdk.aar**. You should receive it from us, along with
the App ID.

You can also download it from here: https://dl.dropboxusercontent.com/s/vijxkoh6xf9w2r7/bazz_bridge_sdk.aar

To add it to your project, follow the steps below:

1. With your project open in Android Studio, select '**File => New => New Module**'
2. In the dialog opened, select '**Import .JAR/.AAR Package**', and click '**Next**'
3. In the '**File name**' line - browse to the '**bazz_bridge_sdk**' folder of the demo project, or to where you saved the file from the link above, and select the **aar** file. Click on '**Finish**'
4. After Gradle Sync is done, go to your project settings: right-click on the project module, and select '**Open Module Settings**'.
5. Select your **app** module in the left pane, and click the '**Dependencies**' tab. Click the **plus (+)** sign, and select '**Module dependency**'. Select '**:bazz_bridge_sdk**', and click '**OK**' twice.

Done. Now you can start integrating the BAZZ-Bridge SDK as explained in the next sections.

# Usage

