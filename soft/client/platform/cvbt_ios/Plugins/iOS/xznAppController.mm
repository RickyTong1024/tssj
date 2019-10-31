
#import "xznAppController.h"
#import "xznManager.h"
#import <SCore/SCore.h>

@implementation xznAppController

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    // Override point for customization after application launch.
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    NSLog(@"hxc_text_didfinish");
    //  必须在初始化window之前调用
    [SPluginWrapper application:application didFinishLaunchingWithOptions:launchOptions];
    
    //_window = [[UIWindow alloc] initWithFrame:[UIScreen mainScreen].bounds];
    //_window.rootViewController = [[xzn_platformManager alloc] init];
    //[_window makeKeyAndVisible];
    //  初始化SDK(必须在window初始化完成之后调用)

    
    return YES;
}

- (void)applicationWillResignActive:(UIApplication *)application
{
    [SPluginWrapper applicationWillResignActive:application];
}

- (void)applicationDidEnterBackground:(UIApplication *)application
{
    [SPluginWrapper applicationDidEnterBackground:application];
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
    [SPluginWrapper applicationWillEnterForeground:application];
}

- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
{
    [SPluginWrapper application:application openURL:url sourceApplication:nil annotation:nil];
    return YES;
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(nullable NSString *)sourceApplication annotation:(id)annotation
{
    [SPluginWrapper application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
    return YES;
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey, id> *)options
{
    [SPluginWrapper application:application openURL:url options:options];
    return YES;
}

 - (void)applicationWillTerminate:(UIApplication *)application
{
    [SPluginWrapper applicationWillTerminate:application];
}

- (UIInterfaceOrientationMask)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window
{
    /*
     *  注意:
     *  如返回值interfaceOrientation为nil，则不使用SDK返回值
     *  返回值中的<UIInterfaceOrientationMaskAll>代表原来需要返回的值
     */
    NSNumber *interfaceOrientation = [SPluginWrapper application:application supportedInterfaceOrientationsForWindow:window];
    return interfaceOrientation ? [interfaceOrientation integerValue] : UIInterfaceOrientationMaskAll;
}

@end
IMPL_APP_CONTROLLER_SUBCLASS(xznAppController)
