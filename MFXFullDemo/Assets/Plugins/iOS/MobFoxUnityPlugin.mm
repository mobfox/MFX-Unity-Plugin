//
//  MobFoxUnityPlugin.m
//  MobFoxUnityPlugin
//
//  Created by Itamar Nabriski on 11/17/15.
//  Copyright © 2015 Itamar Nabriski. All rights reserved.
//


#import "MobFoxUnityPlugin.h"

extern "C"
{
    extern UIViewController* UnityGetGLViewController();
    extern void UnitySendMessage(const char *, const char *, const char *);
}

@interface MobFoxUnityPlugin()

@property NSString* gameObject;

@property (nonatomic,strong) MFXBannerAd*       mBannerAd;
@property (nonatomic,strong) UIView*            mBannerView;
@property (nonatomic,strong) MFXInterstitialAd* mInterstitialAd;
@property (nonatomic,strong) MFXNativeAd*       mNativeAd;

@property NSMutableDictionary* mDictBannerAds;
@property int nextId;

@end

@implementation MobFoxUnityPlugin

//======================================================================================
//======  I N I T                                                                 ======
//======================================================================================

-(id) init{
    self = [super init];
    self.mDictBannerAds = [[NSMutableDictionary alloc] init];
    self.nextId = 1;
    self.gameObject = nil;
    
    [MobFoxSDK sharedInstance];
    
    return self;
}

//======================================================================================
//======  H E L P E R S                                                           ======
//======================================================================================

-(void) showMessage:(NSString*)message
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> log(%@)",message);
}

-(char*) getSDKVersion
{
	NSString* nsVer = [MobFoxSDK sdkVersion];
	char*     cVer  = (char*)[nsVer UTF8String];
    if (cVer == NULL) return NULL;

	char* res = (char*)malloc(strlen(cVer) + 1);
    strcpy(res, cVer);
	return res;
}

//======================================================================================
//======  G L O B A L                                                             ======
//======================================================================================

-(void) setDemoAge:(NSString*)val
{
    [MobFoxSDK setDemoAge:val];
}

-(void) setDemoGender:(NSString*)val
{
    [MobFoxSDK setDemoGender:val];
}

-(void) setDemoKeywords:(NSString*)val
{
    [MobFoxSDK setDemoKeywords:val];
}

-(void) setLatitude:(CGFloat)val
{
    [MobFoxSDK setLatitude:[NSNumber numberWithFloat:val]];
}

-(void) setLongitude:(CGFloat)val
{
    [MobFoxSDK setLongitude:[NSNumber numberWithFloat:val]];
}

-(void) setCOPPA:(BOOL)val
{
    [MobFoxSDK setCoppa:val];
}

//======================================================================================
//======  B A N N E R                                                             ======
//======================================================================================

-(int) createBanner:(NSString*)invh 
			originX:(CGFloat)originX 
			originY:(CGFloat)originY
          sizeWidth:(CGFloat)sizeWidth
         sizeHeight:(CGFloat)sizeHeight
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> createBanner(%@)",invh);
    
    NSLog(@"dbg: ### MobFoxUnityPlugin >> rect x: %f", originX);
    NSLog(@"dbg: ### MobFoxUnityPlugin >> rect y: %f", originY);
    NSLog(@"dbg: ### MobFoxUnityPlugin >> rect width: %f", sizeWidth);
    NSLog(@"dbg: ### MobFoxUnityPlugin >> rect height: %f", sizeHeight);
    
    self.mBannerView = [[UIView alloc] initWithFrame:CGRectMake(originX,originY,sizeWidth,sizeHeight)];
    
    self.mBannerAd = [MobFoxSDK createBanner:invh
                                    width:sizeWidth
                                   height:sizeHeight
                             withDelegate:self];
    
    [self.mBannerAd setBannerRefresh:0];

    NSString* key = [NSString stringWithFormat:@"key-%d",self.nextId];
    [self.mDictBannerAds setValue:self.mBannerAd forKey:key];
    int cur = self.nextId;
    self.nextId= self.nextId + 1;
    //banner.type = @"video";
    
    [MobFoxSDK loadBanner:self.mBannerAd];
    return cur;
}

-(void) hideBanner
{
	if (self.mBannerView!=nil)
    {
        self.mBannerView.hidden = TRUE;
    }
}

-(void) showBanner
{
	if (self.mBannerView!=nil)
    {
        self.mBannerView.hidden = FALSE;
    }
}

-(void) setBannerRefresh:(int)val
{
	if (self.mBannerAd!=nil)
	{
	    [MobFoxSDK setBannerRefresh:self.mBannerAd withSeconds:val];
	}
}

