#import "xznManager.h"
#import <SCore/SCore.h>
@implementation xznManager
int getlanguage()
{
    return 0;
}

void gameu_open_store()
{
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"itms://itunes.apple.com/cn/app/id967602545?mt=8"]];
}

char *platform_id()
{
    NSString* sl = @"BT_IOS";
    char* ret = nil;
    ret = (char*) malloc([sl length] + 1);
    memcpy(ret,[sl UTF8String],([sl length] + 1));
    return ret;
}

char *returnPrice(void *sku)
{
    return "";
}
@end

@implementation xzn_platformManager

    NSString *role_Level;
    NSString *role_id;
    NSString *role_name;
    NSString *role_server_id;
    NSString *role_server_name;
    NSString *cur_userId;

void platform_game_init()
{
    
    [[SYSDKPlatform getInstance] setCallback:^(SAction action, NSDictionary *result) {
        switch (action)
        {
            case SActionInitSuccess:
                NSLog(@"初始化成功");
                break;
            case SActionInitFailure:
                NSLog(@"初始化失败，error=%@", result);
                break;
            case SActionLoginSuccess:
                NSLog(@"登录成功，user=%@", result);
                
                if(cur_userId == nil)
                {
                    cur_userId = result[@"userId"];
                    NSString *_out = [NSString stringWithFormat:@"%@ %@ 0",result[@"userId"], result[@"token"]];
                    UnitySendMessage("game_node", "platform_login_success", [_out UTF8String]);
                }
                else
                {
                    cur_userId = [result objectForKey:@"userId"];
                    NSString *_out = [NSString stringWithFormat:@"%@ %@ 1",result[@"userId"], result[@"token"]];
                    UnitySendMessage("game_node", "platform_login_success", [_out UTF8String]);
                }
                [[SYSDKPlatform getInstance] doAntiAddictionQuery];
                break;
            case SActionLoginFailure:
                NSLog(@"登录失败，error=%@", result);
                UnitySendMessage("game_node", "platform_login_fail", "");
                break;
            case SActionAccountSwitchLogoutSuccess:
                NSLog(@"账号切换-注销成功");
                cur_userId = nil;
                UnitySendMessage("game_node", "platform_logout", "");
                break;
            case SActionAccountSwitchFailure:
                NSLog(@"账号切换失败，error=%@", result);
                break;
            case SActionPaySuccess:
                NSLog(@"支付成功");
                UnitySendMessage("game_node", "recharge_done", "");
                break;
            case SActionPayFailure:
                NSLog(@"支付失败，error=%@", result);
                UnitySendMessage("game_node", "recharge_cancel", "");
                break;
            case SActionAntiAddictionQuerySuccess:
                NSLog(@"防沉迷查询成功, result=%@", result);
                break;
            case SActionAntiAddictionQueryFailure:
                NSLog(@"防沉迷查询失败，error=%@", result);
                break;
            default:
                break;
        }
    }];
    
    NSDictionary *params = @{@"name" : @"x战娘",
                             @"shortName" : @"xzn",
                             @"direction" : @"0"};
    [[SYSDK getInstance] setDebug:false];
    [[SYSDK getInstance] initWithParams:params];
    
}

void platform_game_user_upgrade(int level)
{
    role_Level = [NSString stringWithFormat:@"%d", level];
    [[SYSDKPlatform getInstance] onRoleLevelUpgrade:role_Level];
}

void platform_game_login()
{
    [[SYSDKPlatform getInstance] doLogin];
}

void platform_game_logout()
{
    [[SYSDKPlatform getInstance] doAccountSwitch];
}

void platform_on_game_login(void *param)
{
    NSString *_param = [NSString stringWithUTF8String:param];
    NSArray *_params = [_param componentsSeparatedByString:@"_"];
    NSLog(@"%@",_params);
    if([_params count] >= 5)
    {
        role_id = _params[0];
        role_name = _params[1];
        role_Level = _params[2];
        role_server_id = _params[3];
        role_server_name = _params[4];
        
        if (![[SYSDKPlatform getInstance] isLogined])
        {
            NSLog(@"未登录不能调用setRoleInfo");
            return;
        }
        
        NSMutableDictionary *roleInfo = [NSMutableDictionary dictionary];
        roleInfo[@"roleId"] = role_id;
        roleInfo[@"roleName"] = role_name;
        roleInfo[@"zoneId"] = role_server_id;
        roleInfo[@"zoneName"] = @"测试服";
        roleInfo[@"partyName"] = @"公会名称";
        roleInfo[@"roleLevel"] = role_Level;
        roleInfo[@"roleVipLevel"] = @"16";
        roleInfo[@"balance"] = @"0";
        if([_params[5] isEqualToString:@"0"])
        {
            roleInfo[@"isNewRole"] = @"0";
        }
        else
        {
            roleInfo[@"isNewRole"] = @"1";
        }
        [[SYSDKPlatform getInstance] setRoleInfo:roleInfo];
    }
}

void platform_buy(void *body)
{
    NSString *_body = [NSString stringWithUTF8String:body];
    NSArray *_bodys = [_body componentsSeparatedByString:@"|"];
    NSString *player_id = _bodys[0];
    NSString *server_id = _bodys[1];
    NSString *recharge_id = _bodys[2];
    NSString *ios_id = _bodys[3];
    NSString *huodong_id = _bodys[4];
    NSString *entry_id = _bodys[5];
    NSString *recharge_price = _bodys[6];
    NSString *recharge_name = _bodys[7];
    NSString *recharge_desc = _bodys[8];
    NSString *pay_cpUserInfo =[NSString stringWithFormat:@"%@_%@_%@_%@_%@", player_id, server_id, recharge_id, huodong_id, entry_id];
    
    if (![[SYSDKPlatform getInstance] isLogined])
    {
        NSLog(@"未登录不能调用doPay");
        return;
    }
    NSArray *recharge_name_sp = [recharge_name componentsSeparatedByString:@"+"];
    NSMutableDictionary *payInfo = [NSMutableDictionary dictionary];
    payInfo[@"productId"] = ios_id;
    payInfo[@"productName"] = [recharge_name_sp objectAtIndex:0];
    payInfo[@"productDesc"] = recharge_desc;
    payInfo[@"productPrice"] = recharge_price;
    payInfo[@"productCount"] = @"1";
    payInfo[@"productType"] = @"0";
    payInfo[@"coinName"] = @"钻石";
    payInfo[@"coinRate"] = @"500";
    payInfo[@"extendInfo"] = pay_cpUserInfo;
    
    payInfo[@"roleId"] = player_id;
    payInfo[@"roleName"] = role_name;
    payInfo[@"zoneId"] = server_id;
    payInfo[@"zoneName"] = role_server_name;
    payInfo[@"partyName"] = @"nil";
    payInfo[@"roleLevel"] = role_Level;
    payInfo[@"roleVipLevel"] = @"16";
    payInfo[@"balance"] = @"0";
    
    [[SYSDKPlatform getInstance] doPay:payInfo];
}
@end
