#import "xznManager.h"
@implementation xznManager

void initNotify(){
    if ([UIApplication instancesRespondToSelector:@selector(registerUserNotificationSettings:)])
    {
        [[UIApplication sharedApplication] registerUserNotificationSettings:[UIUserNotificationSettings settingsForTypes:UIUserNotificationTypeAlert|UIUserNotificationTypeBadge|UIUserNotificationTypeSound categories:nil]];
    }
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];
}

void createNotify(void *text, int secondsFromNow) {
    UILocalNotification *newNotification = [[UILocalNotification alloc] init];
    if (newNotification) {
        newNotification.timeZone = [NSTimeZone defaultTimeZone];
        newNotification.repeatInterval = 0;
        newNotification.fireDate = [NSDate dateWithTimeIntervalSinceNow:secondsFromNow];
        newNotification.alertBody = [NSString stringWithUTF8String:text];
        newNotification.alertAction = @"打开";
        newNotification.applicationIconBadgeNumber = 1;
        newNotification.soundName = UILocalNotificationDefaultSoundName;
        [[UIApplication sharedApplication] scheduleLocalNotification:newNotification];
    }
    NSLog(@"Post new localNotification:%@", newNotification);
}

void cancelNotify() {
    [[UIApplication sharedApplication] cancelAllLocalNotifications];
}

//将文本复制到IOS剪贴板
- (void)objc_copyTextToClipboard : (NSString*)text
{
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    pasteboard.string = text;
}

int getlanguage()
{
    NSUserDefaults * defaults = [NSUserDefaults standardUserDefaults];
    
    NSArray * allLanguages = [defaults objectForKey:@"AppleLanguages"];
    
    NSString * preferredLang = [allLanguages objectAtIndex:0];
    
    if([preferredLang isEqualToString:@"zh-Hant"])
    {//当系统语言是中文或
        return 0;
    }
    else if([preferredLang isEqualToString:@"zh-Hans"])
    { // 繁体中文时
        return 1;
    }
    else{ //其它语言的情况下
        return 2;
    }
}

void returnPrice(void *p)
{

}

void game_open_store()
{
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"itms://itunes.apple.com/cn/app/id967602545?mt=8"]];
}

char *platform_id()
{
    NSString* sl = @"ios";
    char* ret = nil;
    ret = (char*) malloc([sl length] + 1);
    memcpy(ret,[sl UTF8String],([sl length] + 1));
    return ret;
}

@end

static xznManager *iosClipboard;
void copyTextToClipboard(const char *textList)
{
    NSString *text = [NSString stringWithUTF8String: textList] ;
    if(iosClipboard == NULL)
    {
        iosClipboard = [[xznManager alloc] init];
    }
    [iosClipboard objc_copyTextToClipboard: text];
}
