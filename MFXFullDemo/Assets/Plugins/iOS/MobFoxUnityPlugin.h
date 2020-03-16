//
//  MobFoxUnityPlugin.h
//  MobFoxUnityPlugin
//
//  Created by Itamar Nabriski on 11/17/15.
//  Copyright © 2015 Itamar Nabriski. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <MFXSDKCore/MFXSDKCore.h>

@interface MobFoxUnityPlugin : NSObject<MFXBannerAdDelegate,MFXInterstitialAdDelegate,MFXNativeAdDelegate>


-(void) showMessage:(NSString*)message;


-(void) setDemoAge:(NSString*)val;
-(void) setDemoGender:(NSString*)val;
-(void) setDemoKeywords:(NSString*)val;
-(void) setLatitude:(CGFloat)val;
-(void) setLongitude:(CGFloat)val;

-(void) setCOPPA:(BOOL)val;




-(int) createBanner:(NSString*)invh 
			originX:(CGFloat)originX 
			originY:(CGFloat)originY
          sizeWidth:(CGFloat)sizeWidth
         sizeHeight:(CGFloat)sizeHeight;

-(void) hideBanner;
-(void) showBanner;
-(void) setBannerRefresh:(int)val;
-(void) setBannerFloorPrice:(CGFloat)val;
-(void) releaseBanner;





-(void) createInterstitial:(NSString*)invh;
-(void) showInterstitial;
-(void) setInterstitialFloorPrice:(CGFloat)val;
-(void) releaseInterstitial;



-(void) createNative:(NSString*)invh;
-(void) setNativeFloorPrice:(CGFloat)val;
-(void) callToActionClicked;
-(void) setNativeAdContext:(NSString*)val;
-(void) setNativeAdPlacementType:(NSString*)val;
-(void) setNativeAdIconImage:(BOOL)bRequired withSize:(int)size;
-(void) setNativeAdMainImage:(BOOL)bRequired withWidth:(int)width andHeight:(int)height;
-(void) setNativeAdTitle:(BOOL)bRequired withMaxLen:(int)maxLen;
-(void) setNativeAdDesc:(BOOL)bRequired withMaxLen:(int)maxLen;
-(void) releaseNative;

@end
