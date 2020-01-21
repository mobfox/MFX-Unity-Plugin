#ifndef MoPubAdapterMobFox_h
#define MoPubAdapterMobFox_h

#import <MFXSDKCore/MFXSDKCore.h>
#if __has_include(<MoPub/MoPub.h>)
#import <MoPub/MoPub.h>
#elif __has_include(<MoPubSDKFramework/MoPub.h>)
#import <MoPubSDKFramework/MoPub.h>
#else
#import "MPBannerCustomEvent.h"
#import "MoPub.h"
#endif

@interface MoPubAdapterMobFox : MPBannerCustomEvent <MFXBannerAdDelegate>

- (void)requestAdWithSize:(CGSize)size customEventInfo:(NSDictionary *)info;

// Added by Shimon for Unity MoPub plugin
- (void)requestAdWithSize:(CGSize)size customEventInfo:(NSDictionary *)info adMarkup:(NSString *)adMarkup;

//- (BOOL)enableAutomaticImpressionAndClickTracking;

@end

#endif
