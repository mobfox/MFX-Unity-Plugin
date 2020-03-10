//
//  MFMediatedNativeContentAd.h
//  Adapters
//
//  Created by Shimi Sheetrit on 1/1/17.
//  Copyright © 2017 Matomy Media Group Ltd. All rights reserved.
//


#import <GoogleMobileAds/GoogleMobileAds.h>
#import <MFXSDKCore/MFXSDKCore.h>

@interface MFMediatedNativeContentAd : NSObject <GADMediatedUnifiedNativeAd, GADMediatedNativeAdDelegate>

- (instancetype _Nullable )initWithSampleNativeAd:(nullable MFXNativeAd *)mobfoxNativeAd
                 nativeAdViewAdOptions:(nullable GADNativeAdViewAdOptions *)nativeAdViewAdOptions;

@end
