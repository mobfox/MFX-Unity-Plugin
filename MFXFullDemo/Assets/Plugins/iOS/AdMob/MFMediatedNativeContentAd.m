//
//  MFMediatedNativeContentAd.m
//  Adapters
//
//  Created by Shimi Sheetrit on 1/1/17.
//  Copyright © 2017 Matomy Media Group Ltd. All rights reserved.
//

#import "MFMediatedNativeContentAd.h"

@interface MFMediatedNativeContentAd()

@property (nonatomic) NSString *my_headline;
@property (nonatomic) NSString *my_body;
@property (nonatomic) NSString *my_callToAction;
@property (nonatomic) NSString *my_store;
@property (nonatomic) NSString *my_price;
@property (nonatomic) NSString *my_advertiser;
@property (nonatomic) NSDictionary *extras;
@property (nonatomic) NSDecimalNumber *nsRating;


@property(nonatomic, strong) MFXNativeAd *mobfoxAd;
@property (nonatomic) NSArray *mappedImages;
@property (nonatomic) GADNativeAdImage *mappedIcon;

@property(nonatomic, strong) GADNativeAdViewAdOptions *nativeAdViewAdOptions;

@end


@implementation MFMediatedNativeContentAd

- (instancetype _Nullable )initWithSampleNativeAd:(nullable MFXNativeAd *)mobfoxNativeAd
                            nativeAdViewAdOptions:(nullable GADNativeAdViewAdOptions *)nativeAdViewAdOptions
{
        if (mobfoxNativeAd == nil) {
            return nil;
        }
    

        self = [super init];
        if (self) {
            
            _mobfoxAd = mobfoxNativeAd;
            
            NSDictionary* dictTexts = [MobFoxSDK getNativeAdTexts:_mobfoxAd];
            
            _my_headline     = [NSString stringWithString:[dictTexts objectForKey:@"title"]];
            _my_body         = [dictTexts objectForKey:@"desc"];
            _nsRating        = [NSDecimalNumber decimalNumberWithString:[dictTexts objectForKey:@"rating"]];
            _my_advertiser   = [dictTexts objectForKey:@"sponsored"];
            _my_callToAction = [dictTexts objectForKey:@"ctatext"];

            NSString*     iconUrl = nil;
            NSString*     mainUrl = nil;
            NSDictionary* dictUrls = [MobFoxSDK getNativeAdImageUrls:_mobfoxAd];
            if (dictUrls != nil)
            {
                iconUrl  = [dictUrls objectForKey:@"icon"];
                mainUrl  = [dictUrls objectForKey:@"main"];
            }

            UIImage* iconImg = nil;
            UIImage* mainImg = nil;
            NSDictionary* dictImages = [MobFoxSDK getNativeAdImages:_mobfoxAd];
            if (dictImages != nil)
            {
                iconImg = [dictImages objectForKey:@"icon"];
                mainImg = [dictImages objectForKey:@"main"];
            }

            if (iconImg!=nil)
            {
                _mappedIcon = [[GADNativeAdImage alloc] initWithImage:iconImg];
            } else {
                if (iconUrl != nil) {
                    _mappedIcon = [[GADNativeAdImage alloc] initWithURL:[NSURL URLWithString:iconUrl] scale:0];
                }
            }

            if (mainImg!=nil)
            {
                _mappedImages = @[ [[GADNativeAdImage alloc] initWithImage:mainImg]];
            } else {
                if (mainUrl != nil) {
                    _mappedImages = @[ [[GADNativeAdImage alloc] initWithURL:[NSURL URLWithString:mainUrl] scale:0] ];
                }
            }

            //[MobFoxSDK registerNativeAdForInteraction:native onView:_viewNative];

            
           
        }
        return self;
}

- (GADNativeAdImage *)icon {
    return self.mappedIcon;
}

- (NSArray *)images {
    return self.mappedImages;
}
    
- (NSString *)headline {
    return self.my_headline;
}
    
- (NSString *)body {
    return self.my_body;
}
        
- (NSString *)callToAction {
    return self.my_callToAction;
}
    
- (NSDecimalNumber *)starRating {
    return self.nsRating;
}
    
- (NSString *)store {
    return self.my_store;
}
    
- (NSString *)advertiser {
    return self.my_advertiser;
}
    
- (NSString *)price {
    return self.my_price;
}
    
- (NSDictionary *)extraAssets {
    return self.extras;
}
    
#pragma mark - GADMediatedNativeAdDelegate implementation
    
- (void)mediatedNativeAdDidRecordImpression:(id<GADMediatedNativeAd>)mediatedNativeAd {
    NSLog(@"mediatedNativeAdDidRecordImpression:");
  
}
    
- (void)mediatedNativeAd:(id<GADMediatedNativeAd>)mediatedNativeAd
didRecordClickOnAssetWithName:(NSString *)assetName
                    view:(UIView *)view
          viewController:(UIViewController *)viewController {
    NSLog(@"mediatedNativeAd:didRecordClickOnAssetWithName:view:viewController:");
    

}

@end
 