-(void) setBannerFloorPrice:(CGFloat)val
{
	if (self.mBannerAd!=nil)
	{
	    [MobFoxSDK setBannerFloorPrice:self.mBannerAd withValue:[NSNumber numberWithFloat:val]];
	}
}

-(void) releaseBanner
{
	if (self.mBannerAd!=nil)
	{
	    [MobFoxSDK releaseBanner:self.mBannerAd];
	}
}

//======================================================================================

// MobFox Banner Ad Delegate
- (void)bannerAdLoaded:(MFXBannerAd *)banner
{
    //show banner
    UIViewController* vc = UnityGetGLViewController();
    [vc.view addSubview:self.mBannerView];
    
    [MobFoxSDK addBanner:self.mBannerAd
                  toView:self.mBannerView
                  atRect:CGRectMake(0, 0,
                                    self.mBannerView.frame.size.width,
                                    self.mBannerView.frame.size.height)];

    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxAdDidLoad:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "bannerReady", "MobFoxAdDidLoad msg");
}

- (void)bannerAdLoadFailed:(MFXBannerAd *)banner withError:(NSString*)error
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxAdDidFailToReceiveAdWithError: %@",error);
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "bannerError", [error UTF8String]);
}

- (void)bannerAdShown:(MFXBannerAd *)banner
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxAdShown:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "bannerShown", "MobFoxAdShown msg");
}

- (void)bannerAdClicked:(MFXBannerAd *)banner
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxAdClicked:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "bannerClicked", "MobFoxAdClicked msg");
}

- (void)bannerAdFinished:(MFXBannerAd *)banner
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxAdFinished:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "bannerFinished", "MobFoxAdFinished msg");
}

- (void)bannerAdClosed:(MFXBannerAd *)banner
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxAdClosed:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "bannerClosed", "MobFoxAdClosed msg");
}

//======================================================================================
//======  I N T E R S T I T I A L                                                 ======
//======================================================================================

-(void) createInterstitial:(NSString*)invh{
    
    NSLog(@"dbg: ### MobFoxUnityPlugin >> createInterstitial(%@)",invh);
    
    self.mInterstitialAd = [MobFoxSDK createInterstitial:invh
                                   withRootViewContoller:nil
                                            withDelegate:self];
    
    [MobFoxSDK loadInterstitial:self.mInterstitialAd];
}

-(void) showInterstitial{
   
    NSLog(@"dbg: ### MobFoxUnityPlugin >> showInterstitial");

    if(!self.mInterstitialAd){
        NSLog(@"dbg: ### MobFoxUnityPlugin >> showInterstitial >> inter not init");
        return;
    }
    
    UIViewController* vc = UnityGetGLViewController();

    NSLog(@"dbg: ### MobFoxUnityPlugin >> showInterstitial >> inter ready - show");
    [MobFoxSDK showInterstitial:self.mInterstitialAd aboveViewController:vc];
}

-(void) setInterstitialFloorPrice:(CGFloat)val
{
	if (self.mInterstitialAd!=nil)
	{
	    [MobFoxSDK setInterstitialFloorPrice:self.mInterstitialAd
    	                           withValue:[NSNumber numberWithFloat:val]];
	}
}

-(void) releaseInterstitial
{
	if (self.mInterstitialAd!=nil)
	{
	    [MobFoxSDK releaseInterstitial:self.mInterstitialAd];
	}
}

//======================================================================================

// MobFox Interstitial Ad Delegate
- (void)interstitialAdLoaded:(MFXInterstitialAd *)interstitial
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxInterstitialAdDidLoad >> inter loaded");

    [MobFoxSDK showInterstitial:interstitial];
    
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "interstitialReady", "MobFoxInterstitialAdDidLoad msg");
}

- (void)interstitialAdLoadFailed:(MFXInterstitialAd *)interstitial withError:(NSString*)error
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxInterstitialAdDidFailToReceiveAdWithError >> %@",error);
    
    
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "interstitalError", [error UTF8String]);
}

- (void)interstitialAdShown:(MFXInterstitialAd *)interstitial
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxInterstitialAdShown:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "interstitialShown", "MobFoxInterstitialAdShown msg");
}

- (void)interstitialAdClicked:(MFXInterstitialAd *)interstitial withUrl:(NSString*)url
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxInterstitialAdClicked:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "interstitialClicked", "MobFoxInterstitialAdClicked msg");
}

- (void)interstitialAdFinished:(MFXInterstitialAd *)interstitial
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxInterstitialAdFinished:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "interstitialFinished", "MobFoxInterstitialAdFinished msg");
}

- (void)interstitialAdClosed:(MFXInterstitialAd *)interstitial
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxInterstitialAdClosed:");
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "interstitialClosed", "MobFoxInterstitialAdClosed msg");
}

//======================================================================================
//======  N A T I V E                                                             ======
//======================================================================================

-(void) createNative:(NSString*)invh{
    
    NSLog(@"dbg: ### MobFoxUnityPlugin >> createNative(%@)",invh);
    
    self.mNativeAd = [MobFoxSDK createNativeAd:invh withDelegate:self];
    
    [MobFoxSDK loadNativeAd:self.mNativeAd];
}

-(void) setNativeFloorPrice:(CGFloat)val
{
	if (self.mNativeAd==nil) return;

    [MobFoxSDK setNativeFloorPrice:self.mNativeAd
                         withValue:[NSNumber numberWithFloat:val]];
}

-(void) callToActionClicked
{
	if (self.mNativeAd==nil) return;

    [MobFoxSDK callToActionClicked:self.mNativeAd];
}

-(void) setNativeAdContext:(NSString*)val
{
	if (self.mNativeAd==nil) return;

    [MobFoxSDK setNativeAdContext:self.mNativeAd withContext:val];
}

-(void) setNativeAdPlacementType:(NSString*)val
{
	if (self.mNativeAd==nil) return;

    [MobFoxSDK setNativeAdPlacementType:self.mNativeAd withPlacementType:val];
}

-(void) setNativeAdIconImage:(BOOL)bRequired withSize:(int)size
{
	if (self.mNativeAd==nil) return;

    [MobFoxSDK setNativeAdIconImage:self.mNativeAd
                         isRequired:bRequired
                           withSize:size];
}

-(void) setNativeAdMainImage:(BOOL)bRequired withWidth:(int)width andHeight:(int)height
{
	if (self.mNativeAd==nil) return;

    [MobFoxSDK setNativeAdMainImage:self.mNativeAd
                         isRequired:bRequired
                           withSize:CGSizeMake(width, height)];
}

-(void) setNativeAdTitle:(BOOL)bRequired withMaxLen:(int)maxLen
{
	if (self.mNativeAd==nil) return;

    [MobFoxSDK setNativeAdTitle:self.mNativeAd
                     isRequired:bRequired
                     withMaxLen:maxLen];
}

-(void) setNativeAdDesc:(BOOL)bRequired withMaxLen:(int)maxLen
{
	if (self.mNativeAd==nil) return;

    [MobFoxSDK setNativeAdDesc:self.mNativeAd
                    isRequired:bRequired
                    withMaxLen:maxLen];
}

-(void) releaseNative
{
	if (self.mNativeAd!=nil)
	{
	    [MobFoxSDK releaseNativeAd:self.mNativeAd];
	}
}

//======================================================================================

//called when ad response is returned
- (void)nativeAdLoadFailed:(MFXNativeAd *)native withError:(NSString*)error
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxNativeAdDidFailToReceiveAdWithError >> %@",error);
    
    if (self.gameObject!=nil) UnitySendMessage([self.gameObject UTF8String], "nativeError", [error  UTF8String]);
}

- (void)nativeAdLoaded:(MFXNativeAd *)native
{
    NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxNativeAdDidLoad >> native loaded ###");
    
    if (self.gameObject!=nil)
    {
        NSDictionary* dictTexts     = [MobFoxSDK getNativeAdTexts:native];
        NSDictionary* dictImageUrls = [MobFoxSDK getNativeAdImageUrls:native];

        if ((dictTexts==nil) && (dictImageUrls==nil))
        {
            NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxNativeAdDidLoad >> No ad data ###");
            
            UnitySendMessage([self.gameObject UTF8String], "nativeError", "No ad data");
        } else {
            NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];

            NSString* title     = [dictTexts objectForKey:@"title"];
            NSString* desc      = [dictTexts objectForKey:@"desc"];
            NSString* rating    = [dictTexts objectForKey:@"rating"];
            NSString* sponsored = [dictTexts objectForKey:@"sponsored"];
            NSString* ctatext   = [dictTexts objectForKey:@"ctatext"];
            
            NSString* icon      = [dictImageUrls objectForKey:@"icon"];
            NSString* main      = [dictImageUrls objectForKey:@"main"];
            
            if (title    !=nil) [dict setObject:title     forKey:@"title"];
            if (desc     !=nil) [dict setObject:desc      forKey:@"desc"];
            if (rating   !=nil) [dict setObject:rating    forKey:@"rating"];
            if (sponsored!=nil) [dict setObject:sponsored forKey:@"sponsored"];
            if (ctatext  !=nil) [dict setObject:ctatext   forKey:@"ctatext"];
            if (icon     !=nil) [dict setObject:icon      forKey:@"iconImageUrl"];
            if (main     !=nil) [dict setObject:main      forKey:@"mainImageUrl"];

            NSError *error;
            NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict
                                                               options:NSJSONWritingPrettyPrinted // Pass 0 if you don't care about the readability of the generated string
                                                                 error:&error];
            
            if (! jsonData)
            {
                NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxNativeAdDidLoad >> No ad data ###");
                
                UnitySendMessage([self.gameObject UTF8String], "nativeError", "No ad data");
            } else {
                NSString *msg = [[NSString alloc] initWithData:jsonData
                                                      encoding:NSUTF8StringEncoding];

                NSLog(@"dbg: ### MobFoxUnityPlugin >> MobFoxNativeAdDidLoad %@",msg);
                
                UnitySendMessage([self.gameObject UTF8String], "nativeReady", [msg UTF8String]);
            }
        }
    }
}

- (void)nativeAdImagesReady:(MFXNativeAd *)native
{
}

- (void)nativeAdClicked:(MFXNativeAd *)native
{
}

@end

//======================================================================================
//======  P L U G I N                                                             ======
//======================================================================================

extern "C"
{
    static MobFoxUnityPlugin* plugin = [[MobFoxUnityPlugin alloc] init];
    
    void _setGameObject(const char* gameObject){
        plugin.gameObject = [NSString stringWithUTF8String:gameObject];
    }
    
    //----------------------------------------------------------------------

    void _showMessage(const char* val){
    
    	[plugin showMessage:[NSString stringWithUTF8String:val]];
    }
    
    char* _getSDKVersion() {
    
    	char* ver = "Unknown";
    	if (plugin != NULL)
    	{
    		ver = [plugin getSDKVersion];
    	}
        return ver;
    }

    //----------------------------------------------------------------------
    
    void _setCOPPA(bool subjectToCOPPA){
    
    	[plugin setCOPPA:subjectToCOPPA];
    }
    
    void _setDemoAge(const char* val){
    
    	[plugin setDemoAge:[NSString stringWithUTF8String:val]];
    }
    
    void _setDemoGender(const char* val){
    
    	[plugin setDemoGender:[NSString stringWithUTF8String:val]];
    }
    
    void _setDemoKeywords(const char* val){
    
    	[plugin setDemoKeywords:[NSString stringWithUTF8String:val]];
    }
    
    void _setLatitude(double val){
    
    	[plugin setLatitude:val];
    }
    
    void _setLongitude(double val){
    
    	[plugin setLongitude:val];
    }
    
    //----------------------------------------------------------------------
    
    int _createBanner(const char* invh, float x, float y, float width, float height){
        
        return [plugin createBanner:[NSString stringWithUTF8String:invh]
                            originX:x
                            originY:y
                          sizeWidth:width
                         sizeHeight:height];
        
    }
    
    void _hideBanner(){
        [plugin hideBanner];
    }
    
    void _showBanner(){
        [plugin showBanner];
    }
    
    void _setBannerRefresh (int intervalInSeconds){
        [plugin setBannerRefresh:intervalInSeconds];
    }
    
    void _setBannerFloorPrice (float floorPrice){
        [plugin setBannerFloorPrice:floorPrice];
    }

    void _releaseBanner(){
        [plugin releaseBanner];
    }
    
    //----------------------------------------------------------------------
    
    void _createInterstitial(const char* invh){
        [plugin createInterstitial:[NSString stringWithUTF8String:invh]];
    }
    
    void _showInterstitial(){
        [plugin showInterstitial];
    }
    
    void _setInterstitialFloorPrice (float floorPrice){
        [plugin setInterstitialFloorPrice:floorPrice];
    }
    
    void _releaseInterstitial(){
        [plugin releaseInterstitial];
    }

    //----------------------------------------------------------------------
    
    void _createNative(const char* invh){
        [plugin createNative:[NSString stringWithUTF8String:invh]];
    }

    void _setNativeFloorPrice (float floorPrice){
        [plugin setNativeFloorPrice:floorPrice];
    }

	void _callToActionClicked (){
        [plugin callToActionClicked];
	}

	void _setNativeAdContext (const char* adContext){
        [plugin setNativeAdContext:[NSString stringWithUTF8String:adContext]];
	}

	void _setNativeAdPlacementType (const char* adType){
        [plugin setNativeAdPlacementType:[NSString stringWithUTF8String:adType]];
	}

	void _setNativeAdIconImage ( bool bRequired, int size ){
        [plugin setNativeAdIconImage:bRequired withSize:size];
	}

	void _setNativeAdMainImage ( bool bRequired, int width, int height ){
        [plugin setNativeAdMainImage:bRequired withWidth:width andHeight:height];
	}

	void _setNativeAdTitle ( bool bRequired, int maxLength ){
        [plugin setNativeAdTitle:bRequired withMaxLen:maxLength];
	}

	void _setNativeAdDesc ( bool bRequired, int maxLength ){
        [plugin setNativeAdDesc:bRequired withMaxLen:maxLength];
	}
    
    void _releaseNative(){
        [plugin releaseNative];
    }
}
